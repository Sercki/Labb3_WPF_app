using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Labb3_WPF_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<BookingInfo> history = new List<BookingInfo>();

        public MainWindow()
        {
            InitializeComponent();
            StartExamplesRandom();
            BokaEnabled();
            DisplayContent();

            PickADay.BlackoutDates.AddDatesInPast();
            PickADay.BlackoutDates.Add(new CalendarDateRange(DateTime.Now.AddDays(30), DateTime.MaxValue));
        }
        public void PickADay_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

            List<string> workingday = new List<string>();
            List<string> weekend = new List<string>();
            var picker = sender as DatePicker;
            if (PickADay.SelectedDate != null)
            {
                var date = picker.SelectedDate.Value.DayOfWeek.ToString();
                if (date == "Saturday" || date == "Sunday")
                {
                    DateTime startDate = new DateTime(2022, 01, 01, 18, 00, 00);
                    DateTime endDate = new DateTime(2022, 01, 02, 00, 00, 00);
                    for (DateTime dtm = startDate; dtm <= endDate; dtm = dtm.AddMinutes(30))
                    {
                        if (picker.SelectedDate == DateTime.Today)
                        {
                            if (dtm.Hour == DateTime.Now.Hour)
                            {
                                if (dtm.Minute > DateTime.Now.Minute)
                                {
                                    weekend.Add(dtm.ToString("HH:mm"));
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else if (dtm.Hour > DateTime.Now.Hour)
                            {
                                weekend.Add(dtm.ToString("HH:mm"));
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            weekend.Add(dtm.ToString("HH:mm"));
                        }
                    }
                    TimeComboBox.ItemsSource = weekend;
                }
                else
                {
                    DateTime startDate = new DateTime(2022, 01, 01, 18, 00, 00);
                    DateTime endDate = new DateTime(2022, 01, 01, 22, 00, 00);
                    for (DateTime dtm = startDate; dtm <= endDate; dtm = dtm.AddMinutes(30))
                    {
                        if (picker.SelectedDate == DateTime.Today)
                        {
                            if (dtm.Hour == DateTime.Now.Hour)
                            {
                                if (dtm.Minute > DateTime.Now.Minute)
                                {
                                    workingday.Add(dtm.ToString("HH:mm"));
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else if (dtm.Hour > DateTime.Now.Hour)
                            {
                                workingday.Add(dtm.ToString("HH:mm"));
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            workingday.Add(dtm.ToString("HH:mm"));
                        }
                    }
                    TimeComboBox.ItemsSource = workingday;
                }
            }
            else
            {
                BokaEnabled();
            }
        }

        private void Boka_Click(object sender, RoutedEventArgs e)
        {
            var customer = new BookingInfo(PickADay.Text, TimeComboBox.Text, TableComboBox.Text, NameTextBox.Text);
            bool checkIfAvailable = true;
            if (history.Count > 0)
            {
                foreach (var customerBookingInfo in history)
                {
                    if (customer.Date == customerBookingInfo.Date)
                    {
                        if (customer.Time == customerBookingInfo.Time)
                        {
                            if (customer.TableNumber == customerBookingInfo.TableNumber)
                            {
                                MessageBox.Show($"Tyvärr bordet nummer {TableComboBox.Text} är redan bokad. Försök boka ett annat bord eller välja annan datum!!", "Fullbokad bord", MessageBoxButton.OK, MessageBoxImage.Error);
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
                history.Add(customer);
                DisplayContent();
                MessageBox.Show($"Bokning är klart. Kundnamn är: {customer.Name}","Bekräftelse", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            Clear();

        }
        private void DisplayContent()
        {
            ConfirmedList.ItemsSource = null;
            ConfirmedList.ItemsSource = history;
        }

        private void Avboka_Click(object sender, RoutedEventArgs e)
        {
            if (ConfirmedList.SelectedItem == null)
                return;
            history.Remove((BookingInfo)ConfirmedList.SelectedItem);
            DisplayContent();
        }
        private void Clear()
        {
            PickADay.SelectedDate = null;
            TimeComboBox.ItemsSource = null;
            TableComboBox.SelectedValue = null;
            NameTextBox.Text = "";
        }
        private void BokaEnabled()
        {
            if (PickADay.SelectedDate == null || TimeComboBox.SelectedItem == null || TableComboBox.SelectedItem == null || NameTextBox.Text.Length == 0)
            {
                Book.IsEnabled = false;
            }
            else
            {
                Book.IsEnabled = true;
            }
        }

        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            BokaEnabled();
        }

        private void TableComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BokaEnabled();
        }

        private void TimeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BokaEnabled();
        }   
        private void StartExamplesRandom()  //metod för att lägga till ett antal bokningar redo att visas
        {
            string[] exampleNames = { "Anna", "Sofia", "Anders", "Magnus", "Ted" };
            string[] hour = { "18:00", "18:30", "19:00", "19:30", "20:00", "20:30", "21:00", "21:30", "22:00" };
            while (history.Count < 20)
            {
                Random rnd = new Random();
                int randomDay = rnd.Next(0, 31);
                DateTime randomDate = DateTime.Now.AddDays(randomDay);
                int nameIndex = rnd.Next(exampleNames.Length);
                int hourIndex = rnd.Next(hour.Length);
                int table = rnd.Next(1, 6);
                var customer = new BookingInfo(randomDate.ToShortDateString(), hour[hourIndex], table.ToString(), exampleNames[nameIndex]);
                bool checkIfAvailable = true;
                if (history.Count > 0)
                {
                    foreach (var customerBookingInfo in history)
                    {
                        if (customer.Date == customerBookingInfo.Date)
                        {
                            if (customer.Time == customerBookingInfo.Time)
                            {
                                if (customer.TableNumber == customerBookingInfo.TableNumber)
                                {
                                    checkIfAvailable = false;
                                    break;
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
                    history.Add(customer);
                    DisplayContent();
                }
            }
        }

    }
}
