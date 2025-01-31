using BiasBox.Models;
using BiasBox.Webshop.UI;
using Spectre.Console;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace BiasBox.Webshop.UI
{
    internal class CustomerPage
    {
        public static int? LoggedInCustomerId { get; private set; } = null;
        private static string LoggedInCustomerName { get; set; } = string.Empty;

        public static void ShowCustomerPage()
        {
            if (LoggedInCustomerId != null)
            {
                ShowLoggedInCustomerMenu();
                return;
            }

            ConsoleUtils.HideCursor(); // Dölj markören

            string username = "";
            string password = "";
            bool isEnteringUsername = true;

            while (true)
            {
                Console.Clear();
                Header.DisplayHeader();
                Console.WriteLine();

                var usernameBox = new Panel($"[bold white]Enter Username:[/] {username}")
                    .BorderStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .Expand();

                var passwordBox = new Panel($"[bold white]Enter Password:[/] {new string('*', password.Length)}")
                    .BorderStyle(new Style(foreground: Color.DeepPink1))
                    .Expand();

                var grid = new Grid()
                    .AddColumn(new GridColumn().Width(50))
                    .AddRow(usernameBox)
                    .AddEmptyRow()
                    .AddRow(passwordBox);

                var paddedGrid = new Padder(grid, new Padding(55, 5, 0, 0));
                AnsiConsole.Write(paddedGrid);

                var key = Console.ReadKey(intercept: true);

                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        if (isEnteringUsername)
                        {
                            isEnteringUsername = false;
                        }
                        else
                        {
                            using (var context = new MyDbContext())
                            {
                                var customer = context.Customers
                                    .FirstOrDefault(c => c.Username == username && c.Password == password);

                                if (customer != null)
                                {
                                    LoggedInCustomerId = customer.ID;
                                    LoggedInCustomerName = customer.Name;
                                    ShowLoggedInCustomerMenu();
                                    return;
                                }
                                else
                                {
                                    username = "";
                                    password = "";
                                    isEnteringUsername = true;
                                    AnsiConsole.MarkupLine("[bold red]Invalid username or password. Please try again.[/]");
                                    Console.ReadLine();
                                }
                            }
                        }
                        break;

                    case ConsoleKey.Backspace:
                        if (isEnteringUsername && username.Length > 0)
                        {
                            username = username.Substring(0, username.Length - 1);
                        }
                        else if (!isEnteringUsername && password.Length > 0)
                        {
                            password = password.Substring(0, password.Length - 1);
                        }
                        break;

                    default:
                        if (!char.IsControl(key.KeyChar))
                        {
                            if (isEnteringUsername) username += key.KeyChar;
                            else password += key.KeyChar;
                        }
                        break;
                }
            }
        }

        private static void ShowLoggedInCustomerMenu()
        {
            string inputBuffer = "";

            string purchaseHistory = "[italic]No previous purchases found.[/]";
            using (var context = new MyDbContext())
            {
                var orders = context.Orders
                    .Where(o => o.CustomerId == LoggedInCustomerId)
                    .OrderByDescending(o => o.OrderDate)
                    .Take(5)
                    .ToList();

                if (orders.Any())
                {
                    purchaseHistory = string.Join("\n", orders.Select(o =>
                        $"[bold]Order Date:[/] {o.OrderDate.ToShortDateString()} | [bold]Total:[/] {o.TotalAmount:C}"));
                }
            }

            while (true)
            {
                Console.Clear();
                Header.DisplayHeader();
                Console.WriteLine();

                var welcomeBox = new Panel($"[bold green]Welcome back![/]\n {LoggedInCustomerName}")
                    .BorderStyle(new Style(foreground: Color.Green))
                    .Expand();

                var purchasePanel = new Panel($"[bold white]Previous Purchases:[/]\n{purchaseHistory}")
                    .BorderStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .Expand();

                var inputPanel = new Panel($"[bold white]Type [yellow]exit[/] to log out.[/]\n\n[bold yellow]Your choice:[/] {inputBuffer}")
                    .BorderStyle(new Style(foreground: Color.DeepPink1))
                    .Expand();

                var grid = new Grid()
                    .AddColumn(new GridColumn().Width(60))
                    .AddRow(welcomeBox)
                    .AddRow(purchasePanel)
                    .AddRow(inputPanel);

                var paddedGrid = new Padder(grid, new Padding(50, 2, 0, 0));
                AnsiConsole.Write(paddedGrid);

                var key = Console.ReadKey(intercept: true);

                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        if (inputBuffer.ToLower() == "exit")
                        {
                            Console.Clear();
                            Header.DisplayHeader();
                            Console.WriteLine();

                            var logoutBox = new Panel("[bold red]You have been logged out.[/]")
                                .BorderStyle(new Style(foreground: Color.Red))
                                .Expand();

                            AnsiConsole.Write(new Padder(logoutBox, new Padding(0, 5, 0, 0)));
                            LoggedInCustomerId = null;
                            LoggedInCustomerName = string.Empty;
                            ConsoleUtils.ShowCursor();
                            return;
                        }
                        else
                        {
                            inputBuffer = ""; 
                        }
                        break;

                    case ConsoleKey.H:
                        Console.Clear();
                        Header.DisplayHeader();
                        Program.Space();
                        StartPageCode.StartCode();
                        return;

                    case ConsoleKey.C:
                        CategoriesPage.ShowCategoriesPage(); 
                        return;

                    case ConsoleKey.K:
                        CartPage.ShowCartPage(); 
                        return;

                    case ConsoleKey.Backspace:
                        if (inputBuffer.Length > 0)
                            inputBuffer = inputBuffer.Substring(0, inputBuffer.Length - 1);
                        break;

                    default:
                        if (!char.IsControl(key.KeyChar))
                            inputBuffer += key.KeyChar;
                        break;
                }
            }
        }
    }

    public static class ConsoleUtils
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCursorInfo(IntPtr hConsoleOutput, [In] ref CONSOLE_CURSOR_INFO lpConsoleCursorInfo);

        private const int STD_OUTPUT_HANDLE = -11;

        [StructLayout(LayoutKind.Sequential)]
        private struct CONSOLE_CURSOR_INFO
        {
            public int dwSize;
            public bool bVisible;
        }

        public static void HideCursor()
        {
            IntPtr handle = GetStdHandle(STD_OUTPUT_HANDLE);
            CONSOLE_CURSOR_INFO info = new CONSOLE_CURSOR_INFO { dwSize = 1, bVisible = false };
            SetConsoleCursorInfo(handle, ref info);
        }

        public static void ShowCursor()
        {
            IntPtr handle = GetStdHandle(STD_OUTPUT_HANDLE);
            CONSOLE_CURSOR_INFO info = new CONSOLE_CURSOR_INFO { dwSize = 1, bVisible = true };
            SetConsoleCursorInfo(handle, ref info);
        }
    }
}
