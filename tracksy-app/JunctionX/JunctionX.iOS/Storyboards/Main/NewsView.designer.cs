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
	[Register ("NewsView")]
	partial class NewsView
	{
		[Outlet]
		UIKit.UITableView News { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (News != null) {
				News.Dispose ();
				News = null;
			}
		}
	}
}
