using System;
using CoreGraphics;
using Foundation;
using JunctionX.Models;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace JunctionX.iOS.Storyboards.Main.Cells
{
    public partial class AnimalCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("AnimalCell");
        public static readonly UINib Nib;

        static AnimalCell()
        {
            Nib = UINib.FromName("AnimalCell", NSBundle.MainBundle);
        }

        protected AnimalCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
            InitBinding();
        }

        public void InitBinding()
        {
            this.DelayBind(() =>
            {
                Animal animal = (Animal)DataContext;
                var set = this.CreateBindingSet<AnimalCell, Animal>();
                set.Bind(NameLabel).To(vm => vm.Name);
                set.Bind(TypeLabel).To(vm => vm.Type);
                set.Bind(DateLabel).To(vm => vm.BirthdateValue);
                set.Bind(LocationLabel).To(vm => vm.Location);
                set.Bind(AnimLogoImageView).For("Transformations").To("AnimalImageTransformations");
                set.Bind(AnimLogoImageView).For("ImagePath").To(vm => vm.ImageUrl);
                set.Apply();
                AnimLogoImageView.Transformations = animal.AnimalImageTransformations;
                AnimLogoImageView.ImagePath = animal.ImageUrl;
            });
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            ShadowContentView.Layer.CornerRadius = 10f;
            ShadowContentView.Layer.MasksToBounds = false;
            ShadowContentView.Layer.ShadowColor = new UIColor(0f, 0f, 0f, 0.3f).CGColor;
            ShadowContentView.Layer.ShadowOpacity = 12f;
            ShadowContentView.Layer.ShadowRadius = 2f;
            ShadowContentView.Layer.ShadowOffset = new CGSize(0f, 0f);
        }
    }
}
