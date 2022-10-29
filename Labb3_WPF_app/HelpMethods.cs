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
        public static void InstertToList(int checkmethod, List<BookingInfo> inData, BookingInfo inCustomer)
        {
            bool checkIfAvailable = true;
            if (inData.Count > 0)
            {
                foreach (var customerBookingInfo in inData)
                {
                    if (inCustomer.Date == customerBookingInfo.Date)
                    {
                        if (inCustomer.Time == customerBookingInfo.Time)
                        {
                            if (inCustomer.TableNumber == customerBookingInfo.TableNumber)
                            {
                                if (checkmethod == 1)
                                {
                                    MessageBox.Show($"Tyvärr bordet nummer {inCustomer.TableNumber} är redan bokad. Försök boka ett annat bord eller välja annan datum!!", "Fullbokad bord", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                                checkIfAvailable = false;
                                break;                                      //utan break programmet sparar ett nytt objekt av bookinInfo om det finns mer bord som är bokad på samma tid, t.ex. programmet felaktigt sparar bord nr 1 om det finns redan bokning till kl 18 för bord nr 1 och 2. "break;" skyddar mot liknande händelser.
                            }
                            else
                            {
                                checkIfAvailable = true;
                            }
                        }
                    }
                }
            }
            if (checkIfAvailable == true)
            {
                inData.Add(inCustomer);
                if (checkmethod == 1)
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
