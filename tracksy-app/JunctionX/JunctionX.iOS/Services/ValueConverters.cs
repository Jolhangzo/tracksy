using System;
using MvvmCross.Converters;
using JunctionX.iOS.Misc;
using UIKit;

namespace JunctionX.iOS.Services
{
    public class UpperCaseValueConverter : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((string)parameter).ToUpper();
        }
    }
	public class RatingValueConverter : MvxValueConverter
	{
		public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
		    int format = (int)parameter;
            return (bool)((double)value >= (double)format) ? false : true;
		}
	}

	public class ContstraintValueConverter : MvxValueConverter
	{
		public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
				return null;

			if (parameter == null)
				return value;
            if ((int)value == 0)
                return 20f;
            if ((int)value == 1)
                return 35f;
            if ((int)value == 2)
                return 50f;
            if ((int)value == 3)
                return 65f;
            return 65f;
		}
	}

    public class ConstraintValueConverter : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value)
                return 280f;
            else
                return 130f;
        }
    }

    public class NoCardsHeightValueConverter : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((int)value > 0)
                return 15f;
            else
                return 302f;
        }
    }

    public class NotificationsBackgroundColorValueConverter : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value)
                return CustomColor.tabBarBackround;
            else
                return UIColor.White;
        }
    }

    public class ProfileVerifyTextValueConverter : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value)
                return "verified";
            else
                return "verify";
        }
    }
}
