using System;
using JunctionX.iOS.Misc;
using JunctionX.ViewModels.Main;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace JunctionX.iOS.Storyboards.Main
{
	public partial class AnimalDetailsView : MvxViewController<AnimalDetailsViewModel>
	{
		public AnimalDetailsView (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var set = this.CreateBindingSet<AnimalDetailsView, AnimalDetailsViewModel>();
            set.Bind(MainImageView).For("Transformations").To("Animal.AnimalImageTransformations");
            set.Bind(MainImageView).For("ImagePath").To(vm => vm.Animal.ImageUrl);
            set.Bind(NameLabel).To(vm => vm.Animal.Name);
            set.Bind(TypeLabel).To(vm => vm.Animal.Type);
            set.Bind(AgeValueLabel).To(vm => vm.Animal.BirthdateValue);
            set.Bind(PositionLabel).To(vm => vm.Animal.Coordinate);
            set.Bind(OwnerLabel).To(vm => vm.Animal.Owner);
            set.Apply();

            MainImageView.Transformations = ViewModel.Animal.AnimalImageTransformations;
            MainImageView.ImagePath = ViewModel.Animal.ImageUrl;

            PositionLabel.Text = ViewModel.Animal.Coordinate.Latitude + "," + ViewModel.Animal.Coordinate.Longitude;
            if(ViewModel.Animal.PhotoUrls != null)
            {
                ImageView1.ImagePath = ViewModel.Animal.PhotoUrls[0];
                ImageView2.ImagePath = ViewModel.Animal.PhotoUrls[1];
                ImageView3.ImagePath = ViewModel.Animal.PhotoUrls[2];
                //ImageView4.ImagePath = ViewModel.Animal.PhotoUrls[3];
                //ImageView5.ImagePath = ViewModel.Animal.PhotoUrls[4];
                //ImageView6.ImagePath = ViewModel.Animal.PhotoUrls[5];
            }

            if (ViewModel.Animal.Owner == null || ViewModel.Animal.Owner == "")
            {
                AdoptedView.Hidden = true;
                ButtonView1.Hidden = false;
                ButtonView2.Hidden = true;
            }
            else
            {
                AdoptedView.Hidden = false;
                ButtonView1.Hidden = true;
                ButtonView2.Hidden = false;
            }

            ActivityButton.TouchDown += ActivityButton_TouchDown;
            ActivityButton2.TouchDown += ActivityButton_TouchDown;
            AdoptButton.TouchDown += AdoptButton_TouchDown;

            AdoptButton.Layer.CornerRadius = 10f;
            ActivityButton.Layer.CornerRadius = 10f;
            ActivityButton2.Layer.CornerRadius = 10f;

            ActivityButton.Layer.CornerRadius = 10f;
            ActivityButton.Layer.BorderColor = CustomColor.JunctionXGreen.CGColor;
            ActivityButton.Layer.BorderWidth = 2f;
        }

        private void AdoptButton_TouchDown(object sender, EventArgs e)
        {
            //Create Alert
            var okCancelAlertController = UIAlertController.Create("Do you want to adopt?", "For a short amount of $200 you can adopt this animal.", UIAlertControllerStyle.Alert);

            //Add Actions
            okCancelAlertController.AddAction(UIAlertAction.Create("Adopt", UIAlertActionStyle.Default, alert => { Console.WriteLine("Okay was clicked");
                ViewModel.Animal.Owner = "JunctionX";
                MyAnimalsViewModel.AddAnimal(ViewModel.Animal);
                if (ViewModel.Animal.Owner == null || ViewModel.Animal.Owner == "")
                {
                    AdoptedView.Hidden = true;
                    ButtonView1.Hidden = false;
                    ButtonView2.Hidden = true;
                }
                else
                {
                    AdoptedView.Hidden = false;
                    ButtonView1.Hidden = true;
                    ButtonView2.Hidden = false;
                }

                OwnerLabel.Text = ViewModel.Animal.Owner;
            }));
            okCancelAlertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, alert => Console.WriteLine("Cancel was clicked")));

            //Present Alert
            PresentViewController(okCancelAlertController, true, null);
        }

        private void ActivityButton_TouchDown(object sender, EventArgs e)
        {
            ViewModel.OpenAnimalActivity(ViewModel.Animal);
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NavigationItem.Title = ViewModel.Animal.Name;
            var textAttributes = new UIStringAttributes();
            textAttributes.ForegroundColor = UIColor.White;
            NavigationController.NavigationBar.TitleTextAttributes = textAttributes;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }
    }
}
