using MvvmCross.Exceptions;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using JunctionX.ViewModels.Main;
using System;
using System.Threading.Tasks;

namespace JunctionX
{
    public class AppStart : MvxAppStart
    {
        private readonly IMvxNavigationService navigationService;

        public AppStart(IMvxApplication application,
            IMvxNavigationService navigationService) : base(application,
            navigationService)
        {
            this.navigationService = navigationService;
        }

        protected override async Task NavigateToFirstViewModel(object hint = null)
        {
            try
            {
                await navigationService.Navigate<MainViewModel>();
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap("Problem in App Startup");
            }
        }
    }
}