using System;
using MvvmCross.Plugin.Location;

namespace JunctionX.Services
{
    public class LocationService : ILocationService
    {
        private readonly IMvxLocationWatcher watcher;
        private bool isStarted;
        public bool IsStarted => isStarted;

        public MvxCoordinates CurrentOrLastSeenCoordinates => watcher.CurrentLocation?.Coordinates ?? watcher?.LastSeenLocation?.Coordinates;

        public MvxCoordinates CurrentLocation => watcher.CurrentLocation?.Coordinates;

        public MvxCoordinates LastSeenLocation => watcher.LastSeenLocation?.Coordinates;

        public LocationService(IMvxLocationWatcher watcher)
        {
            this.watcher = watcher;
        }

        public void Start(bool fine = false)
        {
            if (isStarted)
                return;
            isStarted = true;
            if(fine)
                watcher.Start(new MvxLocationOptions
                {
                    Accuracy = MvxLocationAccuracy.Fine,
                    TrackingMode = MvxLocationTrackingMode.Foreground,
                    TimeBetweenUpdates = TimeSpan.FromSeconds(5),
                    MovementThresholdInM = 0
                }, OnLocation, OnError);
            else
                watcher.Start(new MvxLocationOptions
                {
                    Accuracy = MvxLocationAccuracy.Coarse,
                    TrackingMode = MvxLocationTrackingMode.Foreground,
                    TimeBetweenUpdates = TimeSpan.FromSeconds(30),
                    MovementThresholdInM = 0
                }, OnLocation, OnError);
        }

        public void Stop()
        {
            if (!isStarted)
                return;
            isStarted = false;
            watcher.Stop();
        }

        private void OnLocation(MvxGeoLocation location)
        {
            //var message = new LocationMessage(this, location.Coordinates);
            //messenger.Publish(message);
        }

        private void OnError(MvxLocationError error)
        {
        }
    }
}
