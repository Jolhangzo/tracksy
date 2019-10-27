using System;
using CoreGraphics;
using Foundation;
using JunctionX.Models;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace JunctionX.iOS.Storyboards.Main.Cells
{
    public partial class NewsCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("NewsCell");
        public static readonly UINib Nib;

        static NewsCell()
        {
            Nib = UINib.FromName("NewsCell", NSBundle.MainBundle);
        }

        protected NewsCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
            InitBinding();
        }

        public void InitBinding()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<NewsCell, Article>();
                set.Bind(CoverImageView).For("ImagePath").To(vm => vm.ImageUrl);
                set.Bind(TitleLabel).To(vm => vm.Title);
                set.Bind(DescriptionLabel).To(vm => vm.Description);
                set.Apply();
            });
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            ShadowView.Layer.CornerRadius = 10f;
            CoverImageView.Layer.MaskedCorners = (CoreAnimation.CACornerMask)3;
            CoverImageView.Layer.CornerRadius = 10f;
            ShadowView.Layer.MasksToBounds = false;
            ShadowView.Layer.ShadowColor = new UIColor(0f, 0f, 0f, 0.3f).CGColor;
            ShadowView.Layer.ShadowOpacity = 12f;
            ShadowView.Layer.ShadowRadius = 2f;
            ShadowView.Layer.ShadowOffset = new CGSize(0f, 0f);
        }
    }
}
