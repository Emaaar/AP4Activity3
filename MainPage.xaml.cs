using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AP4Activity3
{
     public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        

        private async void btnAdd_Clicked(object sender, EventArgs e)
        {
            if (
         !string.IsNullOrEmpty(txtPresentReading.Text) &&
         !string.IsNullOrEmpty(txtPreviousReading.Text))
            {
                // Validate meter number


                if (double.TryParse(txtPresentReading.Text, out double presentReading) &&
                    double.TryParse(txtPreviousReading.Text, out double previousReading))
                {
                    if (presentReading >= previousReading)
                    {
                        double consumptionReading = presentReading - previousReading;

                        double electricityCharge = 0;
                        double kwh = 0;
                        if (consumptionReading < 72)
                        {
                            kwh = 6.50;
                        }
                        else if (consumptionReading <= 150)
                        {
                            kwh = 9.50;
                        }
                        else if (consumptionReading <= 300)
                        {
                            kwh = 10.50;
                        }
                        else if (consumptionReading <= 400)
                        {
                            kwh = 12.50;
                        }
                        else if (consumptionReading <= 500)
                        {
                            kwh = 14.00;
                        }
                        else
                        {
                            kwh = 16.50;
                        }
                        electricityCharge = kwh * consumptionReading;
                        double demandCharge = 0;
                        double serviceCharge = 0;

                        
                        if (H.IsChecked && B.IsChecked)
                        {
                            // Alert the user if both checkboxes are checked
                            await DisplayAlert("Error", "Please select only one type of registration", "OK");
                            return;
                        }
                        else if (H.IsChecked)
                        {
                           
                            demandCharge = 300;
                            serviceCharge = 200;
                        }
                        else if (B.IsChecked)
                        {
                           
                            demandCharge = 600;
                            serviceCharge = 400;
                        }
                        else
                        {
                            // Alert the user if no checkbox is checked
                            await DisplayAlert("Error", "Please select a type of registration", "OK");
                            return;
                        }

                       
                        double principalAmount = electricityCharge + demandCharge + serviceCharge;
                        double vat = principalAmount * 0.05;
                        double amountPayable = principalAmount + vat;

                        Person person = new Person()
                        {

                            PrincipalAmount = principalAmount.ToString(), // No need to convert to string
                            AmountPayable = amountPayable.ToString()// No need to convert to string
                        };

                        await App.SQLiteDb.SaveItemAsync(person);

                        // Show success message
                        await DisplayAlert("Success", "Data saved successfully", "OK");

                        

                        await Navigation.PushAsync(new Result());

                    }
                    else
                    {
                        await DisplayAlert("Error", "Present reading should be greater than or equal to previous reading", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Invalid reading values", "OK");
                }
            }
            else
            {
                await DisplayAlert("Required", "Please enter all fields", "OK");
            }

        }

        private async void btnUpdate_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMeterNo.Text))
            {
                // Check if meter number is provided
                if (!int.TryParse(txtMeterNo.Text, out int meterNumber))
                {
                    await DisplayAlert("Error", "Invalid meter number format. Meter number should be an integer.", "OK");
                    return;
                }

                // Search for the person using the meter number
                var searchResult = await App.SQLiteDb.GetItemByMeterNoAsync(txtMeterNo.Text.Trim());

                if (searchResult == null)
                {
                    await DisplayAlert("Error", "Person not found", "OK");
                    return;
                }

                if (!string.IsNullOrEmpty(txtPresentReading.Text) &&
                    !string.IsNullOrEmpty(txtPreviousReading.Text))
                {
                    if (double.TryParse(txtPresentReading.Text, out double presentReading) &&
                        double.TryParse(txtPreviousReading.Text, out double previousReading))
                    {
                        if (presentReading >= previousReading)
                        {
                            double consumptionReading = presentReading - previousReading;

                            // Calculate charges
                            double electricityCharge = 0;
                            if (consumptionReading < 72)
                            {
                                electricityCharge = consumptionReading * 6.50;
                            }
                            else if (consumptionReading <= 150)
                            {
                                electricityCharge = consumptionReading * 9.50;
                            }
                            else if (consumptionReading <= 300)
                            { 
                                electricityCharge = consumptionReading * 10.50;
                            }
                            else if (consumptionReading <= 400)
                            { 
                                electricityCharge = consumptionReading * 12.50;
                            }
                            else if (consumptionReading <= 500)
                            { 
                                electricityCharge = consumptionReading * 14.00;
                            }
                            else
                            { 
                                electricityCharge = consumptionReading * 16.50;
                            }

                            double demandCharge = 0;
                            double serviceCharge = 0;

                            if (H.IsChecked && B.IsChecked)
                            {
                                // Alert the user if both checkboxes are checked
                                await DisplayAlert("Error", "Please select only one type of registration", "OK");
                                return;
                            }
                            else if (H.IsChecked)
                            {

                                demandCharge = 300;
                                serviceCharge = 200;
                            }
                            else if (B.IsChecked)
                            {

                                demandCharge = 600;
                                serviceCharge = 400;
                            }
                            else
                            {
                                // Alert the user if no checkbox is checked
                                await DisplayAlert("Error", "Please select a type of registration", "OK");
                                return;
                            }

                            

                            double principalAmount = electricityCharge + demandCharge + serviceCharge;
                            double vat = principalAmount * 0.05;
                            double amountPayable = principalAmount + vat;

                            // Update Person's properties with calculated values
                            searchResult.PrincipalAmount = principalAmount.ToString();
                            searchResult.AmountPayable = amountPayable.ToString();

                            // Save the updated Person to the database
                            await App.SQLiteDb.SaveItemAsync(searchResult);

                            // Show success message
                            await DisplayAlert("Success", "Data updated successfully", "OK");

                           
                            await Navigation.PushAsync(new Result());
                        }
                        else
                        {
                            await DisplayAlert("Error", "Present reading should be greater than or equal to previous reading", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "Invalid reading values", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Required", "Please enter all fields", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Please enter a meter number", "OK");
            }

        }

        private async void btnDelete_Clicked(object sender, EventArgs e)
        {

            string meterNoToDelete = txtMeterNo.Text.Trim();

            if (string.IsNullOrWhiteSpace(meterNoToDelete))
            {
                await DisplayAlert("Error", "Please enter a Meter Number to delete", "OK");
                return;
            }

            // Prompt the user to confirm the deletion
            bool deleteConfirmed = await DisplayAlert("Confirm", "Are you sure you want to delete this item?", "Yes", "No");

            if (deleteConfirmed)
            {
                // Delete the item from the database
                int rowsAffected = await App.SQLiteDb.DeleteItemByMeterNoAsync(meterNoToDelete);

                if (rowsAffected > 0)
                {
                    await DisplayAlert("Success", "Item deleted successfully", "OK");

                    // Refresh the ListView after deletion
                    //await RefreshListView();
                    await Navigation.PushAsync(new Result());
                }
                else
                {
                    await DisplayAlert("Error", "Failed to delete item", "OK");
                }
            }

        }
    }
}
