using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using Figgle;

namespace BiasBox.Webshop.UI
{
    internal class Header
    {
        public static void DisplayHeader()
        {

            // Ställ in UTF-8 för att stödja blommor
            Console.OutputEncoding = System.Text.Encoding.UTF8;

           
            Console.Clear();
            int consoleWidth = Console.WindowWidth;

            
            string flowerLine = new string('✿', consoleWidth);

            
            var headerTitle = FiggleFonts.Epic.Render("BIAS  BOX");
            string[] headerLines = headerTitle.Split('\n'); 

            
            string description = "[italic white]The ultimate store for all your favorite KPOP merchandise![/]";

            
            AnsiConsole.MarkupLine($"[rgb(173,216,230)]{flowerLine}[/]"); 
            Console.WriteLine(); 

            
            foreach (var line in headerLines)
            {
                PrintCentered($"[bold rgb(255,20,147)]{line}[/]"); 
            }
            
            PrintCentered(description); 
            Console.WriteLine(); 

            AnsiConsole.MarkupLine($"[rgb(173,216,230)]{flowerLine}[/]"); 

            
            DisplayBoxes();
        }

        
        private static void PrintCentered(string text)
        {
            int consoleWidth = Console.WindowWidth;

            
            string plainText = Spectre.Console.Markup.Remove(text);

            
            int padding = (consoleWidth - plainText.Length) / 2;
            string centeredLine = new string(' ', Math.Max(0, padding)) + text;

            
            AnsiConsole.MarkupLine(centeredLine);
        }

        
        private static void DisplayBoxes()
        {
            
            string bunny = @"(\_/)
(o.o)
(" + '"' + ")" + "(" + '"' + @")";

            
            var grid = new Grid()
    .AddColumn(new GridColumn().Width(15))  // Kolumn för vänstra kaninen
    .AddColumn(new GridColumn().Width(22))  // Första boxen
    .AddColumn(new GridColumn().Width(22))  // Andra boxen
    .AddColumn(new GridColumn().Width(22))  // Tredje boxen
    .AddColumn(new GridColumn().Width(22))  // Fjärde boxen
    .AddColumn(new GridColumn().Width(22))  // Femte boxen (Admin)
    .AddColumn(new GridColumn().Width(10)); // Kolumn för högra kaninen

            var pinkColor = new Color(255, 105, 180);

            // Lägg till en rad med boxar och kaniner
            grid.AddRow(
                new Panel($"[bold]{bunny}[/]").NoBorder(), // Vänstra kaninen 
                new Panel("[white bold]H - Homepage[/]").Border(BoxBorder.Square).BorderStyle(new Style(pinkColor)),
                new Panel("[white bold]C - Categories[/]").Border(BoxBorder.Square).BorderStyle(new Style(pinkColor)),
                new Panel("[white bold]L - LogIn Page[/]").Border(BoxBorder.Square).BorderStyle(new Style(pinkColor)),
                new Panel("[white bold]A - Admin Page[/]").Border(BoxBorder.Square).BorderStyle(new Style(pinkColor)),
                new Panel("[white bold]K - Cart[/]").Border(BoxBorder.Square).BorderStyle(new Style(pinkColor)),
                new Panel($"[bold]{bunny}[/]").NoBorder() // Högra kaninen
            );

            Console.WriteLine();
            Console.WriteLine();

            // Centrera griden
            int consoleWidth = Console.WindowWidth;
            int gridWidth = 15 + 22 * 5 + 10; 
            int paddingLeft = (consoleWidth - gridWidth) / 2;

            AnsiConsole.Write(new Padder(grid, new Padding(paddingLeft, 0, 0, 0)));
        }

        public static void BTSHeader()
        {



        }
    }


    
    
    
}

