using LabShortestRouteFinder.Model;
using LabShortestRouteFinder.ViewModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace LabShortestRouteFinder.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public RouteViewModel ViewModel { get; set; }
        public MainViewModel MainViewModel { get; set; }

        public RouteViewModel RouteViewModel { get; set; }
        public GraphViewModel GraphViewModel { get; set; }
        public ObservableCollection<CityNode> Cities { get; set; }
        public ObservableCollection<Route> Routes { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            MainViewModel = new MainViewModel();
            System.Diagnostics.Debug.WriteLine($"MainViewModel Initialized: {MainViewModel != null}");
            RouteViewModel = new RouteViewModel(MainViewModel);
            GraphViewModel = new GraphViewModel(MainViewModel);

            //DataContext = MainViewModel;
            //this.DataContext = RouteViewModel;
            DataContext = new MainViewModel();
            this.DataContext = MainViewModel;

        }

        private void OnNavigationSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NavigationListBox.SelectedItem is ListBoxItem selectedItem)
            {
                string? tabName = selectedItem.Tag as string;

                foreach (TabItem tab in MainTabControl.Items)
                {
                    if (tab.Name == tabName)
                    {
                        MainTabControl.SelectedItem = tab;
                        break;
                    }
                }
            }
        }
        //private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        //{
        //    if (e.EditAction == DataGridEditAction.Commit)
        //    {
        //        // Force update of the source collection
        //        var dataGrid = sender as DataGrid;
        //        dataGrid?.CommitEdit(DataGridEditingUnit.Row, true);
        //    }
        //}

        //private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        //{
        //    var dataGrid = sender as DataGrid;

        //    if (e.EditAction == DataGridEditAction.Commit)
        //    {
        //        // Temporarily detach event handler to avoid recursion
        //        dataGrid.RowEditEnding -= DataGrid_RowEditEnding;

        //        try
        //        {
        //            // Commit the edit to update the bound source
        //            dataGrid.CommitEdit(DataGridEditingUnit.Row, true);
        //        }
        //        finally
        //        {
        //            // Reattach the event handler
        //            dataGrid.RowEditEnding += DataGrid_RowEditEnding;
        //        }
        //    }
        //}



        private void SaveRouteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MainViewModel == null)
                {
                    MessageBox.Show("MainViewModel is not properly initialized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Ensure Cities and Routes collections are initialized
                if (MainViewModel.Cities == null || MainViewModel.Routes == null)
                {
                    MessageBox.Show("Cities or Routes collection is not initialized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Validate that Cities and Routes have content
                if (!MainViewModel.Cities.Any() || !MainViewModel.Routes.Any())
                {
                    MessageBox.Show("No Cities or Routes to save.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Debug info
                System.Diagnostics.Debug.WriteLine($"Saving Cities Count: {MainViewModel.Cities.Count}");
                System.Diagnostics.Debug.WriteLine($"Saving Routes Count: {MainViewModel.Routes.Count}");

                // Save both Cities and Routes
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources");
                string filePath = Path.Combine(directoryPath, "Routes.json");
                MainViewModel.SaveRouteToJson(filePath);
                MessageBox.Show("Routes and Cities saved to JSON file!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //private void SaveRouteButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (MainViewModel == null)
        //        {
        //            MessageBox.Show("MainViewModel is not properly initialized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //            return;
        //        }

        //        // Validate NewRoute
        //        var newRoute = MainViewModel.NewRoute;
        //        if (newRoute.Start == null || newRoute.Destination == null ||
        //            newRoute.DrivingDistance <= 0 || newRoute.Cost <= 0)
        //        {
        //            MessageBox.Show("Invalid route details. Please complete all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //            return;
        //        }

        //        // Add NewRoute to Routes
        //        MainViewModel.Routes.Add(new Route
        //        {
        //            Start = newRoute.Start,
        //            Destination = newRoute.Destination,
        //            DrivingDistance = newRoute.DrivingDistance,
        //            StraightLineDistance = newRoute.StraightLineDistance,
        //            Cost = newRoute.Cost
        //        });

        //        // Reset NewRoute for new input
        //        MainViewModel.NewRoute = new Route
        //        {
        //            Start = null,
        //            Destination = null
        //        };

        //        // Save all data to JSON
        //        MainViewModel.SaveRouteToJson();
        //        MessageBox.Show("Route added and saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"An error occurred while saving data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}





        private void SaveCityButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Add the new city to the Cities collection
                if (DataContext is MainViewModel viewModel)
                {
                    var newCity = new CityNode
                    {
                        Name = viewModel.NewCity.Name,
                        Latitude = viewModel.NewCity.Latitude,
                        Longitude = viewModel.NewCity.Longitude,
                        X = 0, // Or calculate X and Y if needed
                        Y = 0
                    };
                    viewModel.Cities.Add(newCity);

                    // Save to JSON
                    var filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName ?? string.Empty, "Resources", "routes.json");
                    MainViewModel.SaveCityNodeToJson(filePath);

                    MessageBox.Show("City saved successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

    }
}
