using JunctionX.iOS.Misc;
using JunctionX.Services;
using JunctionX.iOS.Services;
using MvvmCross.Localization;
using MvvmCross.Platforms.Ios.Core;
using MvvmCross;
using MvvmCross.Platforms.Ios.Presenters;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.Converters;
using MvvmCross.IoC;
using MvvmCross.Binding.BindingContext;

namespace JunctionX.iOS
{
    public class Setup : MvxIosSetup<App>
    {
		protected override void InitializeIoC()
        {
            base.InitializeIoC();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IPermissionService, PermissionService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IShareService, ShareService>();
        }

		protected override IMvxIosViewsContainer CreateIosViewsContainer()
        {
			return new ViewsContainer();
        }

		protected override IMvxIosViewPresenter CreateViewPresenter()
        {
			return new IosPresenter(ApplicationDelegate, Window);
        }

        protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);
            registry.AddOrOverwrite("Language", new MvxLanguageConverter());
        }

		protected override void InitializeLastChance()
		{
			base.InitializeLastChance();
            //Mvx.RegisterType<IMvxBindingContext, MvxBindingContext>();
        }
    }
}
