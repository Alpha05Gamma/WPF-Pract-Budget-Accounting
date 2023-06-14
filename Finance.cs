using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FinanceManager
{
    internal class Finance
    {
        public Finance(string name, string type, double money, DateTime date)
        {
            this.name = name;
            this.type = type;
            this.money = money;
            this.date = date;
        }
        public string name { get; set; }
        public string type { get; set; }
        public double money { get; set; }
        public DateTime date { get; set; }

        public static ObservableCollection<Finance> finances;    
    }
}
