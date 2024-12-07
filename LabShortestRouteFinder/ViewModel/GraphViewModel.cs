using LabShortestRouteFinder;
using LabShortestRouteFinder.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace LabShortestRouteFinder.ViewModel
{
    public class GraphViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Route> _routes;
        public ObservableCollection<CityNode> Cities { get; set; }

        
        public ObservableCollection<Route> Routes
        {
            get
            { return _routes; }
            set
            {
                _routes = value;
                OnPropertyChanged(nameof(Routes));
            }
        }

        private ObservableCollection<Route> _fastestRoutes = new();
        public ObservableCollection<Route> FastestRoutes
        {
            get => _fastestRoutes;
            set
            {
                _fastestRoutes = value;
                OnPropertyChanged(nameof(FastestRoutes));
            }
        }

        private ObservableCollection<Route> _nonFastRoutes = new();
        public ObservableCollection<Route> NonFastRoutes
        {
            get => _nonFastRoutes;
            set
            {
                _nonFastRoutes = value;
                OnPropertyChanged(nameof(NonFastRoutes));
            }
        }


        public GraphViewModel(MainViewModel mainViewModel)
        {
            Cities = mainViewModel.Cities;
            Routes = mainViewModel.Routes;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            System.Diagnostics.Debug.WriteLine($"From RouteViewModel - The property Changed: {propertyName}");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void FindShortestAndLongestRoutes(CityNode start, CityNode destination)
        {
            // Find all matching routes between the start and destination cities
            var matchedRoutes = Routes
                .Where(r => r.Start.Name == start.Name && r.Destination.Name == destination.Name)
                .ToList();

            System.Diagnostics.Debug.WriteLine($"Matched Routes Count: {matchedRoutes.Count}");

            foreach (var route in matchedRoutes)
            {
                System.Diagnostics.Debug.WriteLine($"Route Start: {route.Start.Name}, Destination: {route.Destination.Name}, DrivingDistance: {route.DrivingDistance}");
            }

            if (matchedRoutes.Any())
            {
                // Sort the routes by DrivingDistance in ascending order
                var sortedRoutes = matchedRoutes.OrderBy(r => r.DrivingDistance).ToList();

                // Clear and update FastestRoutes and NonFastRoutes
                FastestRoutes.Clear();
                NonFastRoutes.Clear();

                // Add the route with the lowest DrivingDistance to FastestRoutes
                FastestRoutes.Add(sortedRoutes[0]);
                NormalizeCoordinatesForRoutes(FastestRoutes);

                // Add all other routes to NonFastRoutes
                for (int i = 1; i < sortedRoutes.Count; i++)
                {
                    NonFastRoutes.Add(sortedRoutes[i]);
                }
                NormalizeCoordinatesForRoutes(NonFastRoutes);
            }
            else
            {
                // Clear both lists if no routes are found
                FastestRoutes.Clear();
                NonFastRoutes.Clear();
                System.Diagnostics.Debug.WriteLine("No routes found for the specified start and destination cities.");

                // Show message box to inform the user
                MessageBox.Show("No routes found between those cities!", "No Routes Found", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void NormalizeCoordinatesForRoutes(IEnumerable<Route> routes)
        {
            int maxX = Cities.Max(c => c.X);
            int maxY = Cities.Max(c => c.Y);

            foreach (var route in routes)
            {
                route.Start.X = (route.Start.X * 433) / maxX; // Normalize to Canvas width
                route.Start.Y = (route.Start.Y * 842) / maxY; // Normalize to Canvas height
                route.Destination.X = (route.Destination.X * 433) / maxX; // Normalize to Canvas width
                route.Destination.Y = (route.Destination.Y * 842) / maxY; // Normalize to Canvas height
            }
        }

    }
}
