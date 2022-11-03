using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Labb3_WPF_app                         //Jag tänkte skapa ett antal av extension metods:
{
    internal static class HelpMethods
    {
        /// <summary>
        /// en metod som lägger en nyskapad bokning till en lista av bokningar OM det finns tillräcklig mycket lediga platser vid ett bord
        /// </summary>
        /// <param name="isMessageBoxNeeded">Finns det behöv att visa en messageBox till användaren? true eller false? </param>
        /// <param name="inData">Lista av alla bokningar</param>
        /// <param name="inCustomer">objekt av klassen BookingInfo</param>
        public static void InstertToList(bool isMessageBoxNeeded, List<BookingInfo> inData, BookingInfo inCustomer)
        {
            bool checkIfAvailable = true;
            int seats = 0;
            List<BookingInfo> sameDateTimeTableReservations = new();
            if (inData.Count > 0)
            {
                try
                {
                    var sameReservationDetails = inData.Where(customerBookingInfo => customerBookingInfo.Date == inCustomer.Date && customerBookingInfo.Time == inCustomer.Time && customerBookingInfo.TableNumber == inCustomer.TableNumber);
                    foreach (var reservation in sameReservationDetails)
                    {
                        sameDateTimeTableReservations.Add(reservation);
                    }
                    sameDateTimeTableReservations.ForEach(reservation => seats += int.Parse(reservation.GuestsAmount));
                    if (seats >= 5 || seats + int.Parse(inCustomer.GuestsAmount) > 5)
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
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message, "oops!", MessageBoxButton.OK,MessageBoxImage.Error);
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
        /// <summary>
        /// Hjälp metod till PickADay_SelectedDateChanged som visar olika timmar att välja i combobox TimeCombobox beröende på användarens väl i datepicker (tips: om man testar programmet efter kl 18, man kan se att antal timmar som finns att välja minskar
        ///t.ex. om man testar programmet efter kl 16, man ser alla timmar fom kl 18-22 eller 18-00 MEN om man testar programmet efter kl 21, man kan endast välja timmar >21 )  
        /// </summary>     
        public static void AvailableHours(DateTime startValue, DateTime endValue, DatePicker chooseDate, List<string> TitleOfDayList, ComboBox selectHour)
        {
            DateTime Midnight = new(2022, 01, 02, 00, 00, 00);
            for (DateTime dtm = startValue; dtm <= endValue; dtm = dtm.AddMinutes(30))
            {
                try
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
                catch (Exception Ex)
                {

                    MessageBox.Show(Ex.Message, "oops!", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            selectHour.ItemsSource = TitleOfDayList;
        }
        /// <summary>
        /// En metod som uppdaterar fil log(dagensdatum) värje gång man trycker på boka eller avboka knappar. OBS!!!Finns också en extra metod SaveToFile_Click i xaml.cs som tillåter att spara filen med bestämd namn. Jag skapade denna extra metod för att underlätta testing av programmet.
        /// </summary>   
        public static async Task UpdateListToFile(List<BookingInfo> ReservationsList)
        {
            try
            {
                string FileName = $"log {DateTime.Now.ToShortDateString()}.json";
                using FileStream createStream = File.Create(FileName);
                await JsonSerializer.SerializeAsync(createStream, ReservationsList);
                await createStream.DisposeAsync();
            }
            catch (Exception Ex)
            {

                MessageBox.Show(Ex.Message, "oops!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
