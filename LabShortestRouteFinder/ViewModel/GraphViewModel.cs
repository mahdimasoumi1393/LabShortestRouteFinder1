using LabShortestRouteFinder;
using LabShortestRouteFinder.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

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
    }
}
