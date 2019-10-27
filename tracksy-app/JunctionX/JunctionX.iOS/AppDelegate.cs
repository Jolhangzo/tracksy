using System.Diagnostics;
using Foundation;
using UIKit;
using Google.Maps;
using JunctionX.Services;
using BranchXamarinSDK;
using System;
using Plugin.Settings.Abstractions;
using MvvmCross.Localization;
using System.Threading.Tasks;
using MvvmCross.Platforms.Ios.Core;
using MvvmCross;
using MvvmCross.Plugin.Messenger;
using static JunctionX.ViewModels.Main.MainViewModel;

namespace JunctionX.iOS
{
    [Register("AppDelegate")]
	public class AppDelegate : MvxApplicationDelegate<Setup, App>
    {
		public override UIWindow Window { get; set; }
		private const string MapApiKey = "AIzaSyAUjCLDWeQwBJX5xIWFf8IAKBq87y6Tqx0";
        public Lazy<IMvxLanguageBinder> LocalizedTextSource = new Lazy<IMvxLanguageBinder>(() => new MvxLanguageBinder());
        // Saved shortcut item used as a result of an app launch, used later when app is activated.
        private UIApplicationShortcutItem launchedShortcutItem;

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			base.FinishedLaunching(application, launchOptions);
			MapServices.ProvideAPIKey(MapApiKey);

            // Override point for customization after application launch.
            var shouldPerformAdditionalDelegateHandling = true;
            // If a shortcut was launched, display its information and take the appropriate action
            if (launchOptions != null)
            {
                var foo = Firebase.Core.Configuration.SharedInstance;
                var shortcutItem = launchOptions[UIApplication.LaunchOptionsShortcutItemKey] as UIApplicationShortcutItem;
                if (shortcutItem != null)
                {
                    launchedShortcutItem = shortcutItem;

                    // This will block "performActionForShortcutItem:completionHandler" from being called.
                    shouldPerformAdditionalDelegateHandling = false;
                }
            }
            try
            {
                Mvx.IoCProvider.CallbackWhenRegistered<ILocationService>(() =>
                {
                    var ls = Mvx.IoCProvider.Resolve<ILocationService>();
                    ls.Start();
                });
            }

#pragma warning disable CS0168 // Variable is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // Variable is declared but never used
            {
                Debugger.Break();
            }

            var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(
                UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null
            );
            application.RegisterUserNotificationSettings(notificationSettings);
            return shouldPerformAdditionalDelegateHandling;
		}

		public override async void OnActivated(UIApplication application)
		{
			if (launchedShortcutItem != null)
			{
    			try
    			{
    				await HandleShortCutItem(launchedShortcutItem);
    			}
				catch {}
    			finally
    			{
    				launchedShortcutItem = null;
    			}
    		}
        }

		// Called when the user activates your application by selecting a shortcut on the home screen, except when 
		// WillFinishLaunching (UIApplication, NSDictionary) or FinishedLaunching (UIApplication, NSDictionary) returns `false`.
		// You should handle the shortcut in those callbacks and return `false` if possible. In that case, this 
		// callback is used if your application is already launched in the background.
		public override async void PerformActionForShortcutItem(UIApplication application, UIApplicationShortcutItem shortcutItem, UIOperationHandler completionHandler)
        {
			try
            {
				completionHandler(await HandleShortCutItem(shortcutItem));
            }
            catch
            {

            }
            completionHandler(false);
		}

        private async Task<bool> HandleShortCutItem(UIApplicationShortcutItem shortcutItem)
        {
            // Verify that the provided `shortcutItem`'s `type` is one handled by the application.
            string shortcutIdentifier = shortcutItem.Type;
            if (shortcutIdentifier == null)
                return false;
			return false;
        }

        private static UIApplicationShortcutIcon CreateIcon(UIApplicationShortcutIconType type)
        {
            return UIApplicationShortcutIcon.FromType(type);
		}
        
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken) {
            var dt = deviceToken.Description.Replace("<", "").Replace(">", "").Replace(" ", "");
            if (!string.IsNullOrEmpty(dt)) {
                var settings = Mvx.IoCProvider.Resolve<ISettings>();
                var messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();
				messenger.Publish(new NotificationTokenChangedMessage(this));
            }
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            new UIAlertView("Error registering push notifications", error.LocalizedDescription, null, "OK", null).Show();
        }

		[Export("application:didReceiveRemoteNotification:fetchCompletionHandler:")]
        public override async void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
		{
            BranchIOS.getInstance().HandlePushNotification(userInfo);
			try
            {
				var aps = userInfo.ObjectForKey(new NSString("aps")) as NSDictionary;
                var serializedData = userInfo.ObjectForKey(new NSString("data")) as NSString;
            }
            catch
            {
            }
			finally
			{
                completionHandler(UIBackgroundFetchResult.NoData);
			}
		}

        // Called when there is an error initializing Branch
        public void SessionRequestError(BranchError error)
        {
            Console.WriteLine("Branch error: " + error.ErrorCode);
            Console.WriteLine(error.ErrorMessage);
        }

        public override void DidEnterBackground(UIApplication application)
        {
            Mvx.IoCProvider.CallbackWhenRegistered<ILocationService>(() =>
            {
                Mvx.IoCProvider.Resolve<ILocationService>().Stop();
            });
        }

        public override void WillEnterForeground(UIApplication application)
        {
        }
    }
}
