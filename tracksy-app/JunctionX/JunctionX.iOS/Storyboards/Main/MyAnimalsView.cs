using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using JunctionX.ViewModels.Main;
using UIKit;
using MvvmCross.Platforms.Ios.Binding.Views;
using JunctionX.iOS.Storyboards.Main.Cells;

namespace JunctionX.iOS.Storyboards.Main
{
    [MvxTabPresentation(WrapInNavigationController = true, TabName = "my_animals", TabIconName = "my_animals")]
    public partial class MyAnimalsView : MvxViewController<MyAnimalsViewModel>
    {
        public bool IsKeyboardUp;

        public MyAnimalsView(IntPtr handle) : base(handle)
        {
        }

        #region VC life cycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var source = new MvxSimpleTableViewSource(Animals,AnimalCell.Key);
            var set = this.CreateBindingSet<MyAnimalsView, MyAnimalsViewModel>();
            set.Bind(source).To(vm => vm.Items);
            set.Bind(source).For(vm => vm.SelectionChangedCommand).To("OpenAnimalDetails");
            Animals.Source = source;
            set.Apply();
            Animals.TableFooterView = new UIView(new CoreGraphics.CGRect(0, 0, 0, 9f));
            
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            NavigationController.NavigationBar.TopItem.Title = "My Animals";
            var textAttributes = new UIStringAttributes();
            textAttributes.ForegroundColor = UIColor.White;
            NavigationController.NavigationBar.TitleTextAttributes = textAttributes;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NavigationController.NavigationBar.TopItem.Title = "My Animals";
            var textAttributes = new UIStringAttributes();
            textAttributes.ForegroundColor = UIColor.White;
            NavigationController.NavigationBar.TitleTextAttributes = textAttributes;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        #endregion

        #region View Setup

        #endregion


        #region Keyboard Observing

        #endregion
    }
}
