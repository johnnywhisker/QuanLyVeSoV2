using SimpleCMenu.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyVeSo
{
    class Program
    {
        static void exit()
        {
            Database.UpdateDatabase();
            Environment.Exit(69);
        }
        static void Main(string[] args)
        {
            Database.UpdateDataset();
            Console.Clear();
            ConsoleMenu menu = new ConsoleMenu();
            string headerText = "   ____  _    _         _   _     _  __     __  __      ________    _____  ____  " +
                 Environment.NewLine + "  / __ \\| |  | |  /\\   | \\ | |   | | \\ \\   / /  \\ \\    / /  ____|  / ____|/ __ \\ " +
                 Environment.NewLine + " | |  | | |  | | /  \\  |  \\| |   | |  \\ \\_/ /    \\ \\  / /| |__    | (___ | |  | |" +
                 Environment.NewLine + " | |  | | |  | |/ /\\ \\ | . ` |   | |   \\   /      \\ \\/ / |  __|    \\___ \\| |  | |" +
                 Environment.NewLine + " | |__| | |__| / ____ \\| |\\  |   | |____| |        \\  /  | |____   ____) | |__| |" +
                 Environment.NewLine + "  \\___\\_\\_____/_/    \\_\\_| \\_|   |______|_|         \\/   |______| |_____/ \\____/ ";           
            menu.Header = headerText;
            menu.SubTitle = "\n-----------------------------------MENU---------------------------------------";
            menu.addMenuItem(1, "Them ve so", Interface.addTicket);
            menu.addMenuItem(2, "Xem tat ca ve so", Interface.showAllTicket);
            menu.addMenuItem(3, "Tim kiem ve so", Interface.findTicket);
            menu.addMenuItem(4, "Sua thong tin ve so", Interface.modifyTicket);
            menu.addMenuItem(5, "Xoa ve so", Interface.deleteTicket);
            menu.addMenuItem(0, "Thoat", Program.exit);
            menu.showMenu();
            Console.ReadKey();

        }
    }
}
