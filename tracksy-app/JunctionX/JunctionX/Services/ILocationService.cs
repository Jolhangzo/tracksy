using MvvmCross.Plugin.Location;

namespace JunctionX.Services
{
    public interface ILocationService
    {
        MvxCoordinates CurrentOrLastSeenCoordinates { get; }
        MvxCoordinates CurrentLocation { get; }
        MvxCoordinates LastSeenLocation { get; }
        bool IsStarted { get; }
        void Start(bool fine = false);
        void Stop();
    }
}