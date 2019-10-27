using System.Collections.Generic;
using MvvmCross.ViewModels;

namespace JunctionX.Misc
{
	public class Presenter
	{
		public const string NavigationModeKey = "NavigationMode";
		public const string ClearStackKey = "ClearStack";
		public const string ClearTopKey = "ClearTop";
		public const string ReplaceKey = "Replace";

		public static readonly IMvxBundle ClearStackPresentationBundle =
			new MvxBundle(new Dictionary<string, string> { { NavigationModeKey, ClearStackKey } });
		public static readonly IMvxBundle ClearTopPresentationBundle =
			new MvxBundle(new Dictionary<string, string> { { NavigationModeKey, ClearTopKey } });
		public static readonly IMvxBundle ReplacePresentationBundle =
			new MvxBundle(new Dictionary<string, string> { { NavigationModeKey, ReplaceKey } });
	}
}
