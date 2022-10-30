using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Labb3_WPF_app
{
    internal class BookingInfo
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string TableNumber { get; set; }
        public string Name { get; set; }
        public int GuestsAmount { get; set; }

        public BookingInfo(string date, string time, string tableNumber, string name, ComboBox GuestsNumber)
        {
            this.Date = date;
            this.Time = time;
            this.TableNumber = tableNumber;
            this.Name = name;
            this.GuestsAmount = int.Parse(GuestsNumber.Text.ToString());
        }
    }
}
