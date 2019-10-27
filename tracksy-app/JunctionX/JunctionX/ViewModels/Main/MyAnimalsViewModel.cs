using Acr.UserDialogs;
using MvvmCross.Navigation;
using MvvmCross.Plugin.FieldBinding;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using Plugin.Settings.Abstractions;
using JunctionX.Services;
using MvvmCross.Logging;
using JunctionX.Models;
using System.Collections.Generic;
using System.Linq;

namespace JunctionX.ViewModels.Main
{
    public class MyAnimalsViewModel : MvxNavigationViewModel
    {
        private readonly IPermissionService permissionService;
        private readonly IMvxMessenger messenger;
        private readonly ILocationService locationService;

        public INC<bool> IsLocationPermissionGranted = new NC<bool>(true);
        public static List<Animal> StaticItems = new List<Animal>();
        public INC<Animal[]> Items = new NC<Animal[]>();

        public MyAnimalsViewModel(
            IPermissionService permissionService, ILocationService locationService,
            IUserDialogs userDialogs, IMvxNavigationService navigationService, IMvxLogProvider logProvider,
            ISettings settings, IMvxMessenger messenger, IShareService shareService) : base(logProvider, navigationService)
        {
            this.permissionService = permissionService;
            this.locationService = locationService;
            this.messenger = messenger;
        }

        public static void AddAnimal(Animal animal)
        {
            StaticItems.Add(animal);
        }

        public static string GetAnimalName()
        {
            return StaticItems[0].Name;
        }

        public override async void ViewAppearing()
        {
            base.ViewAppearing();
            //AddAnimal(
            //        new Animal() { Coordinate = new Coordinate(47.475202f, 19.097626f), Name = "Csabi", Type = "Desert Eagle", AnimalId = 2, Location = "Nockamixon State Park", ImageUrl = "bird1", Owner = "" }
            //    );
            if (StaticItems == null)
            {
                Items.Value = new[]
                {
                    new Animal() { Coordinate = new Coordinate(47.475202f, 19.097626f), Name = "Csabi", Type = "Desert Eagle", AnimalId = 2, Location = "Nockamixon State Park", ImageUrl = "bird1", Owner = "Kate" }
                };
                StaticItems = Items.Value.ToList();
            }
            else
                Items.Value = StaticItems.ToArray();
        }

        public async void RequestLocationPermission()
        {
            IsLocationPermissionGranted.Value = await permissionService.IsLocationPermissionGranted();
            await permissionService.RequestLocationPermission();
            IsLocationPermissionGranted.Value = await permissionService.IsLocationPermissionGranted();
            await permissionService.RequestLocationPermission();
        }

        public void Back()
        {
            messenger.Publish(new MainViewModel.CloseMainViewModelMessage(this));
        }

        public async void OpenAnimalDetails(Animal animal)
        {
            await NavigationService.Navigate<AnimalDetailsViewModel, Animal>(animal);
        }
    }
}