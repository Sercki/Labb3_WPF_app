using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Labb3_WPF_app
{
    interface ReservationKravförG
    {
        string Date { get; set; }
        string Time { get; set; }
        string TableNumber { get; set; }
        string Name { get; set; }
    }
    interface ReservationKravförVG
    {        
        string GuestsAmount { get; set; }
    }
    internal class BookingInfo : ReservationKravförG, ReservationKravförVG
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string TableNumber { get; set; }
        public string Name { get; set; }
        public string GuestsAmount { get; set; }

        public BookingInfo(string date, string time, string tableNumber, string name, string guestsAmount)
        {
            this.Date = date;
            this.Time = time;
            this.TableNumber = tableNumber;
            this.Name = name;
            this.GuestsAmount = guestsAmount;
        }
    }
}
