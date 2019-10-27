using System;
using System.Drawing;
using System.Linq;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using JunctionX.iOS.Misc;
using JunctionX.ViewModels.Main;
using UIKit;

namespace JunctionX.iOS.Storyboards.Main
{
    public partial class MainView : MvxTabBarViewController<MainViewModel>
    {
        public static int SelectedTab = 1;

        public MainView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var set = this.CreateBindingSet<MainView, MainViewModel>();
            set.Apply();
            ShouldSelectViewController += MainView_ShouldSelectViewController;
            
            TabBar.BarTintColor = CustomColor.tabBarBackround;
           // NavigationBarHandler.Init(this);
        }

        protected override void SetTitleAndTabBarItem(UIViewController viewController,
            MvxTabPresentationAttribute attribute)
        {
            if (string.IsNullOrEmpty(attribute.TabName))
            //    attribute.TabName = "offers";
            //if (string.IsNullOrEmpty(attribute.TabIconName))
            //    attribute.TabIconName = "offers";
            base.SetTitleAndTabBarItem(viewController, attribute);


            viewController.TabBarItem.SetTitleTextAttributes(new UITextAttributes
            {
                TextColor = CustomColor.JunctionXGray,
                Font = UIFont.FromName("sf_pro_text_regular", 11)
            }, UIControlState.Normal);
            viewController.TabBarItem.SetTitleTextAttributes(new UITextAttributes
            {
                TextColor = CustomColor.JunctionXBlue,
                Font = UIFont.FromName("sf_pro_text_regular", 11)
            }, UIControlState.Selected);
            viewController.TabBarItem.Title = ViewModel.LocalizedTextSource.GetText(attribute.TabName);

            viewController.TabBarItem.Image =
                UIImage.FromBundle(attribute.TabIconName).ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
            viewController.TabBarItem.SelectedImage =
                UIImage.FromBundle(attribute.TabIconName).ImageWithRenderingMode(UIImageRenderingMode.Automatic);
        }

        public override void ShowTabView(UIViewController viewController, MvxTabPresentationAttribute attribute)
        {
            System.Type vcType;

            if (viewController is UINavigationController)
                vcType = viewController.ChildViewControllers[0].GetType();
            else
                vcType = viewController.GetType();

            var existingVc = ChildViewControllers.FirstOrDefault(x =>
            {
                if (x is UINavigationController)
                    return x.ChildViewControllers[0].GetType() == vcType;
                else
                    return x.GetType() == vcType;
            });
            if (existingVc != null && ChildViewControllers != null)
                SelectedIndex = Array.IndexOf(ChildViewControllers, existingVc);
            else
                base.ShowTabView(viewController, attribute);
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            this.NavigationController.SetNavigationBarHidden(true, false);
            if (SelectedViewController != null)
                SelectedViewController.ViewDidLayoutSubviews();
        }

        bool MainView_ShouldSelectViewController(UITabBarController tabBarController, UIViewController viewController)
        {
            if (viewController.ChildViewControllers[0] is MapView)
                SelectedTab = 1;
            else if (viewController.ChildViewControllers[0] is MyAnimalsView)
                SelectedTab = 2;
            else if (viewController.ChildViewControllers[0] is NewsView)
                SelectedTab = 3;
            return true;
        }

        public void ReplaceTopChildViewModel(UIViewController viewController)
        {
            if (SelectedIndex > 5 && (MoreNavigationController?.ViewControllers?.Any() ?? false))
            {
                var lastViewController = (MoreNavigationController.ViewControllers.Last()).GetIMvxIosView();

                if (lastViewController != null)
                {
                    MoreNavigationController.PopViewController(true);
                    return;
                }
            }

            if (SelectedViewController is UINavigationController navController
                && navController.ViewControllers != null)
            {
                if (navController.ViewControllers.Any())
                {
                    var newStack = navController.ViewControllers.Take(navController.ViewControllers.Length - 1)
                                                .Concat(new[] { viewController }).ToArray();
                    navController.SetViewControllers(newStack, true);
                }
                else
                    navController.PushViewController(viewController, true);
            }
        }
    }
}