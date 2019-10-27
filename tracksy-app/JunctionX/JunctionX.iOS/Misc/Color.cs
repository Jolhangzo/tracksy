using System;
using UIKit;

namespace JunctionX.iOS.Misc
{
    public static class CustomColor
    {
        public static readonly UIColor JunctionXGreen = UIColorFromHexa("66f246");
        public static readonly UIColor JunctionXGray = UIColorFromHexa("9B9B9B");
        public static readonly UIColor JunctionXBlue = UIColorFromHexa("007aff");
        public static readonly UIColor tabBarBackround = UIColorFromHexa("E8E8E8");

        public static UIColor UIColorFromHexa(this string hexString)
        {
            if (hexString.Length == 3)
                hexString = hexString + hexString;

            if (hexString.Length != 6)
                throw new Exception("Invalid hex string.");

            var red = int.Parse(hexString.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            var green = int.Parse(hexString.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            var blue = int.Parse(hexString.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier);

            return UIColor.FromRGB(red, green, blue);
        }
    }
}

