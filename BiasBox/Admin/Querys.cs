using BiasBox.Models;
using BiasBox.Webshop.UI;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace BiasBox.Admin
{
    public class Querys
    {
        public static void ShowStatistics()
        {
            while (true)
            {
                Console.Clear();
                Header.DisplayHeader();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();

                // Statistikdata
                var bestSellingProducts = GetBestSellingProducts();
                var salesBySuppliers = GetSalesBySuppliers();

                // Skapa statistik tabeller
                var productTable = CreateTable("", new[] { "Product", "Total Sold" },
                    bestSellingProducts.Select(p => new[] { p.ProductName, p.TotalSold.ToString() }));

                var supplierTable = CreateTable("", new[] { "Supplier", "Total Sold" },
                    salesBySuppliers.Select(s => new[] { s.SupplierName, s.TotalSold.ToString() }));

               
                var statsGrid = new Grid()
                    .AddColumn(new GridColumn().Width(90))
                    .AddColumn(new GridColumn().Width(90))
                    .AddRow(
                        new Panel(productTable)
                            .Header("[Green] 🏆 Top Selling Products [/]")
                            .Border(BoxBorder.Double)
                            .BorderStyle(new Style(foreground: Color.LightSkyBlue1))
                            .Padding(2, 1)
                            .Expand(),
                        new Panel(supplierTable)
                            .Header("[Green] 🏢 Sales by Supplier [/]")
                            .Border(BoxBorder.Double)
                            .BorderStyle(new Style(foreground: Color.LightSkyBlue1))
                            .Padding(2, 1)
                            .Expand()
                    );

               
                var paddedStatsGrid = new Padder(statsGrid, new Padding(20, 2, 20, 2));
                AnsiConsole.Write(paddedStatsGrid);

               
                var actionMessage = "[bold white]Press B to return to AdminPage.[/]";
                AnsiConsole.Write(new Panel(actionMessage)
                    .BorderStyle(new Style(foreground: Color.DeepPink1))
                    .Padding(1, 0)
                    .Expand());

               
                var key = Console.ReadKey(intercept: true).Key;
                switch (key)
                {
                    case ConsoleKey.B:
                        AdminPage.ShowAdminPage(); 
                        return;

                    default:
                        ShowErrorMessage("Invalid input. Press 'B' to return.");
                        break;
                }
            }
        }

        // Hämta bäst säljande produkter
        private static List<(string ProductName, int TotalSold)> GetBestSellingProducts()
        {
            using var connection = new SqlConnection("Server=.\\SQLExpress;Database=BiasBox;Trusted_Connection=True;");
            string query = @"
                SELECT TOP 3 p.Name AS ProductName, SUM(oi.Quantity) AS TotalSold
                FROM OrderItems oi
                JOIN Products p ON oi.ProductId = p.ID
                GROUP BY p.Name
                ORDER BY TotalSold DESC";

            return connection.Query<(string ProductName, int TotalSold)>(query).ToList();
        }

        // Hämta försäljning per leverantör
        private static List<(string SupplierName, int TotalSold)> GetSalesBySuppliers()
        {
            using var connection = new SqlConnection("Server=.\\SQLExpress;Database=BiasBox;Trusted_Connection=True;");
            string query = @"
                SELECT s.Name AS SupplierName, SUM(oi.Quantity) AS TotalSold
                FROM OrderItems oi
                JOIN Products p ON oi.ProductId = p.ID
                JOIN Suppliers s ON p.SupplierId = s.ID
                GROUP BY s.Name
                ORDER BY TotalSold DESC";

            return connection.Query<(string SupplierName, int TotalSold)>(query).ToList();
        }

       
        private static Table CreateTable(string title, string[] columns, IEnumerable<string[]> rows)
        {
            var table = new Table().Title($"[bold]{title}[/]");

            foreach (var column in columns)
                table.AddColumn(column);

            foreach (var row in rows)
                table.AddRow(row);

            table.Border = TableBorder.Minimal;
            table.BorderStyle = new Style(foreground: Color.LightSkyBlue1);

            return table;
        }

       
        private static void ShowErrorMessage(string message)
        {
            var errorPanel = new Panel($"[red]{message}[/]")
                .BorderStyle(new Style(foreground: Color.Red))
                .Padding(1, 0)
                .Expand();

            AnsiConsole.Write(errorPanel);
            System.Threading.Thread.Sleep(2000);
        }
    }
}
