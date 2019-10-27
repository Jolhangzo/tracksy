// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace JunctionX.iOS.Storyboards.Main
{
	[Register ("MyAnimalActivityView")]
	partial class MyAnimalActivityView
	{
		[Outlet]
		FFImageLoading.Cross.MvxCachedImageView AnimLogoImageView { get; set; }

		[Outlet]
		UIKit.UILabel DateLabel { get; set; }

		[Outlet]
		UIKit.UILabel LocationLabel { get; set; }

		[Outlet]
		UIKit.UIView MapPlaceHolderView { get; set; }

		[Outlet]
		UIKit.UILabel NameLabel { get; set; }

		[Outlet]
		UIKit.UISlider Slider { get; set; }

		[Outlet]
		UIKit.UIButton StartButton { get; set; }

		[Outlet]
		UIKit.UILabel TimeLabel { get; set; }

		[Outlet]
		UIKit.UIView TimeView { get; set; }

		[Outlet]
		UIKit.UILabel TypeLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (AnimLogoImageView != null) {
				AnimLogoImageView.Dispose ();
				AnimLogoImageView = null;
			}

			if (NameLabel != null) {
				NameLabel.Dispose ();
				NameLabel = null;
			}

			if (TypeLabel != null) {
				TypeLabel.Dispose ();
				TypeLabel = null;
			}

			if (DateLabel != null) {
				DateLabel.Dispose ();
				DateLabel = null;
			}

			if (LocationLabel != null) {
				LocationLabel.Dispose ();
				LocationLabel = null;
			}

			if (StartButton != null) {
				StartButton.Dispose ();
				StartButton = null;
			}

			if (Slider != null) {
				Slider.Dispose ();
				Slider = null;
			}

			if (MapPlaceHolderView != null) {
				MapPlaceHolderView.Dispose ();
				MapPlaceHolderView = null;
			}

			if (TimeLabel != null) {
				TimeLabel.Dispose ();
				TimeLabel = null;
			}

			if (TimeView != null) {
				TimeView.Dispose ();
				TimeView = null;
			}
		}
	}
}
