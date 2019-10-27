using System;
using CoreGraphics;
using CoreLocation;
using GMCluster;
using Google.Maps;
using MapKit;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using JunctionX.ViewModels.Main;
using JunctionX.Models;
using System.Collections.Generic;
using System.Linq;
using JunctionX.iOS.Misc;
using UIKit;
using Foundation;

namespace JunctionX.iOS.Storyboards.Main
{
    [MvxTabPresentation(WrapInNavigationController = true, TabName = "map", TabIconName = "map")]
    public partial class MapView : MvxViewController<MapViewModel>,
        IGMUClusterManagerDelegate, IMKMapViewDelegate, IMapViewDelegate
    {
        public Google.Maps.MapView mapView;
        private GMUClusterManager clusterManager;
        GMUDefaultClusterRenderer renderer;
        private static CLLocationCoordinate2D? lastCameraPosition;
        private static float lastCameraZoom;
        private static readonly int defaultZoom = 3;
        private List<Animal> oldItems = new List<Animal>();
        public Animal selectedAnimal;

        public MapView(IntPtr handle) : base(handle)
        {
        }

        #region VC life cycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var set = this.CreateBindingSet<MapView, MapViewModel>();
            set.Apply();
            CameraPosition camera = CameraPosition.FromCamera(GetInitialCameraPosition(),
                GetInitialCameraZoom());
            mapView = Google.Maps.MapView.FromCamera(CGRect.Empty, camera);
            mapView.MyLocationEnabled = true;
            mapView.Settings.MyLocationButton = true;
            mapView.CameraPositionIdle += MapView_CameraPositionIdle;
            mapView.InfoTapped += MapView_InfoTapped;
            mapView.CoordinateTapped += MapView_CoordinateTapped;
            //mapView.WillMove += MapView_WillMove;
            ViewModel.Items.Changed += ItemsChanged;
            View.AddSubview(mapView);
            mapView.Frame = MapPlaceHolderView.Frame;

            var iconGenerator = new GMUDefaultClusterIconGenerator();
            var algorithm = new GMUNonHierarchicalDistanceBasedAlgorithm();

            renderer = new GMUDefaultClusterRenderer(mapView, iconGenerator)
            {
                Delegate = new JunctionXClusterRendererDelegate(this)
            };

            clusterManager = new GMUClusterManager(mapView, algorithm, renderer);
            mapView.TappedMarker += (map, marker) => SelectMarker(marker);

            ActivityButton.TouchDown += ActivityButton_TouchDown;
            ActivityButton2.TouchDown += ActivityButton_TouchDown;
            AdoptButton.TouchDown += AdoptButton_TouchDown;
            DetailsButton.TouchDown += DetailsButton_TouchDown;
            DetailsButton2.TouchDown += DetailsButton_TouchDown;
            CloseButton.TouchDown += CloseButton_TouchDown;

            //var gest = new UITapGestureRecognizer(() => { View.SendSubviewToBack(MarkerInfoView);});
            //TapView.AddGestureRecognizer(gest);
        }

        private void CloseButton_TouchDown(object sender, EventArgs e)
        {
            View.SendSubviewToBack(MarkerInfoView);
        }

        private void DetailsButton_TouchDown(object sender, EventArgs e)
        {
            ViewModel.OpenDetailActivity(selectedAnimal);
        }

        private void AdoptButton_TouchDown(object sender, EventArgs e)
        {
            //Create Alert
            var okCancelAlertController = UIAlertController.Create("Do you want to adopt?", "For a short amount of $200 you can adopt this animal.", UIAlertControllerStyle.Alert);

            //Add Actions
            okCancelAlertController.AddAction(UIAlertAction.Create("Adopt", UIAlertActionStyle.Default, alert => {
                Console.WriteLine("Okay was clicked");
                selectedAnimal.Owner = "JunctionX";
                MyAnimalsViewModel.AddAnimal(selectedAnimal);
                if (selectedAnimal.Owner == null || selectedAnimal.Owner == "")
                {
                    AdoptView.Hidden = true;
                    ButtonsView1.Hidden = false;
                    ButtonsView2.Hidden = true;
                }
                else
                {
                    AdoptView.Hidden = false;
                    ButtonsView1.Hidden = true;
                    ButtonsView2.Hidden = false;
                }

                OwnerLabel.Text = selectedAnimal.Owner;
            }));
            okCancelAlertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, alert => Console.WriteLine("Cancel was clicked")));

            //Present Alert
            PresentViewController(okCancelAlertController, true, null);
        }

        private void ActivityButton_TouchDown(object sender, EventArgs e)
        {
            ViewModel.OpenAnimalActivity(selectedAnimal);
        }

        //private void MapView_WillMove(object sender, GMSWillMoveEventArgs e)
        //{
        //    if(!isInfoOn)
        //        View.SendSubviewToBack(MarkerInfoView);
        //}

        private void MapView_CoordinateTapped(object sender, GMSCoordEventArgs e)
        {
            View.SendSubviewToBack(MarkerInfoView);
        }

        private void MapView_InfoTapped(object sender, GMSMarkerEventEventArgs e)
        {
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            mapView.Frame = MapPlaceHolderView.Frame;
            AdoptButton.Layer.CornerRadius = 10f;
            ActivityButton2.Layer.CornerRadius = 10f;
            ActivityButton.Layer.CornerRadius = 10f;
            ActivityButton.Layer.BorderColor = CustomColor.JunctionXGreen.CGColor;
            ActivityButton.Layer.BorderWidth = 2f;
            DetailsButton.Layer.CornerRadius = 10f;
            DetailsButton.Layer.BorderColor = CustomColor.JunctionXGreen.CGColor;
            DetailsButton.Layer.BorderWidth = 2f;
            DetailsButton2.Layer.CornerRadius = 10f;
            DetailsButton2.Layer.BorderColor = CustomColor.JunctionXGreen.CGColor;
            DetailsButton2.Layer.BorderWidth = 2f;
            NavigationController.NavigationBar.TopItem.Title = "Map";
            var textAttributes = new UIStringAttributes();
            textAttributes.ForegroundColor = UIColor.White;
            NavigationController.NavigationBar.TitleTextAttributes = textAttributes;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NavigationController.NavigationBar.TopItem.Title = "Map";
            var textAttributes = new UIStringAttributes();
            textAttributes.ForegroundColor = UIColor.White;
            NavigationController.NavigationBar.TitleTextAttributes = textAttributes;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        #endregion

        #region View Setup

        private void MapView_CameraPositionIdle(object sender, GMSCameraEventArgs e)
        {
            var visibleRegion = mapView.Projection.VisibleRegion;
            var bounds = new CoordinateBounds(visibleRegion);
            lastCameraPosition = mapView.Camera.Target;
            lastCameraZoom = mapView.Camera.Zoom;
            //ViewModel.CurrentBounds.Value = new GeoLocationBounds
            //{
            //    NorthEast = new MvxCoordinates
            //    {
            //        Longitude = bounds.NorthEast.Longitude,
            //        Latitude = bounds.NorthEast.Latitude
            //    },
            //    SouthWest = new MvxCoordinates
            //    {
            //        Longitude = bounds.SouthWest.Longitude,
            //        Latitude = bounds.SouthWest.Latitude
            //    }
            //};
        }

        private CLLocationCoordinate2D GetInitialCameraPosition()
        {
            if (lastCameraPosition != null)
                return lastCameraPosition.Value;
            if (ViewModel.CurrentLocation != null)
            {
                return new CLLocationCoordinate2D(ViewModel.CurrentLocation.Latitude,
                    ViewModel.CurrentLocation.Longitude);
            }

            return new CLLocationCoordinate2D(47.49801, 19.03991);
        }

        private float GetInitialCameraZoom()
        {
            return lastCameraPosition == null ? defaultZoom : lastCameraZoom;
        }

        public bool SelectMarker(Marker marker)
        {
            if (!(marker.UserData is GMUStaticCluster) && marker.UserData != null)
            {
                SetupInfoView(marker);
                View.BringSubviewToFront(MarkerInfoView);
                //marker.Title = ((ShopClusterItem)marker.UserData).Title;
                //ViewModel.SelectedShopId.Value = ((ShopClusterItem)marker.UserData).PublicShopId;
            }

            return false;
        }

        private void SetupInfoView(Marker marker)
        {
            Animal data = ViewModel.Items.Value.Where(x => x.AnimalId == ((AnimalClusterItem) marker.UserData).AnimalId)
                .First();
            selectedAnimal = data;
            NameLabel.Text = data.Name;
            TypeLabel.Text = data.Type;
            DateLabel.Text = data.BirthdateValue.ToString();
            CoordinateLabel.Text = data.Coordinate.Latitude + "," + data.Coordinate.Longitude;
            AnimLogoImageView.Transformations = data.AnimalImageTransformations;
            AnimLogoImageView.ImagePath = data.ImageUrl;
            if (data.Owner == null || data.Owner == "")
            {
                AdoptView.Hidden = true;
                ButtonsView1.Hidden = false;
                ButtonsView2.Hidden = true;
            }
            else
            {
                AdoptView.Hidden = false;
                ButtonsView1.Hidden = true;
                ButtonsView2.Hidden = false;
            }

            OwnerLabel.Text = data.Owner;
        }


        private List<AnimalClusterItem> clusterItems = new List<AnimalClusterItem>();
        private void ItemsChanged(object sender, EventArgs e)
        {
            if (mapView == null)
                return;

            var clusterChanged = false;
            foreach (var newItem in ViewModel.Items.Value)
            {
                var exitingCi = clusterItems.FirstOrDefault(x => x.AnimalId == newItem.AnimalId);
                if (exitingCi != null)
                {
                    exitingCi.Position = new CLLocationCoordinate2D(newItem.Coordinate.Latitude, newItem.Coordinate.Longitude);
                    clusterChanged = true;
                }
                else
                {
                    AnimalClusterItem item = new AnimalClusterItem(
                     new CLLocationCoordinate2D(newItem.Coordinate.Latitude, newItem.Coordinate.Longitude), newItem.AnimalId, newItem.Name,
                     newItem.ImageUrl);
                    clusterItems.Add(item);
                    clusterManager.AddItem(item);
                    clusterChanged = true;
                }
            }
            if (clusterChanged)
                clusterManager.Cluster();




            //List<Animal> addableItems = new List<Animal>();
            ////foreach (var newItem in ViewModel.Items.Value)
            ////{
            ////    var equals = false;
            ////    if (oldItems.Count > 0)
            ////        foreach (var oldItem in oldItems)
            ////        {
            ////            if (newItem.AnimalId == oldItem.AnimalId)
            ////                equals = true;
            ////        }

            ////    if (!equals)
            ////        addableItems.Add(newItem);
            ////}

            //clusterManager.ClearItems();
            //AnimalClusterItem[] items = ViewModel.Items.Value.Select(x => new AnimalClusterItem(
            //       new CLLocationCoordinate2D(x.Coordinate.Latitude, x.Coordinate.Longitude), x.AnimalId, x.Name,
            //       x.ImageUrl)).ToArray();
            //clusterManager.AddItems(items);
            //oldItems.AddRange(addableItems);
            ////if (addableItems.Count > 0)
            ////{
            //    clusterManager.Cluster();
            //    if (mapView.SelectedMarker != null)
            //        SelectMarker(mapView.SelectedMarker);
            //}
        }

        public void ChangeCoordinateLbael(float lat, float longi)
        {
            CoordinateLabel.Text = lat + "," + longi;
        }

        #endregion

        #region Keyboard Observing

        #endregion
    }

    public class JunctionXClusterRendererDelegate : GMUClusterRendererDelegate
    {
        private MapView view;

        public JunctionXClusterRendererDelegate(MapView view)
        {
            this.view = view;
        }

        public override void WillRenderMarker(GMUClusterRenderer renderer, Overlay marker)
        {
            if (marker.UserData is AnimalClusterItem && marker is Marker m)
            {
                switch (((AnimalClusterItem) marker.UserData).ImageName)
                {
                    case "bird_marker1":
                        m.IconView = new UIImageView(UIImage.FromBundle("bird_marker1"));
                        break;
                    case "bird_marker2":
                        m.IconView = new UIImageView(UIImage.FromBundle("bird_marker2"));
                        break;
                    case "bird_marker3":
                        m.IconView = new UIImageView(UIImage.FromBundle("bird_marker3"));
                        break;
                    default:
                        break;
                }
            }
        }

        [Export("renderer:didRenderMarker:")]
        public void DidRenderMarker(GMUClusterRenderer renderer, Overlay marker)
        {
            if (marker.UserData is AnimalClusterItem && marker is Marker m)
            {
                if (view.selectedAnimal == null)
                    return;
                if (view.selectedAnimal.AnimalId == ((AnimalClusterItem)marker.UserData).AnimalId)
                {
                    view.ChangeCoordinateLbael(view.selectedAnimal.Coordinate.Latitude, view.selectedAnimal.Coordinate.Longitude);
                    //Task.Delay(1500).GetAwaiter().OnCompleted(() => {
                    //    view.mapView.SelectedMarker = m;
                    //    view.SelectMarker(m);
                    //});
                }
            }
        }
    }
}