using BiasBox.Models;
using BiasBox.Webshop.UI;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BiasBox.Categories
{
    public class Accessories
    {
        public static void ShowAccessories()
        {
            ConsoleUtils.HideCursor();

            using (var context = new MyDbContext())
            {
                var accessories = context.Products
                    .Where(p => p.Categories.Any(c => c.Name == "Accessories"))
                    .ToList();

                if (!accessories.Any())
                {
                    Console.Clear();
                    Header.DisplayHeader();
                    Console.WriteLine("\n\n\n");

                    AnsiConsole.Markup("[bold red]No products were found in the Accessories category[/]");
                    Console.WriteLine("\nPress Enter to return...");
                    ConsoleUtils.ShowCursor();
                    Console.ReadLine();
                    return;
                }

                var productTable = new Table()
                    .AddColumn("Product ID")
                    .AddColumn("Product Name")
                    .AddColumn("Price");

                foreach (var product in accessories)
                {
                    productTable.AddRow(
                        product.ID.ToString(),
                        product.Name,
                        $"{product.Price:C}"
                    );
                }

                productTable.Border = TableBorder.Minimal;
                productTable.BorderStyle = new Style(foreground: Color.LightSkyBlue1);

                var paddedProductTable = new Padder(productTable, new Padding(10, 1, 10, 1));

                var productPanel = new Panel(paddedProductTable)
                    .Header("[DeepPink1]Accessories[/]")
                    .BorderStyle(new Style(foreground: Color.LightSkyBlue1))
                    .Expand();

                string inputMessage = "[DeepPink1]Display information:[/] [white]Enter the product ID.[/]                        [DeepPink1]Purchase the product:[/] [white]Enter the product ID followed by a dot.[/]";
                string productInfo = "";
                string inputBuffer = "";

                var inputMessagePanel = new Panel(inputMessage)
                    .BorderStyle(new Style(foreground: Color.DeepPink1))
                    .Expand();

                var productInfoPanel = new Panel(productInfo)
                    .Header("[LightSkyBlue1]Product Information[/]")
                    .BorderStyle(new Style(foreground: Color.DeepPink1))
                    .Expand();

                var inputBoxPanel = new Panel($"[DeepPink1]Input:[/] {inputBuffer}")
                    .BorderStyle(new Style(foreground: Color.LightSkyBlue1))
                    .Expand();

                while (true)
                {
                    Console.Clear();
                    Header.DisplayHeader();
                    Console.WriteLine("\n");

                    var leftColumn = new Rows(productPanel);
                    var rightColumn = new Rows(inputMessagePanel, productInfoPanel, inputBoxPanel);

                    var grid = new Grid()
                        .AddColumn(new GridColumn().Width(70))
                        .AddColumn(new GridColumn().Width(40))
                        .AddRow(leftColumn, rightColumn);

                    var paddedGrid = new Padder(grid, new Padding(35, 0, 0, 0));

                    AnsiConsole.Write(paddedGrid);

                    var key = Console.ReadKey(intercept: true);

                    switch (key.Key)
                    {
                        case ConsoleKey.H:
                            Console.Clear();
                            Header.DisplayHeader();
                            Program.Space();
                            StartPageCode.StartCode();
                            return;

                        case ConsoleKey.L:
                            ConsoleUtils.ShowCursor();
                            CustomerPage.ShowCustomerPage();
                            return;

                        case ConsoleKey.K:
                            ConsoleUtils.ShowCursor();
                            CartPage.ShowCartPage();
                            return;

                        case ConsoleKey.Enter:
                            if (inputBuffer.Contains("."))
                            {
                                var parts = inputBuffer.Split('.');
                                if (int.TryParse(parts[0], out int productId))
                                {
                                    using (var dbContext = new MyDbContext())
                                    {
                                        var selectedProduct = dbContext.Products.FirstOrDefault(p => p.ID == productId);

                                        if (selectedProduct != null)
                                        {
                                            CartPage.AddToCart(productId);
                                            inputMessagePanel = new Panel($"[green]Product '{selectedProduct.Name}' added to your cart.[/]")
                                                .BorderStyle(new Style(foreground: Color.Green))
                                                .Expand();
                                        }
                                        else
                                        {
                                            inputMessagePanel = new Panel("[red]No product was found with the given ID. Please try again.[/]")
                                                .BorderStyle(new Style(foreground: Color.Red))
                                                .Expand();
                                        }
                                    }
                                }
                                else
                                {
                                    inputMessagePanel = new Panel("[red]Invalid product ID format. Use 'ProductID.' to add to cart.[/]")
                                        .BorderStyle(new Style(foreground: Color.Red))
                                        .Expand();
                                }
                                inputBuffer = "";
                            }
                            else if (int.TryParse(inputBuffer, out int infoProductId)) // För att visa produktinformation
                            {
                                var selectedProduct = accessories.FirstOrDefault(p => p.ID == infoProductId);

                                if (selectedProduct != null)
                                {
                                    productInfo = $"[bold]Product Name:[/] {selectedProduct.Name}\n" +
                                                  $"[bold]Price:[/] {selectedProduct.Price:C}\n" +
                                                  $"[bold]Description:[/] {selectedProduct.Info}";

                                    productInfoPanel = new Panel(productInfo)
                                        .Header("[LightSkyBlue1]Product Information[/]")
                                        .BorderStyle(new Style(foreground: Color.DeepPink1))
                                        .Expand();
                                }
                                else
                                {
                                    inputMessagePanel = new Panel("[red]No product was found with the given ID. Please try again.[/]")
                                        .BorderStyle(new Style(foreground: Color.Red))
                                        .Expand();
                                }
                                inputBuffer = "";
                            }
                            break;

                        case ConsoleKey.Backspace:
                            if (inputBuffer.Length > 0)
                            {
                                inputBuffer = inputBuffer.Substring(0, inputBuffer.Length - 1);
                            }
                            break;

                        default:
                            if (char.IsDigit(key.KeyChar) || key.KeyChar == '.')
                            {
                                inputBuffer += key.KeyChar;
                            }
                            break;
                    }

                    inputBoxPanel = new Panel($"[DeepPink1]Input:[/] {inputBuffer}")
                        .BorderStyle(new Style(foreground: Color.DeepSkyBlue1))
                        .Expand();
                }
            }
        }
    }
}
