using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
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
        public MainWindow()
        {
            InitializeComponent();
            
            datum.BlackoutDates.AddDatesInPast();
            datum.BlackoutDates.Add(new CalendarDateRange(DateTime.Now.AddDays(30), DateTime.MaxValue));
        }
        public void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] workingday = new string[9];
            string[] weekend = new string[13];
            int number = 0;
            var picker = sender as DatePicker;
            var date = picker.SelectedDate.Value.DayOfWeek.ToString();
            if (date == "Saturday" || date == "Sunday")
            {                
                DateTime startDate = new DateTime(2022, 01, 01, 18, 00, 00);
                DateTime endDate = new DateTime(2022, 01, 02, 00, 00, 00);

                for (DateTime dtm = startDate; dtm <= endDate; dtm = dtm.AddMinutes(30))
                {
                    weekend[number] = dtm.ToString("HH:mm");
                    number++;
                }
                Tid.ItemsSource = weekend;


            }
            else
            {
                DateTime startDate = new DateTime(2022, 01, 01, 18, 00, 00);
                DateTime endDate = new DateTime(2022, 01, 01, 22, 00, 00);

                for (DateTime dtm = startDate; dtm <= endDate; dtm = dtm.AddMinutes(30))
                {
                    workingday[number] = dtm.ToString("HH:mm");
                    number++;
                }
                Tid.ItemsSource = workingday;
            }
        }
    }
}
