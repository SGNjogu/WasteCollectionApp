using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Diagnostics;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace ttnm.Services.Maps
{
    [INotifyPropertyChanged]
    public partial class MapService : IMapService
    {
        /// <summary>
        /// Bind this to the UI from your ViewModel
        /// You'll have to make MapService an ObservableProperty
        /// </summary>
        [ObservableProperty]
        public Map map;

        private Location _location;
        private MapSpan _mapSpan;

        private CancellationTokenSource _cancellationTokenSource;
        private GeolocationRequest _geolocationRequest;

        public event Action<MapClickedEventArgs> OnMapClicked;

        public MapService()
        {
            Map = new Map
            {
                MapType = MapType.Street,
                IsScrollEnabled = true,
                IsZoomEnabled = true,
                IsTrafficEnabled = true,
                IsShowingUser = false
            };
            Map.MapClicked += MapClicked;
            SetDefaultLocation();
        }

        /// <summary>
        /// Listen to OnMapClicked event in your ViewModel
        /// </summary>
        private void MapClicked(object sender, MapClickedEventArgs e)
        {
            OnMapClicked?.Invoke(e);
        }

        /// <summary>
        /// Default location is Nairobi CBD
        /// </summary>
        public void SetDefaultLocation()
        {
            UpdateMapLocation(new Location(-1.282903396750622, 36.818594759644796));
        }

        /// <summary>
        /// Sets the current user's location in the Map
        /// </summary>
        public async Task SetUsersLocation()
        {
            UpdateMapLocation(await GetCurrentLocation());
        }

        /// <summary>
        /// Updates the current location in the map
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        public void UpdateMapLocation(double latitude, double longitude)
        {
            UpdateMapLocation(new Location(latitude, longitude));
        }

        /// <summary>
        /// Updates the current location in the map
        /// </summary>
        /// <param name="location"></param>
        public void UpdateMapLocation(Location location)
        {
            _location = location;
            _mapSpan = MapSpan.FromCenterAndRadius(_location, Distance.FromKilometers(0.444));
            Map.MoveToRegion(_mapSpan);
        }

        /// <summary>
        /// Adds a pin to the current location
        /// </summary>
        /// <param name="label">Name of the place</param>
        /// <param name="adress">Address of the place</param>
        public void AddPinToCurrentLocation(string label = "", string adress = "")
        {
            Pin pin = new Pin
            {
                Label = label,
                Address = adress,
                Type = PinType.Place,
                Location = _location
            };
            Map.Pins.Add(pin);
        }

        /// <summary>
        /// Zooms the map
        /// </summary>
        /// <param name="zoomLevel"></param>
        public void ZoomMap(double zoomLevel = 0.5)
        {
            double latlongDegrees = 360 / (Math.Pow(2, zoomLevel));

            if (Map.VisibleRegion != null)
            {
                Map.MoveToRegion(new MapSpan(Map.VisibleRegion.Center, latlongDegrees, latlongDegrees));
            }
        }

        /// <summary>
        /// Gets the device current location
        /// </summary>
        /// <param name="oneTimeLocation"></param>
        /// <returns>True if you don't want to continously listen to the location</returns>
        public async Task<Location> GetCurrentLocation(bool oneTimeLocation = true)
        {
            try
            {
                _geolocationRequest = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                _cancellationTokenSource = new CancellationTokenSource();
                Location location = await Geolocation.GetLocationAsync(_geolocationRequest, _cancellationTokenSource.Token);

                if (location != null)
                {
                    Debug.WriteLine($"Current Location => Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");

                    if (oneTimeLocation)
                    {
                        if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
                            _cancellationTokenSource.Cancel();
                    }
                    return location;
                }
                else
                {
                    return await GetLastKnownLocation();
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                throw fnsEx;
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                throw fneEx;
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                throw pEx;
            }
            catch
            {
                // Unable to get location
                throw;
            }
        }

        /// <summary>
        /// Gets the device last known location
        /// </summary>
        /// <returns>Return Location object</returns>
        public async Task<Location> GetLastKnownLocation()
        {
            try
            {
                Location location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    Debug.WriteLine($"Last Known Location => Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    return location;
                }
                else
                {
                    return null;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                throw fnsEx;
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                throw fneEx;
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                throw pEx;
            }
            catch
            {
                // Unable to get location
                throw;
            }
        }

        /// <summary>
        /// Generates a static map image url
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="apiKey"></param>
        /// <param name="zoom"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns>Returns the image url in PNG format</returns>
        public string RenderStaticMapImage
            (
            double lat,
            double lng,
            string apiKey,
            int zoom = 16,
            int width = 600,
            int height = 600
            )
        {
            return $"https://maps.googleapis.com/maps/api/staticmap?center={lat},{lng}&zoom={zoom}&size={width}x{height}&maptype=roadmap&markers=color:red%7Clabel:S%7C{lat},{lng}&sensor=false&key={apiKey}";
        }

        /// <summary>
        /// Generates a location url
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <returns>Returns the location url</returns>
        public string CreateLocationLink(double lat, double lng)
        {
            return $"https://maps.google.com/?ll={lat},{lng}";
        }
    }
}
