using System;
using JunctionX.Models;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace JunctionX.ViewModels.Main
{
    public class AnimalDetailsViewModel: MvxNavigationViewModel<Animal>
    {
        public Animal Animal { get; set; }

        public AnimalDetailsViewModel(
            IMvxNavigationService navigationService,
            IMvxLogProvider logProvider) : base(logProvider, navigationService)
        {
        }

        public override void Prepare(Animal animal)
        {
            Animal = animal;
        }

        public async void OpenAnimalActivity(Animal animal)
        {
            await NavigationService.Navigate<MyAnimalActivityViewModel, Animal>(animal);
        }
    }
}
