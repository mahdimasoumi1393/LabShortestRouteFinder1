using LabShortestRouteFinder;
using LabShortestRouteFinder.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.Json;

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
            var matchedRoutes = Routes
                .Where(r => r.Start.Name == start.Name && r.Destination.Name == destination.Name)
                .ToList();

            System.Diagnostics.Debug.WriteLine($"Matched Routes Count: {matchedRoutes.Count}");

            foreach (var route in matchedRoutes)
            {
                System.Diagnostics.Debug.WriteLine($"Route Start: {route.Start.Name}, Destination: {route.Destination.Name}, DrivingDistance: {route.DrivingDistance}");
            }

            if (matchedRoutes.Count >= 2)
            {
                var sortedRoutes = matchedRoutes.OrderBy(r => r.DrivingDistance).ToList();
                FastestRoutes.Clear();
                FastestRoutes.Add(sortedRoutes[0]);

                NonFastRoutes.Clear();
                NonFastRoutes.Add(sortedRoutes[1]);
            }
            else
            {
                FastestRoutes.Clear();
                NonFastRoutes.Clear();
                System.Diagnostics.Debug.WriteLine("Not enough routes found for comparison.");
            }
        }


    }
}
