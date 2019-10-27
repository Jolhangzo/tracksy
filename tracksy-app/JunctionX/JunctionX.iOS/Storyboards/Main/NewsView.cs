using System;
using MvvmCross.Binding.BindingContext;
using JunctionX.ViewModels.Main;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using UIKit;
using MvvmCross.Platforms.Ios.Binding.Views;
using JunctionX.iOS.Storyboards.Main.Cells;
using Foundation;

namespace JunctionX.iOS.Storyboards.Main
{
   [MvxTabPresentation(WrapInNavigationController = true, TabName = "news", TabIconName = "news")]
    public partial class NewsView: MvxViewController<NewsViewModel>
    {

        public NewsView(IntPtr handle) : base(handle)
        {
        }

        #region VC life cycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var source = new MvxSimpleTableViewSource(News, NewsCell.Key);
            var set = this.CreateBindingSet<NewsView, NewsViewModel>();
            set.Bind(source).To("Articles");
            set.Bind(source).For(v=> v.SelectionChangedCommand).To("OpenNewsDetails");
            set.Apply();
            News.Source = source;
            News.TableFooterView = new UIView(new CoreGraphics.CGRect(0, 0, 0, 9f));
            UILocalNotification notification = new UILocalNotification();
            notification.FireDate = NSDate.FromTimeIntervalSinceNow(15);
            //notification.AlertTitle = "Alert Title"; // required for Apple Watch notifications
            notification.AlertAction = "View Alert";
            notification.AlertTitle = "Happy birthday" + MyAnimalsViewModel.GetAnimalName() + "!";
            notification.AlertBody = MyAnimalsViewModel.GetAnimalName() + " just got 2 years old. Check out his activity now!";

            UILocalNotification notification2 = new UILocalNotification();
            notification2.FireDate = NSDate.FromTimeIntervalSinceNow(30);
            //notification.AlertTitle = "Alert Title"; // required for Apple Watch notifications
            notification2.AlertAction = "View Alert";
            notification2.AlertTitle = MyAnimalsViewModel.GetAnimalName() + " flown 6 miles today.";
            notification2.AlertBody = "How many miles did you walk today?";
            UIApplication.SharedApplication.ScheduleLocalNotification(notification);
            UIApplication.SharedApplication.ScheduleLocalNotification(notification2);
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            NavigationController.NavigationBar.TopItem.Title = "News";
            var textAttributes = new UIStringAttributes();
            textAttributes.ForegroundColor = UIColor.White;
            NavigationController.NavigationBar.TitleTextAttributes = textAttributes;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NavigationController.NavigationBar.TopItem.Title = "News";
            var textAttributes = new UIStringAttributes();
            textAttributes.ForegroundColor = UIColor.White;
            NavigationController.NavigationBar.TitleTextAttributes = textAttributes;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        #endregion

        #region View Setup

        #endregion


    }
}

