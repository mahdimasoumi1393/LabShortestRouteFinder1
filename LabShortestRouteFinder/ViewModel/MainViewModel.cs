using LabShortestRouteFinder.Converters;
using LabShortestRouteFinder.Helpers;
using LabShortestRouteFinder.Model;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Xml.Linq;
using JsonException = Newtonsoft.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace LabShortestRouteFinder.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<CityNode> Cities { get; set; }
        public ObservableCollection<Route> Routes { get; set; }
        

        private MapTransformer? mapTransformer;

        public MainViewModel()
        {
            // Initialize data here or load from JSON
            Cities = new ObservableCollection<CityNode>();
            Routes = new ObservableCollection<Route>();

            LoadRectangleAndCitiesFromJson();
            LoadRouteInformationFileFromJson();

            NormalizeCoordinates();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void NormalizeCoordinates()
        {
            int maxX = Cities.Max(c => c.X);
            int maxY = Cities.Max(c => c.Y);

            foreach (var city in Cities)
            {
                city.X = (city.X * 433) / maxX; // Normalize to Canvas width
                city.Y = (city.Y * 842) / maxY; // Normalize to Canvas height
            }

            foreach (var route in Routes)
            {
                route.Start.X = (route.Start.X * 433) / maxX; // Normalize to Canvas width
                route.Start.Y = (route.Start.Y * 842) / maxY; // Normalize to Canvas height
                route.Destination.X = (route.Destination.X * 433) / maxX; // Normalize to Canvas width
                route.Destination.Y = (route.Destination.Y * 842) / maxY; // Normalize to Canvas height
            }
        }


        private void LoadRectangleAndCitiesFromJson()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "swCities.json");
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                var jsonDocument = JsonDocument.Parse(jsonString);

                if (jsonDocument.RootElement.ValueKind == JsonValueKind.Array && jsonDocument.RootElement.GetArrayLength() > 0)
                {
                    var rootElement = jsonDocument.RootElement[0];

                    if (rootElement.TryGetProperty("Rectangle", out JsonElement rectangleElement) && rectangleElement.ValueKind == JsonValueKind.Object)
                    {
                        var rectangle = JsonSerializer.Deserialize<RectangleCoordinates>(rectangleElement.GetRawText());
                        var cities = JsonSerializer.Deserialize<List<CityNode>>(rootElement.GetProperty("Cities").GetRawText());

                        if (rectangle != null && cities != null)
                        {
                            foreach (var city in cities)
                            {
                                Cities.Add(city);
                            }
                            // Debug statement to check cities count after loading
                            System.Diagnostics.Debug.WriteLine($"Cities Count After Loading: {Cities.Count}");

                            var minGpsCoord = new Tuple<double, double>(rectangle.SouthWest.Latitude, rectangle.SouthWest.Longitude);
                            var maxGpsCoord = new Tuple<double, double>(rectangle.NorthEast.Latitude, rectangle.NorthEast.Longitude);

                            int windowWidth = (int)WGS84DistanceCalculator.CalculateDistance(rectangle.NorthEast.Latitude, rectangle.NorthEast.Longitude, rectangle.NorthWest.Latitude, rectangle.NorthWest.Longitude);
                            int windowHeight = (int)WGS84DistanceCalculator.CalculateDistance(rectangle.NorthEast.Latitude, rectangle.NorthEast.Longitude, rectangle.SouthEast.Latitude, rectangle.SouthEast.Longitude);

                            var windowMaxXY = new Tuple<int, int>(windowWidth, windowHeight);

                            MapWin mapWin = new MapWin(minGpsCoord, maxGpsCoord, windowMaxXY);
                            mapTransformer = new MapTransformer(mapWin);

                            var transformedCities = mapTransformer.TransformCities(Cities);
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("The 'Rectangle' property is not an object.");
                    }
                }
                else
                {
                    throw new InvalidOperationException("The root element is not a non-empty array.");
                }
            }
            else
            {
                throw new FileNotFoundException($"The file {filePath} was not found.");
            }
        }


        
        private void LoadRouteInformationFileFromJson()
        {
            var filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName ?? string.Empty, "Resources", "routes.json");
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("routes.json file not found.", filePath);
            }

            var fileInfo = new FileInfo(filePath);
            if ((fileInfo.Attributes & FileAttributes.ReadOnly) != 0)
            {
                throw new UnauthorizedAccessException("The routes.json file is read-only.");
            }

            string jsonString = File.ReadAllText(filePath);
            var jsonDocument = JsonDocument.Parse(jsonString);

            if (jsonDocument.RootElement.TryGetProperty("Routes", out var routesElement))
            {
                var routesJson = routesElement.GetRawText();
                var routes = JsonSerializer.Deserialize<List<Route>>(routesJson);

                if (routes != null && mapTransformer != null)
                {
                    Routes.Clear();
                    foreach (var route in routes)
                    {
                        (route.Start.X, route.Start.Y) = mapTransformer.TransformToScreenPosition(route.Start.Latitude, route.Start.Longitude);
                        (route.Destination.X, route.Destination.Y) = mapTransformer.TransformToScreenPosition(route.Destination.Latitude, route.Destination.Longitude);
                        Routes.Add(route);
                    }
                    System.Diagnostics.Debug.WriteLine($"Routes Count After Loading: {Routes.Count}");
                }
                else
                {
                    throw new InvalidOperationException("The routes could not be deserialized or the mapTransformer is null.");
                }
            }
            else
            {
                throw new JsonException("The JSON file does not contain a 'Routes' property.");
            }
        }



        public void SaveRouteToJson(string filePath)
        {
            try
            {
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName ?? string.Empty, "Resources", "routes.json");

                System.Diagnostics.Debug.WriteLine($"Saving to: {filePath}");

                var data = new
                {
                    Cities = this.Cities.Select(c => new { c.Name, c.X, c.Y, c.Latitude, c.Longitude }).ToList(),
                    Routes = this.Routes.Select(r => new
                    {
                        StartName = r.Start.Name, // Unique property name for the start
                        DestinationName = r.Destination.Name, // Unique property name for the destination
                        r.DrivingDistance,
                        r.StraightLineDistance,
                        r.Cost
                    }).ToList()
                };
                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                System.Diagnostics.Debug.WriteLine($"Serialized Data: {json}");

                File.WriteAllText(filePath, json);
                File.SetLastWriteTime(filePath, DateTime.Now);


                System.Diagnostics.Debug.WriteLine("Data successfully saved.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving data: {ex.Message}");
            }
        }


    }
}
