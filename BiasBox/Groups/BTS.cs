using BiasBox.Models;
using BiasBox.Webshop.UI;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiasBox.Groups
{
    public class BTS
    {
        public static void ShowBTSPage()
        {
            Console.Clear();
            Header.DisplayHeader();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            

         
            var textBox = new Panel("[White]BTS is a globally acclaimed South Korean boy band known for their excepional talent, powerful performances, and meaningful music. They`ve broken numerous records and earned worldwide recognition for their impact on pop culture.[/]")
                .BorderStyle(new Style(foreground: Color.White))
                .Padding(1, 1) 
                
                .RoundedBorder()
                .Expand();


            List<(string MemberId, string MemberName)> btsMembers = new List<(string, string)>
        {
            ("11", "RM"),
            ("12", "Jin"),
            ("13", "Suga"),
            ("14", "J-Hope"),
            ("15", "Jimin"),
            ("16", "V"),
            ("17", "Jungkook")
        };

            
            List<(string ProductId, string ProductName)> btsProducts = new List<(string, string)>
        {
            ("11", "BTS Hoodie - 49.99"),
            ("12", "BTS Lightstick  - 49.99"),
            ("13", "BTS Photo Cards  - 9.99"),
            ("14", "BTS Album - Map of the Soul - 29.99"),
            ("15", "BTS Pin Set - 14.99"),
            ("16", "Suga Keychain - 5.99"),
            ("17", "BTS Poster  - 9.99"),
            ("18", "BTS Sticker Pack - 4.99"),
            ("19", "BTS Calendar - 15.99"),
            ("20", "BTS Bracelet - 8.99"),
            ("21", "BTS Photobook - 29.99"),
            ("22", "BTS Stickers - 4.99"),
            ("23", "BTS Phone Case - 14.99"),
            ("24", "BTS Poster Set - 39.99")
        };

            
            var memberTable = new Table()
                .AddColumn("Member ID")
                .AddColumn("Name");

            foreach (var member in btsMembers)
            {
                memberTable.AddRow(member.MemberId, member.MemberName);
            }

            memberTable.Border = TableBorder.Rounded;
            memberTable.BorderStyle = new Style(foreground: Color.MediumPurple);

            
            var productTable = new Table()
                .AddColumn("Buy ID")
                .AddColumn("Product Name")
                .AddColumn("Buy ID")
                .AddColumn("Product Name");

            int halfCount = btsProducts.Count / 2;
            for (int i = 0; i < halfCount; i++)
            {
                productTable.AddRow(
                    btsProducts[i].ProductId, btsProducts[i].ProductName,                
                    btsProducts[i + halfCount].ProductId, btsProducts[i + halfCount].ProductName 
                );
            }

            productTable.Border = TableBorder.Rounded;
            productTable.BorderStyle = new Style(foreground: Color.LightSkyBlue1);

            
            var grid = new Grid()
                .AddColumn(new GridColumn().Width(123)); 

            
            grid.AddRow(textBox);

            
            grid.AddEmptyRow();
            grid.AddEmptyRow();
            grid.AddEmptyRow();

            

            grid.AddRow(
                new Grid()
                    .AddColumn(new GridColumn().Width(30)) 
                    .AddColumn(new GridColumn().Width(140)) 
                    .AddRow(
                        new Panel(memberTable).Header("[LightSkyBlue1]BTS Members[/]").BorderStyle(new Style(foreground: Color.MediumPurple)),
                        new Panel(productTable).Header("[MediumPurple]BTS Products[/]").BorderStyle(new Style(foreground: Color.DeepSkyBlue1))
                    )
            );

           
            AnsiConsole.Write(
                new Padder(
                    grid,
                    new Padding(20, 0, 0, 0) 
                )
            );
        }
    }
    
}

