using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Labb3_WPF_app
{
    internal static class HelpMethods
    {
        public static void InstertToList(List<BookingInfo> inData, BookingInfo customer)
        {
            bool checkIfAvailable = true;
            if (inData.Count > 0)
            {
                foreach (var customerBookingInfo in inData)
                {
                    if (customer.Date == customerBookingInfo.Date)
                    {
                        if (customer.Time == customerBookingInfo.Time)
                        {
                            if (customer.TableNumber == customerBookingInfo.TableNumber)
                            {
                                MessageBox.Show($"Tyvärr bordet nummer {customer.TableNumber} är redan bokad. Försök boka ett annat bord eller välja annan datum!!", "Fullbokad bord", MessageBoxButton.OK, MessageBoxImage.Error);
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
                inData.Add(customer);
                DisplayContent();
                MessageBox.Show($"Bokning är klart. Kundnamn är: {customer.Name}", "Bekräftelse", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
