using System;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using JunctionX.Models;
using JunctionX.Services;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Plugin.FieldBinding;
using MvvmCross.ViewModels;

namespace JunctionX.ViewModels.Main
{
    public class MyAnimalActivityViewModel: MvxNavigationViewModel<Animal>
    {
        private readonly IBackend backend;
        private IUserDialogs userDialogs;
        public Animal Animal { get; set; }
        public INC<Log[]> Logs = new NC<Log[]>();

        public MyAnimalActivityViewModel(IMvxNavigationService navigationService,
            IMvxLogProvider logProvider, IUserDialogs userDialogs,
            IBackend backend) : base(logProvider, navigationService)
        {
            this.backend = backend;
            this.userDialogs = userDialogs;
        }

        public override void Prepare(Animal animal)
        {
            Animal = animal;
        }

        public override async void ViewAppeared()
        {
            base.ViewAppeared();
            await LoadLogs();
        }

        private async Task LoadLogs()
        {
            var loading = userDialogs.Loading();
            try
            {
                Logs.Value = (await backend.GetLogs(Animal.AnimalId)).OrderBy(x => x.DateTimeValue).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                loading.Dispose();
            }

        }

        public async void Back()
        {
            await NavigationService.Close(this);
        }

       

    }
}
