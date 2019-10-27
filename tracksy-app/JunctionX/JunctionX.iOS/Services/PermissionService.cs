
using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CoreLocation;
using JunctionX.Services;
using UIKit;
using UserNotifications;

namespace JunctionX.iOS.Services
{
    public class PermissionService : IPermissionService
	{
        private CLLocationManager locationManager;
        private IUserDialogs userDialogs;
        private TaskCompletionSource<bool> requestNotificationPermissionCompletitionSource;

        public Task<bool> IsLocationPermissionGranted()
        {
            return Task.FromResult(CLLocationManager.LocationServicesEnabled && (CLLocationManager.Status == CLAuthorizationStatus.AuthorizedAlways ||
                                    CLLocationManager.Status == CLAuthorizationStatus.AuthorizedWhenInUse));
		}

        public Task<bool> IsNotificationPermissionGranted()
        {
			var tcs = new TaskCompletionSource<bool>();
            UNUserNotificationCenter.Current.GetNotificationSettings(ns => {
                tcs.SetResult(ns.AuthorizationStatus == UNAuthorizationStatus.Authorized);
            });
            return tcs.Task;
		}

	    public Task<bool> RequestNotificationPermission()
        {
            requestNotificationPermissionCompletitionSource = new TaskCompletionSource<bool>();
            UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound | UNAuthorizationOptions.Badge,
              (granted, error) => {
                  if (granted)
                      UIApplication.SharedApplication.InvokeOnMainThread(() =>
                      {
                          UIApplication.SharedApplication.RegisterForRemoteNotifications();
                      });
                  requestNotificationPermissionCompletitionSource.SetResult(granted);
              });
            return requestNotificationPermissionCompletitionSource.Task;
        }

	    public Task RequestLocationPermission()
	    {
            if (CLLocationManager.LocationServicesEnabled)
            {
                locationManager = new CLLocationManager();
                locationManager.AuthorizationChanged += LocationManagerOnAuthorizationChanged;
                locationManager.RequestWhenInUseAuthorization();
            }

            return Task.CompletedTask;
	    }

	    public Task<bool> IsRegisteredForRemoteNotifications()
	    {
	        return Task.FromResult(UIApplication.SharedApplication.IsRegisteredForRemoteNotifications);
	    }

	    public Task OpenAppSettings()
	    {
	        UIApplication.SharedApplication.OpenUrl(new Foundation.NSUrl(UIApplication.OpenSettingsUrlString));
	        return Task.CompletedTask;
	    }

	    private async void LocationManagerOnAuthorizationChanged(object sender, CLAuthorizationChangedEventArgs clAuthorizationChangedEventArgs)
        {
            
            switch (clAuthorizationChangedEventArgs.Status)
            {
                case CLAuthorizationStatus.Denied:
                    UIApplication.SharedApplication.OpenUrl(new Foundation.NSUrl(UIApplication.OpenSettingsUrlString));
                    break;
                case CLAuthorizationStatus.NotDetermined:
                    locationManager.RequestWhenInUseAuthorization();
                    break;
                default:
                    break;
            }
        }
    }
}