using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyVeSo
{
    class Database
    {
       public static List<Ticket> tickets = new List<Ticket>();
       public static void UpdateDatabase()
        {
            List<string> lines = new List<string>();
            foreach (Ticket ticket in tickets)
            {
                StringBuilder line = new StringBuilder();
                Dictionary<string, string> processDictionary = ticket.getData();
                foreach (string key in processDictionary.Keys)
                {
                    if (key == "numbers")
                    {
                        line.Append(processDictionary[key]);
                    } 
                    else
                    {
                        line.Append("*" + processDictionary[key]);
                    }
                }
                lines.Add(line.ToString());
            }
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"tickets.txt"))
            {
                foreach (string line in lines)
                {
                    file.WriteLine(line);
                }
            }
        }
        public static void UpdateDataset()
        {
            string[] lines = System.IO.File.ReadAllLines(@"tickets.txt");
            foreach (string line in lines)
            {
                string[] processString = line.Split('*');
                Ticket tempTicket = new Ticket(processString[0],float.Parse(processString[1]),int.Parse(processString[2]),processString[3],DateTime.FromFileTime(long.Parse(processString[4])),processString[5]);
                tickets.Add(tempTicket);
            }
        }
    }
}

