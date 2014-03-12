using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapControl;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KFly.GUI
{
    /// <summary>
    /// This is the viewmodel used for the navigation tab
    /// (data behind)
    /// </summary>
    public class NavigationViewModel:VMBase
    {
        private TileLayerCollection _tileLayers;
        private CollectionViewSource _availableTileLayers;
        private CollectionViewSource _allTileLayers;
        private CollectionViewSource _currentTileLayers;
        private TileLayer _currentTileLayer;

        private Location _center;

        public Location Center
        {
            get { return _center; }
            set 
            { 
                _center = value;
                base.NotifyPropertyChanged("Center");
            }
        }


        public TileLayer CurrentTileLayer
        {
            get { return _currentTileLayer; }
            set 
            { 
                _currentTileLayer = value;
                base.NotifyPropertyChanged("CurrentTileLayer");
            }
        }

        public CollectionViewSource CurrentTileLayers
        {
            get { return _currentTileLayers; }
            set 
            { 
                _currentTileLayers = value;
                base.NotifyPropertyChanged("CurrentTileLayers");
            }
        }

        public NavigationViewModel()
        {
            _center = new Location(65.6196, 22.1520);

            CreateTileLayerCollection();
            _availableTileLayers = new CollectionViewSource()
            {
                Source = _tileLayers,
            };
            _availableTileLayers.View.Filter = item =>
                {
                    KFlyTileLayer tl = item as KFlyTileLayer;
                    return (tl != null)? tl.IsAvailable : false;
                };
            _allTileLayers = new CollectionViewSource()
            {
            //    Source = _tileLayers,
            };
            CurrentTileLayers = _availableTileLayers;
            CurrentTileLayer = CurrentTileLayers.View.CurrentItem as TileLayer;
        }

        private void CreateTileLayerCollection()
        {
            _tileLayers = new TileLayerCollection();
            _tileLayers.Add(new KFlyTileLayer()
            {
                Icon = new BitmapImage(new Uri("../../Resources/OpenStreetMap-logo.jpg", UriKind.Relative)),
                SourceName = "OpenStreetMap",
                Description = "© {y} OpenStreetMap Contributors, CC-BY-SA",
                TileSource = new TileSource("http://{c}.tile.openstreetmap.org/{z}/{x}/{y}.png")
            });
            _tileLayers.Add(new KFlyTileLayer()
            {
                Icon = new BitmapImage(new Uri("../../Resources/OpenCycleMap_icon.png", UriKind.Relative)),
                IsAvailable = true,
                SourceName = "OpenCycleMap",
                Description = "OpenCycleMap - © {y} Andy Allen &amp; OpenStreetMap Contributors, CC-BY-SA",
                TileSource = new TileSource("http://{c}.tile.opencyclemap.org/cycle/{z}/{x}/{y}.png")
            });
         /*    _tileLayers.Add(new KFlyTileLayer()
            {
                Icon = new BitmapImage(new Uri("../../Resources/OpenCycleMap-icon.png", UriKind.Relative)),
                SourceName = "OCM Landscape",
                Description = "OpenCycleMap Landscape - © {y} Andy Allen &amp; OpenStreetMap Contributors, CC-BY-SA",
                TileSource = new TileSource("http://{c}.tile3.opencyclemap.org/landscape/{z}/{x}/{y}.png")
            });
             _tileLayers.Add(new KFlyTileLayer()
            {
                Icon = new BitmapImage(new Uri("../../Resources/OpenCycleMap-icon.png", UriKind.Relative)),
                SourceName = "OCM Transport",
                Description = "OpenCycleMap Transport - © {y} Andy Allen &amp; OpenStreetMap Contributors, CC-BY-SA",
                TileSource = new TileSource("http://{c}.tile2.opencyclemap.org/transport/{z}/{x}/{y}.png")
            });*/
             _tileLayers.Add(new KFlyTileLayer()
            {
                Icon = new BitmapImage(new Uri("../../Resources/MapQuest_icon.png", UriKind.Relative)),
                SourceName = "MapQuest OSM",
                Description = "MapQuest OSM - © {y} MapQuest &amp; OpenStreetMap Contributors",
                TileSource = new TileSource("http://otile{n}.mqcdn.com/tiles/1.0.0/osm/{z}/{x}/{y}.png")
            });

            // Following only for demo, not available
           /*  _tileLayers.Add(new KFlyTileLayer()
            {
                IsAvailable = false,
                SourceName = "Google Maps",
                Description = "Google Maps - © {y} Google",
                TileSource = new TileSource("http://mt{i}.google.com/vt/x={x}&amp;y={y}&amp;z={z}"),
                MaxZoomLevel = 20
            });
             _tileLayers.Add(new KFlyTileLayer()
            {
                IsAvailable = false,
                SourceName = "Google Images",
                Description = "Google Maps - © {y} Google",
                Background = Brushes.LightGray,
                Foreground = Brushes.White,
                TileSource = new TileSource("http://khm{i}.google.com/kh/v=144&amp;x={x}&amp;y={y}&amp;z={z}"),
                MaxZoomLevel = 20
            });*/
             _tileLayers.Add(new KFlyTileLayer()
            {
                IsAvailable = false,
                Icon = new BitmapImage(new Uri("../../Resources/BingMaps_icon.png", UriKind.Relative)),
                SourceName = "Bing Maps",
                Description = "Bing Maps - © {y} Microsoft Corporation",
                Background = Brushes.LightGray,
                Foreground = Brushes.White,
                TileSource = new TileSource("http://ecn.t{i}.tiles.virtualearth.net/tiles/r{q}.png?g=0&amp;stl=h"),
                MinZoomLevel = 1,
                MaxZoomLevel = 19
            });
             _tileLayers.Add(new KFlyTileLayer()
            {
                IsAvailable = false,
                Icon = new BitmapImage(new Uri("../../Resources/BingMaps_icon.png", UriKind.Relative)),
                SourceName = "Bing Images",
                Description = "Bing Maps - © {y} Microsoft Corporation",
                Background = Brushes.LightGray,
                Foreground = Brushes.White,
                TileSource = new TileSource("http://ecn.t{i}.tiles.virtualearth.net/tiles/a{q}.jpeg?g=0"),
                MinZoomLevel = 1,
                MaxZoomLevel = 19
            });
             _tileLayers.Add(new KFlyTileLayer()
            {
                IsAvailable = false,
                Icon = new BitmapImage(new Uri("../../Resources/BingMaps_icon.png", UriKind.Relative)),
                SourceName = "Bing Hybrid",
                Description = "Bing Maps - © {y} Microsoft Corporation",
                Background = Brushes.LightGray,
                Foreground = Brushes.White,
                TileSource = new TileSource("http://ecn.t{i}.tiles.virtualearth.net/tiles/h{q}.jpeg?g=0&amp;stl=h"),
                MinZoomLevel = 1,
                MaxZoomLevel = 19
            });

        }
    }

    public class KFlyTileLayer:TileLayer
    {
        private ImageSource _icon = null;

        public ImageSource Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }
        public Boolean IsChecked = false;
        public Boolean IsAvailable = true;
    }
}
