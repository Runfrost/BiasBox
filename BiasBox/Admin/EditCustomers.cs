using BiasBox.Models;
using BiasBox.Webshop.UI;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;

namespace BiasBox.Admin
{
    public class EditCustomers
    {
        public static void ShowEditCustomers()
        {
            string inputBuffer = "";
            string actionMessage = "[bold white]Press E to edit customer, V to view order history, or B to return.[/]";

            while (true)
            {
                Console.Clear();
                Header.DisplayHeader();
                Program.Space2();

                List<Customer> customers;

                using (var context = new MyDbContext())
                {
                    customers = context.Customers.ToList();
                }

                // Info panel
                var infoPanel = new Panel($"[bold white]{actionMessage}[/]")
                    .BorderStyle(new Style(foreground: Color.DeepPink1))
                    .Expand();

                // Input panel 
                var inputPanel = new Panel($"[bold white]Your Input:[/] {inputBuffer}")
                    .BorderStyle(new Style(foreground: Color.LightSkyBlue1))
                    .Expand();

                // Skapa tabell för kunder
                var customersPanel = new Panel("[bold yellow]No customers found.[/]")
                    .BorderStyle(new Style(foreground: Color.LightSkyBlue1))
                    .Expand();

                if (customers.Any())
                {
                    var customersTable = new Table()
                        .AddColumn("ID")
                        .AddColumn("Name")
                        .AddColumn("Address")
                        .AddColumn("Phone")
                        .AddColumn("Email");

                    foreach (var customer in customers)
                    {
                        customersTable.AddRow(customer.ID.ToString(), customer.Name, customer.Address, customer.PhoneNumber, customer.Email);
                    }

                    customersTable.Border = TableBorder.Minimal;
                    customersTable.BorderStyle = new Style(foreground: Color.LightSkyBlue1);

                    customersPanel = new Panel(customersTable)
                        .Header("[DeepPink1]Customers[/]")
                        .BorderStyle(new Style(foreground: Color.LightSkyBlue1))
                        .Expand();
                }

                
                var grid = new Grid()
                    .AddColumn(new GridColumn().Width(80))
                    .AddRow(infoPanel)
                    .AddRow(inputPanel)
                    .AddRow(customersPanel);

                var paddedLayout = new Padder(grid, new Padding(40, 0, 0, 0));

                AnsiConsole.Write(paddedLayout);

                

                var key = Console.ReadKey(intercept: true).Key;
                inputBuffer = key.ToString().ToLower();

                switch (inputBuffer)
                {
                    case "e":
                        EditCustomer();
                        break;

                    case "v":
                        ViewOrderHistory();
                        break;

                    case "b":
                        AdminPage.ShowAdminPage(); // 🔄 Gå tillbaka till AdminPage
                        return;

                    default:
                        ShowErrorMessage("Invalid input. Use E, V, or B.");
                        break;
                }
            }
        }

        
        private static void EditCustomer()
        {
            using (var context = new MyDbContext())
            {
                int customerId = Convert.ToInt32(AskQuestion("Enter the Customer ID to edit:"));
                var customer = context.Customers.FirstOrDefault(c => c.ID == customerId);

                if (customer == null)
                {
                    ShowErrorMessage("Customer not found.");
                    return;
                }

                string newName = AskQuestion($"Enter new name for {customer.Name} (Press Enter to keep current):", customer.Name);
                string newAddress = AskQuestion($"Enter new address (Press Enter to keep current):", customer.Address);
                string newPhone = AskQuestion($"Enter new phone number (Press Enter to keep current):", customer.PhoneNumber);
                string newEmail = AskQuestion($"Enter new email (Press Enter to keep current):", customer.Email);

                customer.Name = newName;
                customer.Address = newAddress;
                customer.PhoneNumber = newPhone;
                customer.Email = newEmail;

                context.SaveChanges();
                ShowSuccessMessage("Customer updated successfully!");
            }
        }

        
        private static void ViewOrderHistory()
        {
            Program.Space();

            using (var context = new MyDbContext())
            {
                int customerId = Convert.ToInt32(AskQuestion("Enter the Customer ID to view order history:"));
                var customer = context.Customers.FirstOrDefault(c => c.ID == customerId);
                var orders = context.Orders
                    .Where(o => o.CustomerId == customerId)
                    .OrderByDescending(o => o.OrderDate) 
                    .ToList();

                if (customer == null)
                {
                    ShowErrorMessage("Customer not found.");
                    return;
                }

                if (!orders.Any())
                {
                    ShowErrorMessage($"No orders found for {customer.Name}.");
                    return;
                }

                Console.Clear();
                Header.DisplayHeader();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();


                // Dela upp ordern i två grupper 
                var firstHalf = orders.Take(5).ToList();
                var secondHalf = orders.Skip(5).Take(5).ToList();

                
                var ordersTable = new Table()
                    .AddColumn(new TableColumn("Order ID").Width(12))
                    .AddColumn(new TableColumn("Order Date").Width(15))
                    .AddColumn(new TableColumn("Total Amount").Width(18))
                    .AddColumn(new TableColumn("Status").Width(15))
                    .AddColumn(new TableColumn("Order ID").Width(12))
                    .AddColumn(new TableColumn("Order Date").Width(15))
                    .AddColumn(new TableColumn("Total Amount").Width(18))
                    .AddColumn(new TableColumn("Status").Width(15));


                int maxRows = Math.Max(firstHalf.Count, secondHalf.Count);
                for (int i = 0; i < maxRows; i++)
                {
                    var leftOrder = i < firstHalf.Count ? firstHalf[i] : null;
                    var rightOrder = i < secondHalf.Count ? secondHalf[i] : null;

                    ordersTable.AddRow(
                        leftOrder?.ID.ToString() ?? " ", leftOrder?.OrderDate.ToString("yyyy-MM-dd") ?? " ",
                        leftOrder?.TotalAmount.ToString("C") ?? " ", leftOrder?.Status ?? " ",
                        rightOrder?.ID.ToString() ?? " ", rightOrder?.OrderDate.ToString("yyyy-MM-dd") ?? " ",
                        rightOrder?.TotalAmount.ToString("C") ?? " ", rightOrder?.Status ?? " "
                    );
                }

                ordersTable.Border = TableBorder.Rounded;
                ordersTable.BorderStyle = new Style(foreground: Color.LightSkyBlue1);

                
                var ordersPanel = new Panel(ordersTable)
                    .Header($"[DeepPink1]Order History for {customer.Name}[/]") 
                    .BorderStyle(new Style(foreground: Color.LightSkyBlue1))
                    .Expand();

                
                var paddedOrdersPanel = new Padder(ordersPanel, new Padding(10, 2, 10, 2));

                AnsiConsole.Write(paddedOrdersPanel);
                Console.ReadKey();
            }
        }



        
        private static string AskQuestion(string question, string defaultValue = "")
        {
            string inputBuffer = "";
            string actionMessage = $"[bold yellow]{question}[/]";

            while (true)
            {
                Console.Clear();
                Header.DisplayHeader();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();

                
                var infoPanel = new Panel(actionMessage)
                    .BorderStyle(new Style(foreground: Color.DeepPink1))
                    .Expand();

                
                var inputPanel = new Panel($"[bold white]Your Input:[/] {inputBuffer}")
                    .BorderStyle(new Style(foreground: Color.LightSkyBlue1))
                    .Expand();

                
                var grid = new Grid()
                    .AddColumn(new GridColumn().Width(80))
                    .AddRow(infoPanel)
                    .AddRow(inputPanel);

                var paddedLayout = new Padder(grid, new Padding(40, 0, 0, 0));

                AnsiConsole.Write(paddedLayout);

                var key = Console.ReadKey(intercept: true);
                if (key.Key == ConsoleKey.Enter)
                {
                    return string.IsNullOrWhiteSpace(inputBuffer) ? defaultValue : inputBuffer;
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

        // Felmeddelande
        private static void ShowErrorMessage(string message)
        {
            AnsiConsole.MarkupLine($"[red]{message}[/]");
            Console.ReadKey(true);
        }

        // Bekräftelse
        private static void ShowSuccessMessage(string message)
        {
            AnsiConsole.MarkupLine($"[green]{message}[/]");
            Console.ReadKey(true);
        }
    }
}
