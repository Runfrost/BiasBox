using BiasBox.Models;
using BiasBox.Webshop.UI;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BiasBox.Admin
{
    public class SelectedProducts
    {
        public static void ShowSelectedProducts()
        {
            string inputBuffer = "";
            string actionMessage = "[bold white]Press M to mark products, U to unmark, B to return.[/]";
            bool awaitingProductInput = false;
            string currentAction = "";

            while (true)
            {
                Console.Clear();
                Header.DisplayHeader();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();


                // Hämta utvalda produkter
                List<Product> featuredProducts;
                using (var context = new MyDbContext())
                {
                    featuredProducts = context.Products.Where(p => p.IsFeatured).ToList();
                }

                
                var infoPanel = new Panel($"[bold white]{actionMessage}[/]")
                    .BorderStyle(new Style(foreground: Color.DeepPink1))
                    .Expand();

                var inputPanel = new Panel($"[bold white]Your Input:[/] {inputBuffer}")
                    .BorderStyle(new Style(foreground: Color.LightSkyBlue1))
                    .Expand();

                var featuredPanel = CreateFeaturedProductsPanel(featuredProducts);


                var grid = new Grid()
                    .AddColumn(new GridColumn().Width(80))
                    .AddRow(infoPanel)
                    .AddRow(inputPanel)
                    .AddRow(featuredPanel);

                var paddedLayout = new Padder(grid, new Padding(40, 0, 0, 0));
                AnsiConsole.Write(paddedLayout);

                
                var keyInfo = Console.ReadKey(intercept: true);
                var keyChar = keyInfo.KeyChar;

                switch (keyInfo.Key)
                {
                    case ConsoleKey.Enter:
                        if (!awaitingProductInput)
                        {
                            switch (inputBuffer.ToLower())
                            {
                                case "m":
                                    actionMessage = "[yellow]Enter product IDs to mark as featured (comma-separated):[/]";
                                    awaitingProductInput = true;
                                    currentAction = "mark";
                                    break;

                                case "u":
                                    actionMessage = "[yellow]Enter product IDs to unmark as featured (comma-separated):[/]";
                                    awaitingProductInput = true;
                                    currentAction = "unmark";
                                    break;

                                case "b":
                                    AdminPage.ShowAdminPage(); 
                                    return;

                                default:
                                    ShowErrorMessage("Invalid input. Use M, U, or B.");
                                    break;
                            }
                        }
                        else
                        {
                            if (currentAction == "mark")
                                MarkFeaturedProducts(inputBuffer);
                            else if (currentAction == "unmark")
                                UnmarkFeaturedProducts(inputBuffer);

                            awaitingProductInput = false;
                            actionMessage = "[bold white]Press M to mark products, U to unmark, B to return.[/]";
                        }
                        inputBuffer = "";
                        break;

                    case ConsoleKey.Backspace:
                        if (inputBuffer.Length > 0)
                            inputBuffer = inputBuffer.Substring(0, inputBuffer.Length - 1);
                        break;

                    default:
                        if (!char.IsControl(keyChar))
                            inputBuffer += keyChar;
                        break;
                }
            }
        }

        // Skapa panel för utvalda produkter
        private static Panel CreateFeaturedProductsPanel(List<Product> featuredProducts)
        {
            if (!featuredProducts.Any())
            {
                return new Panel("[bold yellow]No featured products yet.[/]")
                    .BorderStyle(new Style(foreground: Color.LightSkyBlue1))
                    .Expand();
            }

            var featuredTable = new Table()
                .AddColumn("ID")
                .AddColumn("Product Name")
                .AddColumn("Price");

            foreach (var product in featuredProducts)
            {
                featuredTable.AddRow(product.ID.ToString(), product.Name, $"{product.Price:C}");
            }

            featuredTable.Border = TableBorder.Minimal;
            featuredTable.BorderStyle = new Style(foreground: Color.LightSkyBlue1);

            return new Panel(featuredTable)
                .Header("[DeepPink1]Featured Products[/]")
                .BorderStyle(new Style(foreground: Color.LightSkyBlue1))
                .Expand();
        }

        // Markera produkter som utvalda
        private static void MarkFeaturedProducts(string input)
        {
            using (var context = new MyDbContext())
            {
                var productIds = ParseProductIds(input);

                var products = context.Products.Where(p => productIds.Contains(p.ID)).ToList();
                if (!products.Any())
                {
                    ShowErrorMessage("No valid products found.");
                    return;
                }

                foreach (var product in products)
                {
                    product.IsFeatured = true;
                }

                context.SaveChanges();
                ShowSuccessMessage("Selected products marked as featured.");
            }
        }

        // Avmarkera produkter som utvalda
        private static void UnmarkFeaturedProducts(string input)
        {
            using (var context = new MyDbContext())
            {
                var productIds = ParseProductIds(input);

                var products = context.Products.Where(p => productIds.Contains(p.ID) && p.IsFeatured).ToList();
                if (!products.Any())
                {
                    ShowErrorMessage("No valid featured products found.");
                    return;
                }

                foreach (var product in products)
                {
                    product.IsFeatured = false;
                }

                context.SaveChanges();
                ShowSuccessMessage("Selected products unmarked as featured.");
            }
        }

        
        private static List<int> ParseProductIds(string input)
        {
            return input.Split(',')
                        .Select(id => int.TryParse(id.Trim(), out var num) ? num : -1)
                        .Where(id => id > 0)
                        .ToList();
        }

       
        private static void ShowSuccessMessage(string message)
        {
            AnsiConsole.Write(new Panel($"[bold green]{message}[/]")
                .BorderStyle(new Style(foreground: Color.Green))
                .Expand());
            System.Threading.Thread.Sleep(2000);
        }

       
        private static void ShowErrorMessage(string message)
        {
            AnsiConsole.Write(new Panel($"[bold red]{message}[/]")
                .BorderStyle(new Style(foreground: Color.Red))
                .Expand());
            System.Threading.Thread.Sleep(2000);
        }
    }
}
