using System;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Exceptions;
using MvvmCross.Platforms.Ios.Presenters;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using JunctionX.iOS.Storyboards.Main;
using JunctionX.Misc;
using UIKit;

namespace JunctionX.iOS.Misc
{
    public class IosPresenter : MvxIosViewPresenter
    {
        private MvxNavigationController _nav;
        public IosPresenter(IUIApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        public override Task<bool> Show(MvxViewModelRequest request)
        {
            var attribute = GetPresentationAttribute(request);
            if (request.PresentationValues != null &&
                request.PresentationValues.ContainsKey(Presenter.NavigationModeKey))
            {
                if (attribute.GetType() != typeof(MvxChildPresentationAttribute))
                {
                    if (request.PresentationValues[Presenter.NavigationModeKey] == Presenter.ReplaceKey)
                        MasterNavigationController.PopViewController(false);
                    else if (request.PresentationValues[Presenter.NavigationModeKey] == Presenter.ClearTopKey)
                    {
                        var success = PopViewControllersUntilViewModel(request.ViewModelType);
                        if (success)
                            return Task.FromResult(true);
                    }
                }
            }
            return base.Show(request);
        }

        public override MvxBasePresentationAttribute GetPresentationAttribute(MvxViewModelRequest request)
        {
            if (request.PresentationValues != null &&
                request.PresentationValues.ContainsKey(Presenter.NavigationModeKey))
            {
                if (request.PresentationValues[Presenter.NavigationModeKey] == Presenter.ClearStackKey)
                    return new MvxRootPresentationAttribute() { WrapInNavigationController = true };
            }

            return base.GetPresentationAttribute(request);
        }


        private bool PopViewControllersUntilViewModel(Type viewModelType)
        {
            for (int i = MasterNavigationController.ViewControllers.Length - 1; i >= 0; i--)
            {
                if (MasterNavigationController.ViewControllers[i] is MvxViewController mvc)
                    if (mvc.ViewModel.GetType() == viewModelType)
                    {
                        MasterNavigationController.PopToViewController(mvc, false);
                        return true;
                    }
            }
            return false;
        }

        protected override MvxNavigationController CreateNavigationController(UIViewController viewController)
        {
            _nav = base.CreateNavigationController(viewController);
            _nav.NavigationBarHidden = false;
            _nav.NavigationBar.BarTintColor = CustomColor.JunctionXGreen;
            _nav.NavigationBar.TintColor = UIColor.White;
            _nav.NavigationBar.Translucent = false;
            return _nav;
        }

        protected override Task<bool> ShowChildViewController(
            UIViewController viewController,
            MvxChildPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            if (viewController is IMvxSplitViewController)
                throw new MvxException("A SplitViewController cannot be presented as a child. Consider using Root instead");

            if (ModalViewControllers.Any())
            {
                if (ModalViewControllers.LastOrDefault() is UINavigationController modalNavController)
                {
                    PushViewControllerIntoStack(modalNavController, viewController, attribute);
                    return Task.FromResult(true);
                }
                throw new MvxException($"Trying to show View type: {viewController.GetType().Name} as child, but there is currently a plain modal view presented!");
            }

            if (TabBarViewController != null)
            {
                if (request.PresentationValues != null &&
                    request.PresentationValues.ContainsKey(Presenter.NavigationModeKey))
                {
                    if (request.PresentationValues[Presenter.NavigationModeKey] == Presenter.ReplaceKey)
                    {
                        if (TabBarViewController is MainView mv)
                        {
                            mv.ReplaceTopChildViewModel(viewController);
                            return Task.FromResult(true);
                        }
                    }
                }
                else
                    return Task.FromResult(TabBarViewController.ShowChildView(viewController));
            }

            if (MasterNavigationController != null)
            {
                PushViewControllerIntoStack(MasterNavigationController, viewController, attribute);
                return Task.FromResult(true);
            }

            throw new MvxException($"Trying to show View type: {viewController.GetType().Name} as child, but there is no current stack!");
        }
    }
}