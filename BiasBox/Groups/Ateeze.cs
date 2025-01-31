using BiasBox.Webshop.UI;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiasBox.Groups
{
    internal class Ateeze
    {
        public static void ShowAteezPage()
        {
            Console.Clear();
            Header.DisplayHeader();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            
            var textBox = new Panel("[White]Ateez is a rising global sensation in the K-pop world, captivating fans with their charismatic stage presence, powerful choreography, and diverse music styles. Known as 'Global Performance Idols,' Ateez has consistently delivered chart-topping hits and unforgettable live performances, solidifying their place as one of the most influential groups in modern K-pop.[/]")
                .BorderStyle(new Style(foreground: Color.White))
                .Padding(1, 1)
                .RoundedBorder()
                .Expand();

            
            List<(string MemberId, string MemberName)> ateezMembers = new List<(string, string)>
    {
        ("18", "Hongjoong"),
        ("19", "Seonghwa"),
        ("20", "Yunho"),
        ("21", "Yeosang"),
        ("22", "San"),
        ("23", "Mingi"),
        ("24", "Wooyoung"),
        ("25", "Jongho")
    };

            
            List<(string ProductId, string ProductName)> ateezProducts = new List<(string, string)>
    {
        ("31", "Ateez Hoodie - 49.99"),
        ("32", "Ateez Lightstick  - 49.99"),
        ("33", "Ateez Photo Cards  - 9.99"),
        ("34", "Ateez Album - Treasure - 29.99"),
        ("35", "Ateez Pin Set - 14.99"),
        ("36", "San Keychain - 5.99"),
        ("37", "Ateez Poster  - 9.99"),
        ("38", "Ateez Sticker Pack - 4.99"),
        ("39", "Ateez Calendar - 15.99"),
        ("40", "Ateez Bracelet - 8.99"),
        ("41", "Ateez Photobook - 29.99"),
        ("42", "Ateez Stickers - 4.99"),
        ("43", "Ateez Phone Case - 14.99"),
        ("44", "Ateez Poster Set - 39.99"),
        ("45", "Ateez Tote Bag - 19.99"),
        ("46", "Ateez Limited Edition - 49.99")
    };

            
            var memberTable = new Table()
                .AddColumn("Member ID")
                .AddColumn("Name");

            foreach (var member in ateezMembers)
            {
                memberTable.AddRow(member.MemberId, member.MemberName);
            }

            memberTable.Border = TableBorder.Rounded;
            memberTable.BorderStyle = new Style(foreground: Color.DarkRed);

            
            var productTable = new Table()
                .AddColumn("Buy ID")
                .AddColumn("Product Name")
                .AddColumn("Buy ID")
                .AddColumn("Product Name");

            int halfCount = ateezProducts.Count / 2;
            for (int i = 0; i < halfCount; i++)
            {
                productTable.AddRow(
                    ateezProducts[i].ProductId, ateezProducts[i].ProductName,               
                    ateezProducts[i + halfCount].ProductId, ateezProducts[i + halfCount].ProductName 
                );
            }

            productTable.Border = TableBorder.Rounded;
            productTable.BorderStyle = new Style(foreground: Color.DarkRed);

           
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
                        new Panel(memberTable).Header("[Green]Ateez Members[/]").BorderStyle(new Style(foreground: Color.DarkRed_1)),
                        new Panel(productTable).Header("[Green]Ateez Products[/]").BorderStyle(new Style(foreground: Color.DarkRed_1))
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
