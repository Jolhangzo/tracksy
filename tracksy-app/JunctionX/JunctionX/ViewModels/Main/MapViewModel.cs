using System;
using System.Threading.Tasks;
using MvvmCross.Localization;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Plugin.FieldBinding;
using MvvmCross.Plugin.Location;
using MvvmCross.ViewModels;
using JunctionX.Services;
using JunctionX.Models;
using Refit;

namespace JunctionX.ViewModels.Main
{
    public class MapViewModel : MvxNavigationViewModel, IMvxLocalizedTextSourceOwner
    {
        private readonly ILocationService locationService;
        private readonly IPermissionService permissionService;
        private readonly IBackend backend;
        private bool animalLoaded;
        public INC<Animal[]> Items = new NC<Animal[]>();

        public MvxCoordinates CurrentLocation =>
            locationService.IsStarted ? locationService.CurrentOrLastSeenCoordinates : null;

        public INC<bool> IsLocationPermissionGranted = new NC<bool>();
        public IMvxLanguageBinder LocalizedTextSource { get; } = new MvxLanguageBinder();
        public INC<int> SelectedShopId = new NC<int>();

        public MapViewModel(
            ILocationService locationService,
            IMvxNavigationService navigationService,
            IMvxLogProvider logProvider,
            IPermissionService permissionService,
            IBackend backend) : base(logProvider, navigationService)
        {
            this.locationService = locationService;
            this.permissionService = permissionService;
            this.backend = backend;
        }

        public override Task Initialize()
        {
            Task.Run(async () => {
                while (true)
                {
                    await LoadAnimals();
                    await Task.Delay(1000);
                }
            });
            return base.Initialize();
        }

        public override async void ViewAppeared()
        {
            base.ViewAppeared();
            IsLocationPermissionGranted.Value = await permissionService.IsLocationPermissionGranted();
            if (!IsLocationPermissionGranted.Value)
                await permissionService.RequestLocationPermission();
            else
            {
                locationService.Start();
            }

            await LoadAnimals();
            //MockMarkers();
        }

        private async Task LoadAnimals()
        {
            try
            {
                Items.Value = await backend.GetAnimals();
                if (Items.Value != null && !animalLoaded)
                {
                    foreach (var item in Items.Value)
                    {
                        if (item.Owner == "JunctionX")
                            MyAnimalsViewModel.AddAnimal(item);
                    }
                    animalLoaded = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async void OpenAnimalActivity(Animal animal)
        {
            await NavigationService.Navigate<MyAnimalActivityViewModel, Animal>(animal);
        }

        public async void OpenDetailActivity(Animal animal)
        {
            await NavigationService.Navigate<AnimalDetailsViewModel, Animal>(animal);
        }


        private void MockMarkers()
        {
            //Items.Value = new Animal[]
            //{
            //    new Animal() {Coordinate = new Coordinate(47.475202f, 19.097626f), Name = "Csabi", YearCount = "2 years old", Type= "Desert Eagle", AnimalId = 2, Location="Nockamixon State Park", ImageUrl="bird1", Owner = "Kate",
            //    Coordinates = new Coordinate[]{
            //        new Coordinate(47.475202f, 19.097626f),
            //         new Coordinate(47.477044f, 19.097916f),
            //         new Coordinate(47.476021f, 19.095770f),
            //         new Coordinate(47.473962f, 19.095716f),
            //         new Coordinate(47.476599f, 19.101214f),
            //         new Coordinate(47.492957f, 19.097871f),
            //         new Coordinate(47.489965f, 19.118778f)
            //    } },
            //    new Animal() {Coordinate =  new Coordinate(47.477044f, 19.097916f), Name = "PÁKÓ", YearCount = "3 years old", Type= "SAS BAzzeg Eagle", AnimalId = 11, Location="New xor State Park", ImageUrl="bird2", Owner = "Google",
            //     Coordinates = new Coordinate[]{
            //        new Coordinate(47.475202f, 19.097626f),
            //         new Coordinate(47.477044f, 19.097916f),
            //         new Coordinate(47.476021f, 19.095770f),
            //         new Coordinate(47.473962f, 19.095716f),
            //         new Coordinate(47.476599f, 19.101214f),
            //         new Coordinate(47.492957f, 19.097871f),
            //         new Coordinate(47.489965f, 19.118778f)
            //    } },
            //    new Animal() {Coordinate = new Coordinate(47.476021f, 19.095770f), Name = "Leo", YearCount = "1 years old", Type= "Harpia Eagle", AnimalId = 16, Location="GOOGLe State Park", ImageUrl="bird3", Owner = null,
            //     Coordinates = new Coordinate[]{
            //        new Coordinate(47.475202f, 19.097626f),
            //         new Coordinate(47.477044f, 19.097916f),
            //         new Coordinate(47.476021f, 19.095770f),
            //         new Coordinate(47.473962f, 19.095716f),
            //         new Coordinate(47.476599f, 19.101214f),
            //         new Coordinate(47.492957f, 19.097871f),
            //         new Coordinate(47.489965f, 19.118778f)
            //    } },
            //    new Animal() {Coordinate = new Coordinate(47.473962f, 19.095716f), Name = "Kislány", YearCount = "1 years old", Type= "kárytavár Eagle", AnimalId = 96, Location="London State Park", ImageUrl="bird2", Owner = "Levi", Coordinates = new Coordinate[]{
            //        new Coordinate(47.475202f, 19.097626f),
            //         new Coordinate(47.477044f, 19.097916f),
            //         new Coordinate(47.476021f, 19.095770f),
            //         new Coordinate(47.473962f, 19.095716f),
            //         new Coordinate(47.476599f, 19.101214f),
            //         new Coordinate(47.492957f, 19.097871f),
            //         new Coordinate(47.489965f, 19.118778f)
            //    } },
            //    new Animal() {Coordinate = new Coordinate(47.476599f, 19.101214f), Name = "Páfrány", YearCount = "5 years old", Type= "Desert Eagle", AnimalId = 13, Location="Lisabon State Park", ImageUrl="bird3", Owner = "", Coordinates = new Coordinate[]{
            //        new Coordinate(47.475202f, 19.097626f),
            //         new Coordinate(47.477044f, 19.097916f),
            //         new Coordinate(47.476021f, 19.095770f),
            //         new Coordinate(47.473962f, 19.095716f),
            //         new Coordinate(47.476599f, 19.101214f),
            //         new Coordinate(47.492957f, 19.097871f),
            //         new Coordinate(47.489965f, 19.118778f)
            //    } },
            //    new Animal() {Coordinate = new Coordinate(47.492957f, 19.097871f), Name = "KUKI", YearCount = "2 years old", Type= "Desert Eagle", AnimalId = 14, Location="úbuda State Park", ImageUrl="bird1", Owner = "Matyi", Coordinates = new Coordinate[]{
            //        new Coordinate(47.475202f, 19.097626f),
            //         new Coordinate(47.477044f, 19.097916f),
            //         new Coordinate(47.476021f, 19.095770f),
            //         new Coordinate(47.473962f, 19.095716f),
            //         new Coordinate(47.476599f, 19.101214f),
            //         new Coordinate(47.492957f, 19.097871f),
            //         new Coordinate(47.489965f, 19.118778f)
            //    } },
            //    new Animal() {Coordinate = new Coordinate(47.489965f, 19.118778f), Name = "SAAS", YearCount = "2 years old", Type= "Desert Eagle", AnimalId = 15, Location="budürs State Park", ImageUrl="bird1", Owner = "Bence", Coordinates = new Coordinate[]{
            //        new Coordinate(47.475202f, 19.097626f),
            //         new Coordinate(47.477044f, 19.097916f),
            //         new Coordinate(47.476021f, 19.095770f),
            //         new Coordinate(47.473962f, 19.095716f),
            //         new Coordinate(47.476599f, 19.101214f),
            //         new Coordinate(47.492957f, 19.097871f),
            //         new Coordinate(47.489965f, 19.118778f)
            //    } },
            //};
        }
    }
}