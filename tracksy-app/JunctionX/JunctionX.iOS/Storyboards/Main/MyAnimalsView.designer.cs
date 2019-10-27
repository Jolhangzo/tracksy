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
	[Register ("MyAnimalsView")]
	partial class MyAnimalsView
	{
		[Outlet]
		UIKit.UITableView Animals { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (Animals != null) {
				Animals.Dispose ();
				Animals = null;
			}
		}
	}
}
