using QuanLyVeSo.ConsoleTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyVeSo
{
    class Interface
    {
        public static void showAllTicket()
        {
            Console.Clear();
            string title = "Danh sach ve so";
            var table = new ConsoleTable("Day so", "Gia tien", "So luong", "Ngay ban", "Ngay xo", "Ma so");
            Console.WriteLine(String.Format("{0," + ((80 / 2) + (title.Length / 2)) + "}", title));
            foreach (Ticket ticket in Database.tickets)
            {
                Dictionary<string, string> processDictionary = ticket.getData();
                table.AddRow(
                processDictionary["numbers"],
                    processDictionary["price"],
                    processDictionary["ammout"],
                    DateTime.FromFileTime(long.Parse(processDictionary["startDate"])).ToShortDateString(),
                    DateTime.FromFileTime(long.Parse(processDictionary["endDate"])).ToShortDateString(),
                    processDictionary["code"]);
            }
            table.Write();
            Console.WriteLine();
            Console.Write("Bam phim bat ki de ve man hinh chinh.");
            Console.ReadKey();
        }

        public static void addTicket()
        {
            Console.Clear();
            Edit:
            try
            {
                Console.Write("Nhap vao day so :  ");
                string numbers = Console.ReadLine();
                Console.Write("Nhap vao gia tien :  ");
                float price = float.Parse(Console.ReadLine());
                Console.Write("Nhap vao so luong ve so: ");
                int ammout = int.Parse(Console.ReadLine());
                Console.Write("Nhap vao ngay xo (dd/MM/YYYY): ");
                DateTime endDate = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", null);
                Console.Write("Nhap vao ma so cua day so: ");
                string code = Console.ReadLine();
                if (numbers == "" || code == "")
                {
                    throw new InvalidCastException();
                }
                Database.tickets.Add(new Ticket(numbers, price, ammout, "", endDate, code));

            }

            catch
            {
                Console.Write("Gia tri khong hop le.Bam phim bat ki de nhap lai hoac bam 0 de huy");
                ConsoleKey key = Console.ReadKey().Key;
                if (key == ConsoleKey.D0 || key == ConsoleKey.NumPad0)
                {
                    return;
                }
                Console.Clear();
                goto Edit;
            }
        }

        public static void findTicket()
        {
            Console.Clear();
            Console.Write("Nhap vao thong tin ve so can tim: ");
            string input = Console.ReadLine();
            bool isFound = false;
            var table = new ConsoleTable("Day so", "Gia tien", "So luong", "Ngay ban", "Ngay xo", "Ma so");
            foreach (Ticket ticket in Database.tickets)
            {
                if (ticket.isMe(input))
                {
                    isFound = true;
                    Dictionary<string, string> processDictionary = ticket.getData();
                    table.AddRow(
                    processDictionary["numbers"],
                        processDictionary["price"],
                        processDictionary["ammout"],
                        DateTime.FromFileTime(long.Parse(processDictionary["startDate"])).ToShortDateString(),
                        DateTime.FromFileTime(long.Parse(processDictionary["endDate"])).ToShortDateString(),
                        processDictionary["code"]);
                }
            }
            if (isFound)
            {
                table.Write();
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Khong tim thay ket qua nao cho tu khoa \"{0}\"", input);
            }
            Console.Write("Bam phim bat ki de ve man hinh chinh.");
            Console.ReadKey();
        }

        public static void modifyTicket()
        {
            Console.Clear();
            Dictionary<string, string> processDictionary = null;
            int index = -1;
            Console.Write("Nhap vao ma so cua ve so can sua: ");
            string input = Console.ReadLine();
            foreach (Ticket book in Database.tickets)
            {
                Dictionary<string, string> tempDic = book.getData();
                if (tempDic["code"] == input)
                {
                    index = Database.tickets.IndexOf(book);
                    processDictionary = tempDic;
                    break;
                }
            }
            if (processDictionary != null)
            {
                ConsoleTable info = new ConsoleTable(new ConsoleTableOptions
                {
                    EnableCount = false,
                    Columns = new[] { "Day so", "Gia tien", "So luong", "Ngay ban", "Ngay xo", "Ma so" }
                });
                info.AddRow(
                    processDictionary["numbers"],
                        processDictionary["price"],
                        processDictionary["ammout"],
                        DateTime.FromFileTime(long.Parse(processDictionary["startDate"])).ToShortDateString(),
                        DateTime.FromFileTime(long.Parse(processDictionary["endDate"])).ToShortDateString(),
                        processDictionary["code"]);
                Edit:
                Console.Clear();
                info.Write();
                try
                {
                    Console.Write("Nhap vao day so [{0}]:  ", processDictionary["numbers"]);
                    string numbers = Console.ReadLine();
                    Console.Write("Nhap vao gia tien [{0}]:  ", processDictionary["price"]);
                    string price = Console.ReadLine();
                    if (price != "")
                    {
                        float tempPrice = float.Parse(price);
                        price = tempPrice.ToString();
                    }
                    Console.Write("Nhap vao so luong [{0}]:  ", processDictionary["ammout"]);
                    string amount = Console.ReadLine();
                    if (amount != "")
                    {
                        float tempAmount = float.Parse(amount);
                        amount = tempAmount.ToString();
                    }
                    Console.Write("Nhap vao ngay xo (dd/MM/yyyy)[{0}]:  ", DateTime.FromFileTime(long.Parse(processDictionary["endDate"])).ToShortDateString());
                    string endDate = Console.ReadLine();
                    if (endDate != "")
                    {
                        endDate = DateTime.ParseExact(endDate, "dd/MM/yyyy", null).ToShortDateString();
                    }
                    Console.Write("Nhap vao ma so ve so [{0}]: ", processDictionary["code"]);
                    string code = Console.ReadLine();
                    string[] modificationInfo = { numbers, price, amount, "", endDate, code };
                    Database.tickets[index].changeData(modificationInfo);
                }
                catch
                {
                    Console.Write("Gia tri khong hop le.Bam phim bat ki de nhap lai.");
                    Console.ReadKey();
                    Console.Clear();
                    goto Edit;
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Khong tim thay ve so cua co ma so: {0}. Bam phim bat ki de tro ve menu.", input);
                Console.ReadKey();
            }
        }

        public static void deleteTicket() {
            Console.Clear();
            Dictionary<string, string> processDictionary = null;
            int index = -1;
            Console.Write("Nhap vao ma so ve so can xoa: ");
            string input = Console.ReadLine();
            foreach (Ticket ticket in Database.tickets)
            {
                Dictionary<string, string> tempDic = ticket.getData();
                if (tempDic["code"] == input)
                {
                    index = Database.tickets.IndexOf(ticket);
                    processDictionary = tempDic;
                    break;
                }
            }
            if (processDictionary != null)
            {
                ConsoleTable info = new ConsoleTable(new ConsoleTableOptions
                {
                    EnableCount = false,
                    Columns = new[] { "Day so", "Gia tien", "So luong", "Ngay ban", "Ngay xo", "Ma so" }
                });
                info.AddRow(
                    processDictionary["numbers"],
                        processDictionary["price"],
                        processDictionary["ammout"],
                        DateTime.FromFileTime(long.Parse(processDictionary["startDate"])).ToShortDateString(),
                        DateTime.FromFileTime(long.Parse(processDictionary["endDate"])).ToShortDateString(),
                        processDictionary["code"]);
                Confirmation:
                Console.Clear();
                info.Write();
                Console.Write("Ban co chac chan muon xoa ve so (y/n): ");
                string answer = Console.ReadLine();
                if (answer != "y" && answer != "n")
                {
                    Console.Clear();
                    Console.WriteLine("Khong hop le. Bam phim bat ki de nhap lai.");
                    Console.ReadKey();
                    Console.Clear();
                    goto Confirmation;
                }
                else
                {
                    if (answer == "y")
                    {
                        Database.tickets.RemoveAt(index);
                        Console.Clear();
                        Console.WriteLine("Xoa ve so thanh cong. Bam phim bat ki de tro ve menu.");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Huy xoa ve so thanh cong. Bam phim bat ki de tro ve menu.");
                        Console.ReadKey();
                    }
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Khong tim thay ve so cua co ma so: {0}. Bam phim bat ki de tro ve menu.", input);
                Console.ReadKey();
            }
        }
    }
}
