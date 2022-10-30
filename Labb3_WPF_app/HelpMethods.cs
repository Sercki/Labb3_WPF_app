using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Labb3_WPF_app
{
    internal static class HelpMethods
    {
        public static void InstertToList(bool isMessageBoxNeeded, List<BookingInfo> inData, BookingInfo inCustomer)
        {            
            bool checkIfAvailable = true;
            int seats = 0;
            List<BookingInfo> sameDateTimeTableReservations = new List<BookingInfo>();
            if (inData.Count > 0)
            {
                foreach (var customerBookingInfo in inData)
                {
                    if (customerBookingInfo.Date == inCustomer.Date)
                    {
                        if (customerBookingInfo.Time == inCustomer.Time)
                        {
                            if (customerBookingInfo.TableNumber == inCustomer.TableNumber)
                            {//tutaj dodano           
                                sameDateTimeTableReservations.Add(customerBookingInfo);
                            }
                        }
                    }
                }                
                foreach (var item in sameDateTimeTableReservations)
                {
                    seats += item.GuestsAmount;
                }       
                if (seats >= 5 || seats + inCustomer.GuestsAmount > 5)
                {
                    if (isMessageBoxNeeded == true)
                    {
                        MessageBox.Show($"Tyvärr bordet nummer {inCustomer.TableNumber} är redan fullbokad. Försök boka ett annat bord eller välja annan datum alternativt dela kundens reservation på flera olika bord!", "Fullbokad bord", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    checkIfAvailable = false;                    
                }
                else
                {
                    checkIfAvailable = true;
                }
            }
            if (checkIfAvailable == true)
            {
                inData.Add(inCustomer);                
                if (isMessageBoxNeeded == true)
                {
                    MessageBox.Show($"Bokning är klart. Kundnamn är: {inCustomer.Name}", "Bekräftelse", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        public static void availableHours(DateTime startValue, DateTime endValue, DatePicker chooseDate, List<string> nameOfDay, ComboBox selectHour)
        {
            DateTime Midnight = new DateTime(2022, 01, 02, 00, 00, 00);
            for (DateTime dtm = startValue; dtm <= endValue; dtm = dtm.AddMinutes(30))
            {
                if (chooseDate.SelectedDate == DateTime.Today)
                {
                    if (dtm.Hour == DateTime.Now.Hour)
                    {
                        if (dtm.Minute > DateTime.Now.Minute)
                        {
                            nameOfDay.Add(dtm.ToString("HH:mm"));
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (dtm.Hour > DateTime.Now.Hour || dtm.Hour == Midnight.Hour) //skapade variabel midnight  och en till vilkor (till höger från || )  i else if sats eftersom option 00:00 försvinner plötsligt om man testar programmet i helgen  
                    {
                        nameOfDay.Add(dtm.ToString("HH:mm"));
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    nameOfDay.Add(dtm.ToString("HH:mm"));
                }
            }
            selectHour.ItemsSource = nameOfDay;
        }
    }
}
