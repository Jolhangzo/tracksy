using System;
using System.Linq;
using MvvmCross.Exceptions;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.ViewModels;
using UIKit;

namespace JunctionX.iOS.Misc
{
    public class ViewsContainer : MvxIosViewsContainer
    {
        public override IMvxIosView CreateViewOfType(Type viewType, MvxViewModelRequest request)
        {
            var viewModelName = request.ViewModelType.Namespace.Split('.').Last();
            try
            {
                return (IMvxIosView)UIStoryboard.FromName(viewModelName, null).InstantiateViewController(viewType.Name);
            }
            catch (Exception ex)
            {
                throw new MvxException("Loading view of type {0} from storyboard {1} failed: {2}", viewType.Name, viewModelName, ex.Message);
            }
        }
    }
}