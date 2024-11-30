using LabShortestRouteFinder.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace LabShortestRouteFinder.ViewModel
{
    public class RouteViewModel : INotifyPropertyChanged
    {


        public ObservableCollection<Route> _routes { get; set; }
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
        public RouteViewModel(MainViewModel mainViewModel)
        {
            // Reference the shared Routes collection
            Routes = mainViewModel.Routes;
            //foreach(var route in Routes)
            //{

            //}
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            System.Diagnostics.Debug.WriteLine($"From RouteViewModel - The property Changed: {propertyName}");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
