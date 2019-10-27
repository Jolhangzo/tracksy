using Acr.UserDialogs;
using MvvmCross.Localization;
using System;
using System.Threading.Tasks;
using JunctionX.Services;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

using MvvmCross.Logging;
using MvvmCross.Plugin.FieldBinding;
using JunctionX.Models;

namespace JunctionX.ViewModels.Main
{
    public class NewsViewModel : MvxNavigationViewModel, IMvxLocalizedTextSourceOwner
    {
        private readonly IBackend backend;
        private readonly IUserDialogs userDialogs;
        public IMvxLanguageBinder LocalizedTextSource { get; } = new MvxLanguageBinder();
        public INC<Article[]> Articles = new NC<Article[]>();
        public NewsViewModel(IMvxLogProvider logProvider,
            IMvxNavigationService navigationService, IBackend backend) : base(logProvider, navigationService)
        {
            this.backend = backend;
        }

        public override async void ViewAppeared()
        {
            base.ViewAppeared();
            await LoadArticles();
        }

        private async Task LoadArticles()
        {
            try
            {
                Articles.Value = await backend.GetArticles();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async void OpenNewsDetails(Article article)
        {
            await NavigationService.Navigate<NewsDetailsViewModel, Article>(article);
        }
        //public async void OpenProfile()
        //{
        //    await NavigationService.Navigate<ProfileViewModel>();
        //}

        //public async void Logout()
        //{
        //    try
        //    {
        //        using (userDialogs.Loading(LocalizedTextSource.GetText("loading")))
        //            await authenticationService.Logout();
        //    }
        //    catch (Exception e)
        //    {
        //        var x = e;
        //    }
        //}
    }
}