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
            DisplayContent();

            Datum.BlackoutDates.AddDatesInPast();
            Datum.BlackoutDates.Add(new CalendarDateRange(DateTime.Now.AddDays(30), DateTime.MaxValue));
        }
        public void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            List<string> workingday = new List<string>();
            List<string> weekend = new List<string>();
            var picker = sender as DatePicker;
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

                Tid.ItemsSource = weekend;
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
                Tid.ItemsSource = workingday;
            }
        }

        private void Boka_Click(object sender, RoutedEventArgs e)
        {
            var customer = new BookingInfo(Datum.Text, Tid.Text, Bordsnummer.Text, Namn.Text);
            history.Add(customer);            
            DisplayContent();            
            MessageBox.Show($"Booking confirmed! {customer.Date} {customer.Time}  {customer.TableNumber} {customer.Name} ");
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
    }
}
