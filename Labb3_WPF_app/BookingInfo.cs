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

        public BookingInfo(string date, string time, string tableNumber, string name)
        {
            this.Date = date;
            this.Time = time;
            this.TableNumber = tableNumber;
            this.Name = name;
            GuestsAmount = 5;

        }
        public bool AvailableSeats(ComboBox AmountOfGuests)
        {
            bool check = false;
            if (AmountOfGuests.SelectedValue != null)
            {
                int difference = int.Parse(AmountOfGuests.Text.ToString());
                GuestsAmount = GuestsAmount - difference;
                if (GuestsAmount > 0)
                {
                     check = true;
                }
                else
                {
                    check = false;
                }                
            }
            return check;
        }
    }
    //internal class Guest
    //{
    //    public string Name { get; set; }
    //    public int GuestsAmount { get; set; }

    //    public Guest(string name, int guestsAmount)
    //    {
    //        this.Name = name;
    //        this.GuestsAmount = guestsAmount;
    //    }

    //}
}
