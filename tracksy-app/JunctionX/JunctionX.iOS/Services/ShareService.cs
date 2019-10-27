using System;
using System.Threading.Tasks;
using Foundation;
using MvvmCross.Platforms.Ios.Views;
using JunctionX.Services;
using UIKit;

namespace JunctionX.iOS.Services
{
    public class ShareService : IShareService
    {
        public Task ShareLink(string message)
        {
            var hostVC = UIApplication.SharedApplication.KeyWindow.GetTopModalHostViewController();
            NSObject[] activityItems = { NSObject.FromObject(message) };
            UIActivityViewController activityViewController = new UIActivityViewController(activityItems, null);
            activityViewController.ExcludedActivityTypes = new NSString[] { };
            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
            {
                activityViewController.PopoverPresentationController.SourceView = hostVC.View;
                activityViewController.PopoverPresentationController.SourceRect = new CoreGraphics.CGRect((hostVC.View.Bounds.Width / 2), (hostVC.View.Bounds.Height / 4), 0, 0);
            }
            hostVC.PresentViewController(activityViewController, true, null);
            return Task.CompletedTask;
        }
    }
}
