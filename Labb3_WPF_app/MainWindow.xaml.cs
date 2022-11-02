﻿using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Text.Json;
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
        List<BookingInfo> history = new();

        public MainWindow()
        {
            InitializeComponent();
            StartExamplesRandom();  //detta metod skapar ett antal bokningar tillsammans med start av programmet. 
            BokaEnabled();
            DisplayContent();
            PickADay.BlackoutDates.AddDatesInPast();
            PickADay.BlackoutDates.Add(new CalendarDateRange(DateTime.Now.AddDays(30), DateTime.MaxValue));
        }
        public void PickADay_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

            List<string> workingday = new();                    //i olika platser i min kod fick jag meddelande att "new" uttryck kan vara förenklad -  så jag gjorde det. Före redigering var:  List<string> workingday = new List<string>();
            List<string> weekend = new();
            DateTime startDate = new(2022, 01, 01, 18, 00, 00);
            DateTime endWorkingday = new(2022, 01, 01, 22, 00, 00);
            DateTime endWeekend = new(2022, 01, 02, 00, 00, 00);
            if (PickADay.SelectedDate != null)
            {
                var picker = sender as DatePicker;
                var date = picker.SelectedDate.Value.DayOfWeek.ToString();  //DatePicker är begränsad med BlackoutDates så jag tycker inte att  det finns risk för ArgumentOutOfRangeException - därför jag använder inte try-catch här
                if (date == "Saturday" || date == "Sunday")
                {
                    HelpMethods.AvailableHours(startDate, endWeekend, picker, weekend, TimeComboBox);
                }
                else
                {
                    HelpMethods.AvailableHours(startDate, endWorkingday, picker, workingday, TimeComboBox);
                }
            }
            else
            {
                BokaEnabled();
            }
        }

        private async void Boka_Click(object sender, RoutedEventArgs e)
        {
            bool needMessageBox = true;
            HelpMethods.InstertToList(needMessageBox, history, new BookingInfo(PickADay.Text, TimeComboBox.Text, TableComboBox.Text, NameTextBox.Text, AmountOfPeopleCombobox.Text));
            await HelpMethods.UpdateListToFile(history);
            DisplayContent();
            Clear();
        }
        private void DisplayContent()
        {
            ConfirmedList.ItemsSource = null;
            ConfirmedList.ItemsSource = history;
        }

        private async void Avboka_Click(object sender, RoutedEventArgs e)
        {
            if (ConfirmedList.SelectedItem == null)
            {
                return;
            }
            else
            {
                history.Remove((BookingInfo)ConfirmedList.SelectedItem);
                await HelpMethods.UpdateListToFile(history);
                DisplayContent();
            }
        }
        private void Clear()
        {
            PickADay.SelectedDate = null;
            TimeComboBox.ItemsSource = null;
            TableComboBox.SelectedValue = null;
            AmountOfPeopleCombobox.SelectedValue = null;
            NameTextBox.Text = "";
        }
        private void BokaEnabled()
        {
            if (PickADay.SelectedDate == null || TimeComboBox.SelectedItem == null || TableComboBox.SelectedItem == null || NameTextBox.Text.Length == 0 || AmountOfPeopleCombobox.SelectedItem == null)
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
        private void AmountOfPeopleCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BokaEnabled();
        }
        private void StartExamplesRandom()  //metod för att lägga till ett antal bokningar redo att visas
        {
            bool needMessageBox = false;
            string[] exampleNames = { "Gunilla", "Birgitta", "Anders", "Magnus", "Sigvard" };
            string[] hour = { "18:00", "18:30", "19:00", "19:30", "20:00", "20:30", "21:00", "21:30", "22:00" };
            while (history.Count < 20)    //för att minska antalet av bokningar redo att visas med start av programmet, jag rekommenderar att ändra siffra vid (history.Count < ) 
            {
                Random rnd = new();
                int randomDay = rnd.Next(0, 30); 
                DateTime randomDate = DateTime.Now.AddDays(randomDay);                                          //OBS! Metoden använder inte HelpMethods.updateListToFile(); - Metoden sparar inte data till filen log
                int nameIndex = rnd.Next(exampleNames.Length);
                int hourIndex = rnd.Next(hour.Length);
                int table = rnd.Next(1, 6);
                int occupiedSeats = rnd.Next(1, 6);
                HelpMethods.InstertToList(needMessageBox, history, new BookingInfo(randomDate.ToShortDateString(), hour[hourIndex], table.ToString(), exampleNames[nameIndex], occupiedSeats.ToString()));
                DisplayContent();
            }
        }
        /// <summary>
        ///Jag skapade savetoFile metoden för att skapa fil med  eget namn. Annars metoden updateListToFile hanterar lista uppdateringar i fil med namn log (dagens datum) (filen uppdaterar sig värje gång man trycker på boka och avboka knappar)
        /// </summary>       
        private async void SaveToFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveDialog = new()
            {
                FileName = $"Backup {DateTime.Now.ToShortDateString()}",
                DefaultExt = ".json",
                Filter = "Backup files (.json)|*.json"
            };
            Nullable<bool> result = saveDialog.ShowDialog();
            if (result == true)
            {
                try
                {
                    using FileStream createStream = File.Create(saveDialog.FileName);
                    await JsonSerializer.SerializeAsync(createStream, history);
                    await createStream.DisposeAsync();
                }
                catch (Exception Ex)
                {

                    MessageBox.Show(Ex.Message, "oops!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private async void LoadFromFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openDialog = new()
            {
                FileName = $"Backup {DateTime.Now.ToShortDateString()}",
                DefaultExt = ".json",
                Filter = "Backup files (.json)|*.json"
            };
            Nullable<bool> result = openDialog.ShowDialog();
            if (result == true)
            {
                history.Clear();
                try
                {
                    using FileStream openStream = File.OpenRead(openDialog.FileName);
                    List<BookingInfo>? getBookInfo = await JsonSerializer.DeserializeAsync<List<BookingInfo>>(openStream);
                    if (getBookInfo != null)
                    {
                        history.AddRange(getBookInfo);
                    }
                }
                catch (Exception Ex)
                {

                    MessageBox.Show(Ex.Message, "oops!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                DisplayContent();
            }
        }
    }
}
