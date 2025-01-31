using BiasBox.Webshop.UI;
using BiasBox.Models;
using Spectre.Console;
using System.Diagnostics;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BiasBox.Admin
{
    public class ChangeProducts
    {
        public static async Task ShowChangeProducts()
        {
            string input = "";
            string actionMessage = "[bold white]Press A to add a product, E to edit, D to delete, or B to return.[/]";

            while (true)
            {
                Console.Clear();
                Header.DisplayHeader();
                Program.Space2();

                ShowPanel(actionMessage, input);

                var key = Console.ReadKey(intercept: true).Key;

                switch (key)
                {
                    case ConsoleKey.A:
                        await AddNewProduct();
                        break;

                    case ConsoleKey.E:
                        await EditProduct();
                        break;

                    case ConsoleKey.D:
                        await DeleteProduct();
                        break;

                    case ConsoleKey.B:
                        AdminPage.ShowAdminPage();
                        return;

                    default:
                        ShowMessage("Invalid input. Use A, E, D, or B.", "red");
                        break;
                }
            }
        }

        // Lägg till ny produkt
        private static async Task AddNewProduct()
        {
            using var context = new MyDbContext();

            string name = AskInput("Enter product name:");
            string info = AskInput("Enter product description:");
            decimal price = Convert.ToDecimal(AskInput("Enter product price:"));
            int inventory = Convert.ToInt32(AskInput("Enter product inventory count:"));
            int supplierId = Convert.ToInt32(AskInput("Enter supplier ID:"));

            var newProduct = new Product
            {
                Name = name,
                Info = info,
                Price = price,
                Inventory = inventory,
                SupplierId = supplierId,
                IsFeatured = false
            };

            context.Products.Add(newProduct);

            var stopwatch = Stopwatch.StartNew(); // Starta tidtagning
            await context.SaveChangesAsync(); // Asynkron lagring
            stopwatch.Stop(); // Stoppa tidtagning

            ShowMessage($"Product added successfully! (Time taken: {stopwatch.ElapsedMilliseconds} ms)", "green");
        }

        // Redigera produkt
        private static async Task EditProduct()
        {
            using var context = new MyDbContext();

            int productId = Convert.ToInt32(AskInput("Enter the Product ID to edit:"));
            var product = await context.Products.FirstOrDefaultAsync(p => p.ID == productId); // Asynkron frågning

            if (product == null)
            {
                ShowMessage("Product not found.", "red");
                return;
            }

            product.Name = AskInput($"Enter new name for {product.Name} (Press Enter to keep current):", product.Name);
            product.Info = AskInput($"Enter new description (Press Enter to keep current):", product.Info);
            product.Price = Convert.ToDecimal(AskInput($"Enter new price (Press Enter to keep current):", product.Price.ToString()));
            product.Inventory = Convert.ToInt32(AskInput($"Enter new inventory count (Press Enter to keep current):", product.Inventory.ToString()));

            //context.SaveChanges();
            await context.SaveChangesAsync();
            ShowMessage("Product updated successfully!", "green");
        }

        // Ta bort produkt
        private static async Task DeleteProduct()
        {
            using var context = new MyDbContext();

            int productId = Convert.ToInt32(AskInput("Enter the Product ID to delete:"));
            var product = await context.Products.FirstOrDefaultAsync(p => p.ID == productId); // Asynkron frågning

            if (product == null)
            {
                ShowMessage("Product not found.", "red");
                return;
            }

            context.Products.Remove(product);
            var stopwatch = Stopwatch.StartNew(); 
            await context.SaveChangesAsync(); 
            stopwatch.Stop(); 

            ShowMessage($"Product added successfully! (Time taken: {stopwatch.ElapsedMilliseconds} ms)", "green");
        }

       
        private static string AskInput(string question, string defaultValue = "")
        {
            string input = "";
            while (true)
            {
                Console.Clear();
                Header.DisplayHeader();
                Console.WriteLine();

                ShowPanel(question, input);

                var key = Console.ReadKey(intercept: true);

                if (key.Key == ConsoleKey.Enter)
                    return string.IsNullOrWhiteSpace(input) ? defaultValue : input;

                if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                    input = input.Substring(0, input.Length - 1);
                else if (!char.IsControl(key.KeyChar))
                    input += key.KeyChar;
            }
        }

        // Panel input
        private static void ShowPanel(string message, string input)
        {
            var infoPanel = new Panel($"[bold yellow]{message}[/]")
                .BorderStyle(new Style(foreground: Color.DeepPink1))
                .Expand();

            var inputPanel = new Panel($"[bold white]Your Input:[/] {input}")
                .BorderStyle(new Style(foreground: Color.LightSkyBlue1))
                .Expand();

            var grid = new Grid()
                .AddColumn(new GridColumn().Width(80))
                .AddRow(infoPanel)
                .AddRow(inputPanel);

            AnsiConsole.Write(new Padder(grid, new Padding(40, 0, 0, 0)));
        }

       
        private static void ShowMessage(string message, string color)
        {
            Console.Clear();
            Header.DisplayHeader();
            Console.WriteLine();

            var panel = new Panel($"[bold white]{message}[/]")
                .BorderStyle(new Style(foreground: color == "green" ? Color.Green : Color.Red))
                .Expand();

            var grid = new Grid()
                .AddColumn(new GridColumn().Width(80))
                .AddRow(panel);

            AnsiConsole.Write(new Padder(grid, new Padding(40, 10, 0, 0)));

            Console.ReadKey(true);
        }
    }
}
