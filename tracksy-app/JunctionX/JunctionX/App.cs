using System;
using System.Net.Http;
using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Localization;
using MvvmCross.Plugin.ResxLocalization;
using MvvmCross.ViewModels;
using Plugin.Settings;
using JunctionX.Resources;
using JunctionX.Services;
using Refit;

namespace JunctionX
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            try
            {
                CreatableTypes()
                    .EndingWith("Service")
                    .AsInterfaces()
                    .RegisterAsLazySingleton();

                Mvx.IoCProvider.RegisterSingleton(() => UserDialogs.Instance);
                Mvx.IoCProvider.RegisterSingleton<IMvxTextProvider>(new MvxResxTextProvider(Strings.ResourceManager));
                Mvx.IoCProvider.RegisterSingleton(CrossSettings.Current);
                Mvx.IoCProvider.RegisterSingleton(() => RestService.For<IBackend>(new HttpClient
                {
                    BaseAddress = new Uri("https://us-central1-tracksynew.cloudfunctions.net")
                }));
                RegisterCustomAppStart<AppStart>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}