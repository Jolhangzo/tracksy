using System;
using JunctionX.Models;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace JunctionX.ViewModels.Main
{
    public class NewsDetailsViewModel: MvxNavigationViewModel<Article>
    {
        public Article Article { get; set; }

        public NewsDetailsViewModel(
           IMvxNavigationService navigationService, IMvxLogProvider logProvider) : base(logProvider, navigationService)
        {
        }


        public override void Prepare(Article article)
        {
            Article = article;
        }
    }
}
