using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Localization;
using MvvmCross.Navigation;
using MvvmCross.Plugin.FieldBinding;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using Plugin.DeviceInfo;
using Plugin.DeviceInfo.Abstractions;
using Plugin.Settings.Abstractions;
using JunctionX.Services;

namespace JunctionX.ViewModels.Main
{
    public class MainViewModel : MvxViewModel, IMvxLocalizedTextSourceOwner
    {
        private readonly IMvxNavigationService navigationService;
        private readonly IPermissionService permissionService;
        private readonly IMvxMessenger messenger;
        private readonly ISettings settings;
        private readonly ILocationService locationService;
        private readonly List<MvxSubscriptionToken> messengerTokens = new List<MvxSubscriptionToken>();

        private bool tabsOpened;

        public IMvxLanguageBinder LocalizedTextSource { get; } = new MvxLanguageBinder();


        public INC<bool> IsOrderDrawerOpen = new NC<bool>();
        public INC<bool> IsFilterCategoriesDrawerOpen = new NC<bool>();
        public INC<int> UnreadNotificationsCount = new NC<int>();

        public MainViewModel(IMvxNavigationService navigationService, IPermissionService permissionService,
            IMvxMessenger messenger, ISettings settings,
            ILocationService locationService)
        {
            this.navigationService = navigationService;
            this.permissionService = permissionService;
            this.messenger = messenger;
            this.settings = settings;
            this.locationService = locationService;
        }

        public override async void ViewAppearing()
        {
            base.ViewAppearing();

            if (!tabsOpened)
            {
                tabsOpened = true;
                await OpenTabs();
            }

            if (CrossDeviceInfo.Current.Platform != Platform.Android &&
                await permissionService.IsLocationPermissionGranted())
                locationService.Start();
        }

        public override async void ViewAppeared()
        {
            base.ViewAppeared();
        }

        public class MainViewModelAppearedMessage : MvxMessage
        {
            public MainViewModelAppearedMessage(object sender) : base(sender)
            {
            }
        }

        private async Task OpenTabs()
        {
            var tasks = new List<Task>
            {
                navigationService.Navigate<MapViewModel>(),
                navigationService.Navigate<MyAnimalsViewModel>(),
                navigationService.Navigate<NewsViewModel>()
            };
            await Task.WhenAll(tasks);
        }

        public override void ViewCreated()
        {
            messengerTokens.Add(messenger.Subscribe<CloseMainViewModelMessage>(OnCloseMainViewModelMessage));
            base.ViewCreated();
        }

        private async void OnCloseMainViewModelMessage(CloseMainViewModelMessage obj)
        {
            await navigationService.Close(this);
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            foreach (var token in messengerTokens)
                token.Dispose();
            base.ViewDestroy(viewFinishing);
        }

        public class CloseMainViewModelMessage : MvxMessage
        {
            public CloseMainViewModelMessage(object sender) : base(sender)
            {
            }
        }

        public class NotificationTokenChangedMessage : MvxMessage
        {
            public NotificationTokenChangedMessage(object sender) : base(sender)
            {
            }
        }
    }
}