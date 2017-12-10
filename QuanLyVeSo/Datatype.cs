using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyVeSo
{
    class Ticket
    {
        string numbers,code;
        float price;
        int ammout;
        DateTime startDate, endDate;

        public Ticket(string numbers,float price,int ammout,string startDate, DateTime endDate,string code) {
            this.numbers = numbers;
            this.price = price;
            this.ammout = ammout;
            if (startDate != "")
            {
                this.startDate = DateTime.FromFileTime(long.Parse(startDate));
            }
            else
            {
                this.startDate = DateTime.Now.Date;
            }
            this.endDate = endDate.Date;
            this.code = code;
        }

        public Dictionary<string, string> getData() {
            Dictionary<string, string> retData = new Dictionary<string, string>();
            retData["numbers"] = numbers;
            retData["price"] = price.ToString();
            retData["ammout"] = ammout.ToString();
            retData["startDate"] = startDate.Date.ToFileTime().ToString();
            retData["endDate"] = endDate.Date.ToFileTime().ToString();
            retData["code"] = code;
            return retData;
        }

        public bool changeData(string[] modificationInfo) {
            if (modificationInfo.Count() == 6) {
                for (int i = 0; i < 6; i++) {
                    switch (i) {
                        case 0:
                            if (modificationInfo[i] != "")
                            {
                                this.numbers = modificationInfo[i];
                            }
                            break;
                        case 1:
                            if (modificationInfo[i] != "")
                            {
                                this.price = float.Parse(modificationInfo[i]);
                            }
                            break;
                        case 2:
                            if (modificationInfo[i] != "")
                            {
                                this.ammout = Convert.ToInt32(modificationInfo[i]);
                            }
                            break;
                        case 3:
                            if (modificationInfo[i] != "")
                            {
                                this.startDate = Convert.ToDateTime(modificationInfo[i]);
                            }
                            break;
                        case 4:
                            if (modificationInfo[i] != "")
                            {
                                this.endDate = Convert.ToDateTime(modificationInfo[i]);
                            }
                            break;
                        case 5:
                            if (modificationInfo[i] != "")
                            {
                                this.code = modificationInfo[i];
                            }
                            break;
                    }
                }
                return true;
            }
            return false;
        }

        public bool isMe(string input) {
            input = input.ToLower();
            try {
                float tempPrice = float.Parse(input);
                if (tempPrice == price)
                    return true;
                else
                    throw new InvalidCastException();
            } catch {
                try {
                    int tempAmmout = Convert.ToInt32(input);
                    if (tempAmmout == ammout)
                        return true;
                    else
                        throw new InvalidCastException();
                } catch {
                    try
                    {
                        DateTime tempDate = DateTime.ParseExact(input, "dd/MM/yyyy", null);
                        if (tempDate.Date == startDate.Date || tempDate.Date == endDate.Date)
                            return true;
                    }
                    catch {
                        if (numbers.Contains(input) || code.Contains(input))
                            return true;
                    }
                }
            }
            return false;
        }
    }
}
