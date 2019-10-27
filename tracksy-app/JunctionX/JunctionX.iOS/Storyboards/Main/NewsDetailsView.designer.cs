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
	[Register ("NewsDetailsView")]
	partial class NewsDetailsView
	{
		[Outlet]
		FFImageLoading.Cross.MvxCachedImageView CoverImageView { get; set; }

		[Outlet]
		UIKit.UILabel DateLabel { get; set; }

		[Outlet]
		UIKit.UILabel DescriptionLabel { get; set; }

		[Outlet]
		UIKit.UILabel TitleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CoverImageView != null) {
				CoverImageView.Dispose ();
				CoverImageView = null;
			}

			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}

			if (DateLabel != null) {
				DateLabel.Dispose ();
				DateLabel = null;
			}

			if (DescriptionLabel != null) {
				DescriptionLabel.Dispose ();
				DescriptionLabel = null;
			}
		}
	}
}
