using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AP4Activity3
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Result : ContentPage
    {
        public Result()
        {
            InitializeComponent();
            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                // Get all persons from the SQLite database
                var personList = await App.SQLiteDb.GetItemsAsync();

                // Set the data as the ItemsSource for the ListView
                sPersons.ItemsSource = personList;
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}


