using BiasBox.Categories;
using BiasBox.Models;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BiasBox.Webshop.UI
{
    public class CartPage
    {
        public static int? LoggedInCustomerId { get; set; } = null;

        public static void AddToCart(int productId)
        {
            using (var context = new MyDbContext())
            {
                var existingItem = context.CartItems.FirstOrDefault(ci => ci.ProductId == productId && ci.CartId == (LoggedInCustomerId ?? 1));

                if (existingItem != null)
                {
                    existingItem.Quantity++;
                }
                else
                {
                    var product = context.Products.FirstOrDefault(p => p.ID == productId);
                    if (product != null)
                    {
                        context.CartItems.Add(new CartItem
                        {
                            CartId = (LoggedInCustomerId ?? 1),
                            ProductId = productId,
                            Quantity = 1
                        });
                    }
                }

                context.SaveChanges();
            }
        }

        public static void ShowCartPage()
        {
            string inputBuffer = "";
            string actionMessage = "Type R to remove, Q to update quantity, B to checkout or K to return.";
            List<CartItem> cartItems;
            decimal totalAmount = 0;
            bool awaitingInput = false;
            string currentAction = "";

            while (true)
            {
                Console.Clear();
                Header.DisplayHeader();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();

                using (var context = new MyDbContext())
                {
                    cartItems = context.CartItems
                        .Include(ci => ci.Product)
                        .Where(ci => ci.CartId == (LoggedInCustomerId ?? 1))
                        .ToList();

                    totalAmount = cartItems.Sum(ci => ci.Quantity * (ci.Product?.Price ?? 0));
                }

                var cartTable = new Table()
                    .AddColumn("Product ID")
                    .AddColumn("Product Name")
                    .AddColumn("Price")
                    .AddColumn("Quantity")
                    .AddColumn("Total");

                foreach (var item in cartItems)
                {
                    cartTable.AddRow(
                        item.ProductId.ToString(),
                        item.Product?.Name ?? "Unknown Product",
                        $"{(item.Product?.Price ?? 0):C}",
                        item.Quantity.ToString(),
                        $"{item.Quantity * (item.Product?.Price ?? 0):C}"
                    );
                }

                cartTable.Border = TableBorder.MinimalDoubleHead;
                cartTable.BorderStyle = new Style(foreground: Color.LightSkyBlue1);

                var cartPanel = new Panel(cartTable)
                    .Header("[DeepPink1]Your Cart[/]")
                    .BorderStyle(new Style(foreground: Color.LightSkyBlue1))
                    .Expand();

                var totalPanel = new Panel($"[bold white]Total Amount: {totalAmount:C}[/]")
                    .BorderStyle(new Style(foreground: Color.Green))
                    .Expand();

                var inputMessagePanel = new Panel($"[bold white]{actionMessage}[/]")
                    .BorderStyle(new Style(foreground: Color.DeepPink1))
                    .Expand();

                var inputBoxPanel = new Panel($"[bold white]Your Input:[/] {inputBuffer}")
                    .BorderStyle(new Style(foreground: Color.LightSkyBlue1))
                    .Expand();

                var grid = new Grid()
                    .AddColumn(new GridColumn().Width(80))
                    .AddRow(cartPanel)
                    .AddRow(totalPanel)
                    .AddRow(inputMessagePanel)
                    .AddRow(inputBoxPanel);

                var paddedLayout = new Padder(grid, new Padding(40, 0, 0, 0));
                AnsiConsole.Write(paddedLayout);

                var key = Console.ReadKey(intercept: true);

                if (key.Key == ConsoleKey.Enter)
                {
                    if (!awaitingInput)
                    {
                        switch (inputBuffer.ToLower())
                        {
                            case "r":
                                awaitingInput = true;
                                currentAction = "remove";
                                actionMessage = "[yellow]Enter Product ID to remove:[/]";
                                break;

                            case "q":
                                awaitingInput = true;
                                currentAction = "update";
                                actionMessage = "[yellow]Enter Product ID and Quantity (e.g., '44 2') to update quantity:[/]";
                                break;

                            case "b":
                                StartCheckout(cartItems, totalAmount);
                                return;

                            case "k":
                                return;

                            case "l":
                                CustomerPage.ShowCustomerPage();
                                return;

                            case "c":
                                CategoriesPage.ShowCategoriesPage();
                                return;

                            case "h":
                                Console.Clear();
                                Header.DisplayHeader();
                                Program.Space();
                                StartPageCode.StartCode();
                                return;

                            default:
                                actionMessage = "[red]Invalid command. Use R, Q, B, L, H, C or K.[/]";
                                break;
                        }

                        inputBuffer = "";
                    }
                    else
                    {
                        if (currentAction == "update")
                        {
                            var parts = inputBuffer.Split(' ');
                            if (parts.Length == 2 && int.TryParse(parts[0], out int productId) && int.TryParse(parts[1], out int newQuantity))
                            {
                                using (var context = new MyDbContext())
                                {
                                    var cartItem = context.CartItems.FirstOrDefault(ci => ci.ProductId == productId && ci.CartId == (LoggedInCustomerId ?? 1));
                                    if (cartItem != null)
                                    {
                                        cartItem.Quantity = newQuantity > 0 ? newQuantity : 1;
                                        context.SaveChanges();
                                        actionMessage = "[green]Quantity updated successfully![/]";
                                    }
                                    else
                                    {
                                        actionMessage = "[red]Item not found in cart.[/]";
                                    }
                                }
                            }
                            else
                            {
                                actionMessage = "[red]Invalid format. Use 'ProductID Quantity' (e.g., '44 2').[/]";
                            }

                            awaitingInput = false;
                            inputBuffer = "";
                        }
                        else if (currentAction == "remove")
                        {
                            if (int.TryParse(inputBuffer, out int productId))
                            {
                                using (var context = new MyDbContext())
                                {
                                    var cartItem = context.CartItems
                                        .FirstOrDefault(ci => ci.ProductId == productId && ci.CartId == (LoggedInCustomerId ?? 1));

                                    if (cartItem != null)
                                    {
                                        context.CartItems.Remove(cartItem);
                                        context.SaveChanges();
                                        actionMessage = "[green]Item removed successfully![/]";
                                    }
                                    else
                                    {
                                        actionMessage = "[red]Item not found in cart.[/]";
                                    }
                                }
                            }
                            else
                            {
                                actionMessage = "[red]Invalid ID. Please enter a valid Product ID.[/]";
                            }

                            awaitingInput = false;
                            inputBuffer = "";
                        }
                    }
                }
                else if (key.Key == ConsoleKey.Backspace && inputBuffer.Length > 0)
                {
                    inputBuffer = inputBuffer.Substring(0, inputBuffer.Length - 1);
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    inputBuffer += key.KeyChar;
                }
            }
        }

        private static void StartCheckout(List<CartItem> cartItems, decimal totalAmount)
        {

            using (var context = new MyDbContext())
            {
                // Betalningsmetoder
                var paymentMethods = context.PaymentMethods.ToList();
                var paymentMethod = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Select Payment Method:[/]")
                        .AddChoices(paymentMethods.Select(pm => pm.Name)));



                // Leveransmetoder
                var shippingMethods = context.ShippingMethods.ToList();
                var selectedShipping = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Select Shipping Method:[/]")
                        .AddChoices(shippingMethods.Select(sm => sm.Name)));

                var shippingMethod = shippingMethods.First(sm => sm.Name == selectedShipping);
                totalAmount += shippingMethod.Price;



                // Skapa och spara ordern först
                var newOrder = new Order
                {
                    CustomerId = LoggedInCustomerId ?? 1,
                    OrderDate = DateTime.Now,
                    TotalAmount = totalAmount,
                    PaymentMethodId = paymentMethods.First(pm => pm.Name == paymentMethod).ID,
                    ShippingMethodId = shippingMethod.ID,
                    Status = "Completed"
                };


                context.Orders.Add(newOrder);
                context.SaveChanges(); 



                // Lägg till OrderItems efter att ordern är sparad
                foreach (var item in cartItems)
                {
                    context.OrderItems.Add(new OrderItem
                    {
                        OrderId = newOrder.ID, 
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Product.Price
                    });



                    // Rensa från varukorgen
                    var cartItem = context.CartItems.First(ci => ci.ID == item.ID);
                    context.CartItems.Remove(cartItem);
                }

                context.SaveChanges(); 

                
                var grid = new Grid()
                     .AddColumn(new GridColumn().Width(50)) 
                     .AddColumn(new GridColumn().Width(80)) 
                     .AddRow(
                      new Markup(""), 
                      new Panel($@"
    [bold green]Your order has been placed successfully![/]
    [yellow]Payment Method:[/] {paymentMethod}
    [yellow]Shipping Method:[/] {selectedShipping}
    [yellow]Total Amount Paid (Including Shipping):[/] {totalAmount:C}
    Press any key to return...
").BorderStyle(new Style(foreground: Color.Green))
                                             );

                
                AnsiConsole.Write(grid);
                Console.ReadKey();
            }


        }
    }
}

