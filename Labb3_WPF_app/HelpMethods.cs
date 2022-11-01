using System;
using System.Collections.Generic;
using System.IO;
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
            bool check = true;
            int seats = 0;
            List<BookingInfo> sameDateTimeTableReservations = new List<BookingInfo>();
            if (inData.Count > 0)
            {                
                var sameReservationDetails = inData.Where(customerBookingInfo => customerBookingInfo.Date == inCustomer.Date && customerBookingInfo.Time == inCustomer.Time && customerBookingInfo.TableNumber == inCustomer.TableNumber);    
                foreach (var reservation in sameReservationDetails)
                {
                    sameDateTimeTableReservations.Add(reservation);                                                                                                                 
                }
                sameDateTimeTableReservations.ForEach(reservation => seats += reservation.GuestsAmount);

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
        public static void availableHours(DateTime startValue, DateTime endValue, DatePicker chooseDate, List<string> TitleOfDayList, ComboBox selectHour)
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
                            TitleOfDayList.Add(dtm.ToString("HH:mm"));
                        }
                    }
                    else if (dtm.Hour > DateTime.Now.Hour || dtm.Hour == Midnight.Hour) //skapade variabel midnight  och en till vilkor (till höger från || )  i else if sats eftersom option 00:00 försvinner plötsligt om man testar programmet i helgen  
                    {
                        TitleOfDayList.Add(dtm.ToString("HH:mm"));
                    }
                }
                else
                {
                    TitleOfDayList.Add(dtm.ToString("HH:mm"));
                }
            }
            selectHour.ItemsSource = TitleOfDayList;
        }
        public static void updateListToFile(List<BookingInfo> ReservationsList) // här metoden skapar fil log (dagens datum). Filen uppdaterar sig automatiskt värje gång man trycker på boka eller avboka knappar. Om man avbokar alla reservationer från lista,
        {                                                                       // filen med dagens datum försvinner. För att blanda inte funktionaliteten av updateListtoFile och Backup Save to file, här filen heter log och filen i backup heter backup. 
            string secondPartOfFileName = DateTime.Now.ToShortDateString();     //Man kan ladda båda två filer med loadfromfile function i backup.
            File.Delete($"log {secondPartOfFileName}.txt");
            foreach (var item in ReservationsList)
            {
                string DataInReservationsList = $"{item.Date},{item.Time},{item.TableNumber},{item.Name},{item.GuestsAmount}\r\n";
                File.AppendAllText($"log {secondPartOfFileName}.txt", DataInReservationsList);
            }
        }
    }
}
