using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreGraphics;
using CoreLocation;
using Google.Maps;
using JunctionX.iOS.Misc;
using JunctionX.ViewModels.Main;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace JunctionX.iOS.Storyboards.Main
{
	public partial class MyAnimalActivityView : MvxViewController<MyAnimalActivityViewModel>
	{
        public Google.Maps.MapView mapView;

        private static CLLocationCoordinate2D? lastCameraPosition;
        private static float lastCameraZoom;
        private static readonly int defaultZoom = 5;
        private bool animate;
        private Marker marker = new Marker();
        private Polyline[] Polylines = new Polyline[] { };
        private DateTime[] Dates = new DateTime[] { };
        private Circle[] Circles = new Circle[] { };
        public MyAnimalActivityView (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            CameraPosition camera = CameraPosition.FromCamera(GetInitialCameraPosition(),
                                                              GetInitialCameraZoom());
            mapView = Google.Maps.MapView.FromCamera(CGRect.Empty, camera);
            mapView.MyLocationEnabled = false;
            mapView.Settings.MyLocationButton = false;
            marker.IconView = new UIImageView(UIImage.FromBundle(ViewModel.Animal.ImageUrl));
            marker.Map = mapView;
            var set = this.CreateBindingSet<MyAnimalActivityView, MyAnimalActivityViewModel>();
            set.Bind(NameLabel).To(vm => vm.Animal.Name);
            set.Bind(TypeLabel).To(vm => vm.Animal.Type);
            set.Bind(LocationLabel).To(vm => vm.Animal.Location);
            set.Apply();
            AnimLogoImageView.Transformations = ViewModel.Animal.AnimalImageTransformations;
            AnimLogoImageView.ImagePath = ViewModel.Animal.ImageUrl;
            DateLabel.Text = ViewModel.Animal.BirthdateValue.ToString();

            View.AddSubview(mapView);
            mapView.Frame = MapPlaceHolderView.Frame;
            ViewModel.Logs.Changed += delegate {
                if (ViewModel.Logs.Value != null)
                {
                    mapView.Animate(new CameraPosition(new CLLocationCoordinate2D(ViewModel.Logs.Value[0].Coordinate.Latitude,
                        ViewModel.Logs.Value[0].Coordinate.Longitude), defaultZoom));
                    Polylines = new Polyline[ViewModel.Logs.Value.Length - 1];
                    Dates = new DateTime[ViewModel.Logs.Value.Length - 1];
                    Circles = new Circle[ViewModel.Logs.Value.Length];
                    for (int i = 0; i < ViewModel.Logs.Value.Length - 1; i++)
                    {
                        var path = new Google.Maps.MutablePath();
                        var polyline = new Google.Maps.Polyline();
                        path.AddCoordinate(new CLLocationCoordinate2D(latitude: ViewModel.Logs.Value[i].Coordinate.Latitude, longitude: ViewModel.Logs.Value[i].Coordinate.Longitude));
                        path.AddCoordinate(new CLLocationCoordinate2D(latitude: ViewModel.Logs.Value[i + 1].Coordinate.Latitude, longitude: ViewModel.Logs.Value[i + 1].Coordinate.Longitude));
                        polyline.Path = path;
                        polyline.StrokeColor = CustomColor.JunctionXBlue;
                        polyline.StrokeWidth = 2.5f;
                        Polylines[i] = polyline;
                        Dates[i] = ViewModel.Logs.Value[i + 1].DateTimeValue;
                        var circle = new Circle();
                        circle.Position = new CLLocationCoordinate2D(latitude: ViewModel.Logs.Value[i].Coordinate.Latitude, longitude: ViewModel.Logs.Value[i].Coordinate.Longitude);
                        circle.StrokeColor = CustomColor.JunctionXBlue;
                        circle.FillColor = CustomColor.JunctionXBlue;
                        circle.Radius = 7.5f;

                        Circles[i] = circle;
                        if (i + 1 == ViewModel.Logs.Value.Length - 1)
                            Circles[i + 1] = circle;

                    }
                    TimeLabel.Text = Dates[0].ToString();
                    Slider.Value = 0;
                    Slider.MinValue = 0;
                    Slider.MaxValue = ViewModel.Logs.Value.Length - 1;
                    Slider.TouchDragEnter += delegate {
                        animate = false;
                        StartButton.SetImage(UIImage.FromBundle("icPlayCircleFilled24Px"), UIControlState.Normal);
                    };
                    Slider.ValueChanged += delegate {
                        HandlePolylines();
                    };
                    HandlePolylines();

                    StartButton.TouchDown += delegate {
                        if ((int)Slider.Value != (int)Slider.MaxValue)
                        {
                            animate = !animate;
                            if (animate)
                                StartButton.SetImage(UIImage.FromBundle("icPauseCircleFilled24Px"), UIControlState.Normal);
                            else
                                StartButton.SetImage(UIImage.FromBundle("icPlayCircleFilled24Px"), UIControlState.Normal);
                            if (animate)
                                Animate();
                        }

                    };
                }
            };
        }

        private async void Animate()
        {
            while (animate)
            {
                StartButton.SetImage(UIImage.FromBundle("icPauseCircleFilled24Px"), UIControlState.Normal);
                if ((int)Slider.Value != (int)Slider.MaxValue)
                    foreach (var item in Polylines)
                    {
                        if ((int)Slider.Value != (int)Slider.MaxValue && animate)
                        {
                            Slider.Value = Slider.Value + 1;
                            HandlePolylines();
                            await Task.Delay(500);
                        }
                        if ((int)Slider.Value == (int)Slider.MaxValue)
                        {
                            animate = false;
                            StartButton.SetImage(UIImage.FromBundle("icPlayCircleFilled24Px"), UIControlState.Normal);
                        }
                    }
                else
                    StartButton.SetImage(UIImage.FromBundle("icPlayCircleFilled24Px"), UIControlState.Normal);
            }
        }

        private void HandlePolylines()
        {
            var sliderInt = (int)Slider.Value;
            Console.WriteLine("SliderInt " + sliderInt);
            marker.Position = new CLLocationCoordinate2D(ViewModel.Logs.Value[(int)Slider.Value].Coordinate.Latitude, ViewModel.Logs.Value[(int)Slider.Value].Coordinate.Longitude);
            for (int i = 0; i < sliderInt; i++)
            {
                if ( i != ViewModel.Logs.Value.Length - 1)
                    Polylines[i].Map = mapView;
                Circles[i].Map = mapView;
                TimeLabel.Text = Dates[i].ToString();
            }
            for (int i = ViewModel.Logs.Value.Length - 2; i > sliderInt - 1; i--)
            { 
                Polylines[i].Map = null;
                Circles[i].Map = null;
                TimeLabel.Text = Dates[i].ToString();
                if (i != ViewModel.Logs.Value.Length - 1)
                {
                    Circles[i + 1].Map = null;
                    TimeLabel.Text = Dates[i].ToString();
                }
            }
            if (sliderInt == 0)
            {
                foreach (var item in Polylines)
                {
                    item.Map = null;
                }
                foreach (var item in Circles)
                {
                    item.Map = null;
                }
                TimeLabel.Text = Dates[0].ToString();
            }
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            mapView.Frame = MapPlaceHolderView.Frame;
            View.BringSubviewToFront(TimeView);
            TimeView.Layer.MaskedCorners = (CoreAnimation.CACornerMask)3;
            TimeView.Layer.CornerRadius = 10f;
        }

         public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NavigationItem.Title = "Activity";
        }

        private CLLocationCoordinate2D GetInitialCameraPosition()
        {
            if (lastCameraPosition != null)
                return lastCameraPosition.Value;
            if (ViewModel.Logs.Value != null)
            {
                if (ViewModel.Logs.Value[0] != null)
                    return new CLLocationCoordinate2D(ViewModel.Logs.Value[0].Coordinate.Latitude,
                        ViewModel.Logs.Value[0].Coordinate.Longitude);
            }
            return new CLLocationCoordinate2D(47.49801, 19.03991);
        }

        private float GetInitialCameraZoom()
        {
            return lastCameraPosition == null ? defaultZoom : lastCameraZoom;
        }

    }
}
