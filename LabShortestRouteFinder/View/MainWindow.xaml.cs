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
        
        public MainViewModel MainViewModel { get; set; }

        public RouteViewModel RouteViewModel { get; set; }
        public GraphViewModel GraphViewModel { get; set; }
       
        public MainWindow()
        {
            InitializeComponent();

            MainViewModel = new MainViewModel();
            System.Diagnostics.Debug.WriteLine($"MainViewModel Initialized: {MainViewModel != null}");
            RouteViewModel = new RouteViewModel(MainViewModel);
            GraphViewModel = new GraphViewModel(MainViewModel);

            this.DataContext = MainViewModel;
            GraphViewControl.DataContext = GraphViewModel; // Explicitly set the GraphViewControl's DataContext
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


        private void FindRouteButton_Click(object sender, RoutedEventArgs e)
        {
            if (StartCityComboBox.SelectedItem is CityNode startCity && DestinationCityComboBox.SelectedItem is CityNode destinationCity)
            {
                System.Diagnostics.Debug.WriteLine($"Selected Start City: {startCity.Name}, Selected Destination City: {destinationCity.Name}");
                GraphViewModel.FindShortestAndLongestRoutes(startCity, destinationCity);
            }
            else
            {
                MessageBox.Show("Please select both Start and Destination cities.");
            }
        }

    }
}
