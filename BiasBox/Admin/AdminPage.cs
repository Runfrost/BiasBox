using BiasBox.Admin; 
using Spectre.Console;
using BiasBox.Models;
using BiasBox.Webshop.UI;

namespace BiasBox.Admin
{
    public class AdminPage
    {
        public static void ShowAdminPage()
        {

           
            try
            {
                while (true)
                {
                    Console.Clear();
                    Header.DisplayHeader();
                    Program.Space2();

                    
                    var infoPanel = new Panel(GetActionMessage())
                        .BorderStyle(new Style(foreground: Color.DeepPink1))
                        .Expand();

                    var inputPanel = new Panel("[bold white]Press a key to make a selection[/]")
                        .BorderStyle(new Style(foreground: Color.LightSkyBlue1))
                        .Expand();

                    var grid = new Grid()
                        .AddColumn(new GridColumn().Width(80))
                        .AddRow(infoPanel)
                        .AddRow(inputPanel);

                    AnsiConsole.Write(new Padder(grid, new Padding(40, 0, 0, 0)));

                   
                    var key = Console.ReadKey(true).Key;

                    switch (key)
                    {
                        case ConsoleKey.D1:
                            SelectedProducts.ShowSelectedProducts();
                            return;

                        case ConsoleKey.D2:
                            ChangeProducts.ShowChangeProducts();
                            return;

                        case ConsoleKey.D3:
                            EditCustomers.ShowEditCustomers();
                            return;

                        case ConsoleKey.D4:
                            Querys.ShowStatistics();
                            return;

                        case ConsoleKey.H:
                            Console.Clear();
                            Header.DisplayHeader();
                            Program.Space();
                            StartPageCode.StartCode();
                            return;

                        default:
                            ShowErrorMessage("Invalid input. Use 1, 2, 3, 4, or H.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"An error occurred: {ex.Message}");
            }
        }

       
        private static string GetActionMessage()
        {
            return "[bold white]Choose an option below:[/]\n\n" +
                   "[bold yellow]1:[/] Manage Featured Products\n" +
                   "[bold yellow]2:[/] Manage All Products\n" +
                   "[bold yellow]3:[/] Manage Customers\n" +
                   "[bold yellow]4:[/] View Statistics\n" +
                   "[bold yellow]H:[/] Return to Homepage";
        }

       
        private static void ShowErrorMessage(string message)
        {
            AnsiConsole.MarkupLine($"[red]{message}[/]");
            System.Threading.Thread.Sleep(1500); // Vänta 1,5 sek 
        }
    }
}
