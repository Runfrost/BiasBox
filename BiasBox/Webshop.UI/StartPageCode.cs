using BiasBox.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiasBox.Webshop.UI
{
    public class StartPageCode
    {




        public static void StartCode()
        {
            
            List<(string ProductId, string ProductName)> featuredProducts;

            using (var context = new MyDbContext())
            {
                
                var products = context.Products
                    .Where(p => p.IsFeatured)
                    .Take(5)
                    .ToList(); 

               
                featuredProducts = products
                    .Select(p => (p.ID.ToString(), $"{p.Name} - {p.Price:C}"))
                    .ToList();
            }
            
            var groups = new List<(string GroupId, string GroupName)>
        {
            ("1", "BTS"),
            ("2", "ATEEZ"),
            ("3", "EXO"),
            ("4", "Seventeen"),
            ("5", "Stray Kids"),
            ("6", "GOT7"),
            ("7", "ENHYPEN"),
            ("8", "TWICE"),
            ("9", "STAYC"),
            ("10", "MYTRO")
        };

           
            var productTable = new Table()
                .AddColumn("Purchase ID")
                .AddColumn("Product");

            foreach (var product in featuredProducts)
            {
                productTable.AddRow(product.Item1, product.Item2);
            }

            
            var groupTable = new Table()
                .AddColumn("Group ID")
                .AddColumn("Group")
                .AddColumn("Group ID")
                .AddColumn("Group");

            for (int i = 0; i < groups.Count / 2; i++)
            {
                groupTable.AddRow(
                    groups[i].GroupId, groups[i].GroupName,
                    groups[i + 5].GroupId, groups[i + 5].GroupName
                );
            }

           
            productTable.Border = TableBorder.DoubleEdge;
            productTable.BorderStyle = new Style(foreground: Color.LightSkyBlue1, background: Color.Black);

            groupTable.Border = TableBorder.DoubleEdge;
            groupTable.BorderStyle = new Style(foreground: Color.LightSkyBlue1, background: Color.Black);

           
            var grid = new Grid()
                .AddColumn(new GridColumn().Width(50)) 
                .AddColumn(new GridColumn().Width(50)); 

            grid.AddRow(
                new Panel(productTable)
                    .Header("[bold rgb(255,20,147)]✨ FEATURED PRODUCTS ✨[/]")
                    .BorderStyle(new Style(foreground: Color.Pink1)),
                new Panel(groupTable)
                    .Header("[bold rgb(255,20,147)]🎶 GROUPS 🎶[/]")
                    .BorderStyle(new Style(foreground: Color.Pink1))
            );

            
            AnsiConsole.Write(
                new Padder(
                    grid,
                    new Padding(30, 0, 0, 0) 
                )
            );
        }





    }
}

    


