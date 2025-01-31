using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiasBox.Categories;
using Spectre.Console;

namespace BiasBox.Webshop.UI
{
    public class CategoriesPage
    {
        public static void ShowCategoriesPage()
        {
            
            Console.Clear();
            Header.DisplayHeader();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            
            var categories = new List<string>
            {
                "🎀 Accessories - A",
                "📀 Albums - Al",
                "👚 Cloths - C",
                "🎤 Groups - G",
                "🌟 Lightsticks - L",
                "🎧 Merchandises - M",
                "🌌 Posters - P",
                "📷 Photo Cards - Pc",
                "📖 Stationerys - S"
            };

            
            if (categories == null || categories.Count == 0)
            {
                AnsiConsole.Markup("[red bold]No categories available to display.[/]");
                return;
            }

            
            var grid = new Grid()
                .AddColumn(new GridColumn().Width(50)) 
                .AddColumn(new GridColumn().Width(50))
                .AddColumn(new GridColumn().Width(50));

           

            for (int i = 0; i < categories.Count; i += 3)
            {
                grid.AddRow(
                    new Panel(categories[i])
                        .BorderStyle(new Style(foreground: Color.LightSkyBlue1))
                        .Padding(2, 1)
                        .Expand(),
                    (i + 1 < categories.Count)
                        ? new Panel(categories[i + 1])
                            .BorderStyle(new Style(foreground: Color.LightSkyBlue1))
                            .Padding(2, 1)
                            .Expand()
                        : new Panel(" ") 
                            .BorderStyle(new Style(foreground: Color.Green))
                            .Padding(2, 1)
                            .Expand(),
                    (i + 2 < categories.Count)
                        ? new Panel(categories[i + 2])
                            .BorderStyle(new Style(foreground: Color.LightSkyBlue1))
                            .Padding(2, 1)
                            .Expand()
                        : new Panel(" ") 
                            .BorderStyle(new Style(foreground: Color.Yellow))
                            .Padding(2, 1)
                            .Expand()
                );
            }

            
            var paddedGrid = new Padder(grid, new Padding(3, 1, 3, 1));

            

            AnsiConsole.Write(
                new Panel(paddedGrid)
                    .BorderStyle(new Style(foreground: Color.Purple))
                    .Expand()
            );


           
            ConsoleKeyInfo keyInfo = Console.ReadKey(true); 
            string input = keyInfo.KeyChar.ToString().ToUpper(); 

            

            switch (input)
            {
                case "A": 
                    Accessories.ShowAccessories();
                    break;

                case "AL": 
                    Albums.ShowAlbums();
                    break;

                case "C": 
                    Cloths.ShowCloths();
                    break;

                case "L": 
                    Lightsticks.ShowLightsticks();
                    break;

                case "P": 
                    Posters.ShowPosters();
                    break;

                case "PC": 
                    Photo_Cards.ShowPhotoCards();
                    break;

                case "M": 
                    Merchandises.ShowMerchandises();
                    break;

                case "S": 
                    Stationerys.ShowStationerys();
                    break;

                case "H": 
                    Console.Clear();
                    StartPageCode.StartCode();
                    break;

                default:
                    Console.WriteLine("Ogiltigt val. Försök igen.");
                    Console.ReadKey(true); 
                    ShowCategoriesPage(); 
                    break;
            }
        }
    }
}
