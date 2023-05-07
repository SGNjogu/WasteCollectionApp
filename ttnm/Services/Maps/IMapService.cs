using Microsoft.Maui.Controls.Maps;

namespace ttnm.Services.Maps
{
    public interface IMapService
    {
        event Action<MapClickedEventArgs> OnMapClicked;

        void AddPinToCurrentLocation(string label = "", string adress = "");
        string CreateLocationLink(double lat, double lng);
        Task<Location> GetCurrentLocation(bool oneTimeLocation = true);
        Task<Location> GetLastKnownLocation();
        string RenderStaticMapImage(double lat, double lng, string apiKey, int zoom = 16, int width = 600, int height = 600);
        void SetDefaultLocation();
        Task SetUsersLocation();
        void UpdateMapLocation(double latitude, double longitude);
        void UpdateMapLocation(Location location);
        void ZoomMap(double zoomLevel = 0.5);
    }
}