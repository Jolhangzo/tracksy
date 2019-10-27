using System;
using CoreLocation;
using Foundation;
using GMCluster;

namespace JunctionX.iOS.Misc
{
    public class AnimalClusterItem : NSObject, IGMUClusterItem
    {
        public CLLocationCoordinate2D Position { get; set; }
        public int AnimalId { get; set; }
        public string Name { get; }
        public string ImageName { get; }

        public AnimalClusterItem(CLLocationCoordinate2D position, int animalId, string title, string imageName)
        {
            AnimalId = animalId;
            Position = position;
            Name = title;
            ImageName = imageName;
        }


    }

}
