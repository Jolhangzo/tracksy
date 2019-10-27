using System;
using JunctionX.ViewModels.Main;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace JunctionX.iOS.Storyboards.Main
{
	public partial class NewsDetailsView : MvxViewController<NewsDetailsViewModel>
	{
		public NewsDetailsView (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var set = this.CreateBindingSet<NewsDetailsView, NewsDetailsViewModel>();
            set.Bind(CoverImageView).For("ImagePath").To(vm => vm.Article.ImageUrl);
            set.Bind(TitleLabel).To(vm => vm.Article.Title);
            set.Bind(DescriptionLabel).To(vm => vm.Article.Description);
            set.Apply();
            DateLabel.Text =
                ViewModel.Article.DateTime.ToString() + " " +
                "by " + ViewModel.Article.Category;
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NavigationItem.Title = "News Details";
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }


        #region View Setup

        #endregion
    }
}
