﻿
---
## ViewModel

GraphViewModel and RouteViewModel has to be synchronized in that way any changes on the Routes should be displayed on the Graph.

In order to keep both ViewModels synchronized will be done by using a **share data source** in a central MainViewModel. This approach allows each ViewModel to manage its own view-specific logic while still being notified of changes in the shared data.

Solution: Use `MainViewModel` as a shared data source with Change Notification.

- **Shared Data Collection** in `MainViewModel`
	Store the central data in `MainViewModel` as `ObservableCollection<Route> Routes` and `ObservableCollection<CityNode> Cities`.`
	objects so changes are automatically propagated to the ViewModels.
- **Use `INotifyPropertyChanged`**
	By exposing collections as `ObsevableCollection` and implementing `INotifyPropertyChanged`, you can keep track of changes and notify dependent ViewModels of changes to the shared data.`
- **Bind both ViewModels to `MainViewModel`**
	Both `GraphViewModel` and `RouteViewModel` will bind to the shared data in `MainViewModel` to keep the views synchronized.`

### `RouteViewModel` and `GraphViewModel` Interaction` 
Now, both `RouteViewModel` and `GraphViewModel` will be constructed with a reference to `MainViewModel`. This way, they can observe changes in the shared data.

- **`RouteViewModel`**
	- `RouteViewModel` will bind to the `Routes` collection in `MainViewModel` to display the list of routes.
	- `RouteViewModel` will also have a method to add a new route to the `Routes` collection in `MainViewModel`.
- **`GraphViewModel`**
	- `GraphViewModel` will bind to the `Cities` collection in `MainViewModel` to display the graph.
	- `GraphViewModel` will also have a method to add a new city to the `Cities` collection in `MainViewModel`.

### Initialize and Bind ViewModels in `MainViewModel`
In `MainViewModel`, you will initialize both `RouteViewModel` and `GraphViewModel` and bind them to the shared data.

## **Summary base structure**
- **Centralized Data**: `MainViewModel` holds the shared data (`Cities` and `Routes`).
- **Separate ViewModels**: `RouteViewModel` and `GraphViewModel` access the shared data by referencing `MainViewModel`.
- **Data Synchronization**: Updates in `MainViewModel` collections are automatically reflected in both views due to `ObservableCollection` and change notification mechanisms.
- **Shared Data Collection** in MainViewModel
- **Use INotifyPropertyChanged**
- **Bind both ViewModels to MainViewModel**
- **RouteViewModel and GraphViewModel Interaction**
- **Initialize and Bind ViewModels in MainViewModel**

### Make Changes on the list reflect in the Graph.
To make the changes in the ListViewControl reflect in the GraphViewControl, particularly when changing the name of the start position, you’ll need to ensure that:

1. **Two-Way Binding** is set up for the `Name` property in `ListViewControl``.
2. **Property Change Notification** is implemented for the `Name` property in the `CityNode` model. This will ensure that when a property changes in the model, all views bound to that property are updated.

### **Summary**
Changes on the  locations names in the ListViewControl will be reflected in the GraphViewControl by setting up two-way binding and implementing property change notification.

### **New Requirements**
- **Add Offset to draw Routes** Start and destination point should be adjust to the middle of the city-node.
  Offset Converter added implemented in f/NewMaoWin

- **Use X and Y coordinates** to represent the position of cities in the GraphViewControl.
   Solution: X and Y coordinates are used on CityNode to represent the position of cities in the GraphViewControl. The X and Y coordinates are calculated based on the position of the city in the GraphViewControl.
	 

- **Add json file discribing cities**: Name, X, Y, Latitude, Longitude
	Solution: X, Y values added with default values, can be calculated when the Map-rectangle and the windows-rectangle are definied.
 
- **Move the transformation window to separate class or function**
	
- **Add json file discribing routes**: Start, Destination, Driving Distance, Straight Line Distance*
	Solution: Start and Destination are the names of the cities, Driving Distance and Straight Line Distance are added with default values, can be calculated when the Map-rectangle and the windows-rectangle are definied.
 
- **Add description of the rectangule to draw the map**: NorthWest, NorthEast, SouthWest and SouthEast corners
	Solution: Add the rectangel on the description of the cities file. The rectangel will be used to draw the map and ecapsulates the cities inside the rectangle.

- **Adjust ListView columns** to display new definition of distance
	Solution: Two new columns added to the ListViewControl to display the new definition of distance. binding to the new properties added to the Route class.

- **Add a new DistanceCalculator class** to calculate the distance between two cities.
	Solution: DistanceCalculator class added to calculate the distance between two cities. The class is used to calculate the driving distance and the straight line distance between two cities.
- **A new class WGS84** added to calculate the distance between two GPS coordinates. On preference the vincenty formula is used to calculate the distance between two GPS coordinates.
	Solution: WGS84DistanceCalculator class added to calculate the distance between two GPS coordinates. The class is used to calculate the driving distance and the straight line distance between two cities.
- 
	 
 
### **Next Requirements**

- **Change the distance** between two cities in the ListViewControl.
- **Add a new route** to the list of routes in the ListViewControl.
- **Remove a route** from the list of routes in the ListViewControl.
- **Load other routes from file** and display them in the ListViewControl.
- X and Y coordinates should be validated to ensure they are within the bounds of the GraphViewControl.
- X and Y coordinates should be saved to a file when changed in the ListViewControl.
- X and Y are windows coordinates and should be derived from the city GPS coordinates when loaded from a file.
- Map the window coordinates to GPS coordinates when saving to a file.
- Find the shortest route between two cities in the GraphViewControl.
- Find cycles in the GraphViewControl.
- Find the shortest cycle in the GraphViewControl.
- Draw the shortest route between two cities in the GraphViewControl with chosen color.
- 

### Closed
- X and Y coordinates should be updated in the GraphViewControl when changed in the ListViewControl.
  Rejected: X and Y coordinates are calculated based on the coordinates of the city. Any change in the X and Y coordinates should be done in the city GPS coordinates.

