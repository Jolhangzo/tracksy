// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace JunctionX.iOS.Storyboards.Main.Cells
{
	[Register ("AnimalCell")]
	partial class AnimalCell
	{
		[Outlet]
		FFImageLoading.Cross.MvxCachedImageView AnimLogoImageView { get; set; }

		[Outlet]
		UIKit.UILabel DateLabel { get; set; }

		[Outlet]
		UIKit.UILabel LocationLabel { get; set; }

		[Outlet]
		UIKit.UILabel NameLabel { get; set; }

		[Outlet]
		UIKit.UIView ShadowContentView { get; set; }

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

			if (ShadowContentView != null) {
				ShadowContentView.Dispose ();
				ShadowContentView = null;
			}
		}
	}
}
