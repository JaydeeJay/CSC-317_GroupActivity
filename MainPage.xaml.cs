using System.Collections.Specialized;
using System.Reflection.Metadata;

namespace MauiApp1
{
    public class SeatingUnit
    {
        public string Name { get; set; }
        public bool Reserved { get; set; }

        public SeatingUnit(string name, bool reserved = false)
        {
            Name = name;
            Reserved = reserved;
        }

    }

    public partial class MainPage : ContentPage
    {
        SeatingUnit[,] seatingChart = new SeatingUnit[5, 10];

        public MainPage()
        {
            InitializeComponent();
            GenerateSeatingNames();
            RefreshSeating();
        }

        private async void ButtonReserveSeat(object sender, EventArgs e)
        {
            var seat = await DisplayPromptAsync("Enter Seat Number", "Enter seat number: ");

            if (seat != null)
            {
                for (int i = 0; i < seatingChart.GetLength(0); i++)
                {
                    for (int j = 0; j < seatingChart.GetLength(1); j++)
                    {
                        if (seatingChart[i, j].Name == seat)
                        {
                            seatingChart[i, j].Reserved = true;
                            await DisplayAlert("Successfully Reserverd", "Your seat was reserverd successfully!", "Ok");
                            RefreshSeating();
                            return;
                        }
                    }
                }

                await DisplayAlert("Error", "Seat was not found.", "Ok");
            }
        }

        private void GenerateSeatingNames()
        {
            List<string> letters = new List<string>();
            for (char c = 'A'; c <= 'Z'; c++)
            {
                letters.Add(c.ToString());
            }

            int letterIndex = 0;

            for (int row = 0; row < seatingChart.GetLength(0); row++)
            {
                for (int column = 0; column < seatingChart.GetLength(1); column++)
                {
                    seatingChart[row, column] = new SeatingUnit(letters[letterIndex] + (column + 1).ToString());
                }

                letterIndex++;
            }
        }

        private void RefreshSeating()
        {
            grdSeatingView.RowDefinitions.Clear();
            grdSeatingView.ColumnDefinitions.Clear();
            grdSeatingView.Children.Clear();

            for (int row = 0; row < seatingChart.GetLength(0); row++)
            {
                var grdRow = new RowDefinition();
                grdRow.Height = 50;

                grdSeatingView.RowDefinitions.Add(grdRow);

                for (int column = 0; column < seatingChart.GetLength(1); column++)
                {
                    var grdColumn = new ColumnDefinition();
                    grdColumn.Width = 50;

                    grdSeatingView.ColumnDefinitions.Add(grdColumn);

                    var text = seatingChart[row, column].Name;

                    var seatLabel = new Label();
                    seatLabel.Text = text;
                    seatLabel.HorizontalOptions = LayoutOptions.Center;
                    seatLabel.VerticalOptions = LayoutOptions.Center;
                    seatLabel.BackgroundColor = Color.Parse("#333388");
                    seatLabel.Padding = 10;

                    if (seatingChart[row, column].Reserved == true)
                    {
                        //Change the color of this seat to represent its reserved.
                        seatLabel.BackgroundColor = Color.Parse("#883333");

                    }

                    Grid.SetRow(seatLabel, row);
                    Grid.SetColumn(seatLabel, column);
                    grdSeatingView.Children.Add(seatLabel);

                }
            }
        }

        //Mirion Draper
        private void ButtonReserveRange(object sender, EventArgs e)
        {
            var seatRange = await DisplayPromptAsync("Enter Seat Range", "enter seat range (ex: A1:A10.", "Ok");

            if (seatRange != null)
            {
                var seats = seatRange.Split(':');
                if (seats.Length != 2)
                {
                    await DisplayAlert("Error", "Invalid format. Please format your range like A1:A10", "Ok");
                    return;
                }

                string startSeat = seats[0];
                string endSeat = seats[1];
                char startRow = startSeat[0];
                char endRow = endSeat[0]
                int startColumn = int.Parse(startSeat.Substring(1));
                int endColumn = int.Parse(endSeat.Substring(1));

                if (startRow != endRow)
                {
                    await DisplayALert("Error", "The start and end seats have to be in the same row.", "Ok");
                    return;
                }

                if (startColumn > endColumn)
                {
                    await DisplayAlert("Error", "Invalid Range. The start seat must come before the end seat.", "Ok");
                    return;
                }

                for (int column = startColumn; column <= endColumn; column++)
                {
                    string seat = $"(startRow){column}";

                    bool seatFound = false;
                    for (int i = 0; i < seatingChart.GetLength(0); i++)
                    {
                        for (int j = 0; j < seaatingChart.GetLength(1); j++)
                        {
                            if (seatingChart[i, j].Name == seat)
                            {
                                seatFound = true;
                                if (seatingChart[i, j].Reserved)
                                {
                                    await DisplayAlert("Error!", $"Seat {seat} is already reserved!", "Ok");
                                    return;
                                }
                            }
                        }
                    }

                    if (!seatFound)
                    {
                        await DisplayAlert("Error!", $"Seat {seat} not found!", "Ok");
                        return;
                    }
                }
                for (int column = startColumn; column <= endColumn; column++)
                {
                    string seat = $"{startRow}{column}";

                    for (int i = 0; i < seatingChart.GetLength(0); i++)
                    {
                        for (int j = 0; j < seatingChart.GetLength(1); j++)
                        {
                            if (seatingChart[i, j].Name == seat)
                            {
                                seatingChart[i, j].Reserved = true;
                            }
                        }
                    }
                }
                    await DisplayAlert("Congradulations!", "All seats in range have been reserved!", "Ok");
                    RefreshSeating();
                
            }
        }

        //Guy Dickerson
        private void ButtonCancelReservation(object sender, EventArgs e)
        {
            var seat = await DisplayPromptAsync("Enter Seat Number", "Enter seat number to cancel reservation: ");

            if (seat != null)
            {
                for (int i = 0; i < seatingChart.GetLength(0); i++)
                {
                    for (int j = 0; j < seatingChart.GetLength(1); j++)
                    {
                        if (seatingChart[i, j].Name == seat)
                        {
                            if (seatingChart[i, j].Reserved)
                            {
                                seatingChart[i, j].Reserved = false;
                                await DisplayAlert("Reservation Cancelled", "Your reservation has been cancelled successfully.", "Ok");
                                RefreshSeating();
                                return;
                            }
                            else
                            {
                                await DisplayAlert("Error", "This seat is not reserved.", "Ok");
                                return;
                            }
                        }
                    }
                }
                await DisplayAlert("Error", "Seat was not found.", "Ok");
            }
        }

        //Patrick Seals
        public void CancelReservationRange(string seatRange)
{
    // Split the seatRange input
    var seats = seatRange.Split(':');
    if (seats.Length != 2)
    {
        DisplayAlert("Error", "Invalid seat range format. Use format like A1:A4.", "OK");
        return;
    }

    string startSeat = seats[0];
    string endSeat = seats[1];

    // Extract row letters and seat numbers
    char startRow = startSeat[0];
    char endRow = endSeat[0];
    int startColumn = int.Parse(startSeat.Substring(1));
    int endColumn = int.Parse(endSeat.Substring(1));

    // Validate that both seats are in the same row
    if (startRow != endRow)
    {
        DisplayAlert("Error", "The start and end seats must be in the same row.", "OK");
        return;
    }

    // Validate that the columns are in the correct order
    if (startColumn > endColumn)
    {
        DisplayAlert("Error", "Invalid range. The start seat should come before the end seat.", "OK");
        return;
    }

    // Loop through the seats in the specified range
    for (int column = startColumn; column <= endColumn; column++)
    {
        string seat = $"{startRow}{column}";

        // Assume we have a method IsSeatReserved(seat) that checks if a seat is reserved
        if (!IsSeatReserved(seat))
        {
            DisplayAlert("Error", $"Seat {seat} is not reserved or invalid.", "OK");
            return;
        }
    }

    // Cancel reservation for all seats in the range
    for (int column = startColumn; column <= endColumn; column++)
    {
        string seat = $"{startRow}{column}";

        // Assume we have a method CancelSeat(seat) that cancels the reservation of the seat
        CancelSeat(seat);
    }

    DisplayAlert("Success", "All seats in the range have been cancelled.", "OK");
}


        //Josh
        private async void ButtonResetSeatingChart(object sender, EventArgs e)
        {
            
            //this part is just for the popup. it returns a bool value
            
            var confirmation = await DisplayAlert("Reset seating chart?", "This action cannot be undone.", "Yes", "No");
            
            //if the popup returns yes it just loops thru the seats and sets Reserved to false
            if (confirmation == true)
            {
                for (int i = 0; i < seatingChart.GetLength(0); i++)
                {
                    for (int j = 0; j < seatingChart.GetLength(1); j++)
                    {
                        seatingChart[i, j].Reserved = false;
                    }
                }

                RefreshSeating();
            }
            else
            {
                return;
            }
        }
    }

}
