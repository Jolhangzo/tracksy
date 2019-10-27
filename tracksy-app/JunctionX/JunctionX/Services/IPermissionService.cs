using System;
using System.Threading.Tasks;

namespace JunctionX.Services
{
    public interface IPermissionService
    {
        Task<bool> IsLocationPermissionGranted();
		Task<bool> IsNotificationPermissionGranted();
        Task<bool> IsRegisteredForRemoteNotifications();
		Task<bool> RequestNotificationPermission();
        Task RequestLocationPermission();
        Task OpenAppSettings();
    }
}