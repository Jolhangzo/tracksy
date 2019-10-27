//using System;
//using System.Windows.Input;
//using Foundation;
//using MvvmCross.Platforms.Ios.Binding.Views;
//using JunctionX.iOS.Storyboards.Main.Cells;
//using MvvmCross.Binding.Extensions;
//using JunctionX.Models;
//using UIKit;
//using CoreGraphics;
//using MvvmCross.Base;
//using MvvmCross.Binding.BindingContext;
//using JunctionX.ViewModels.Main;
//using MvvmCross.Platforms.Ios.Views;
//using JunctionX.iOS.Misc;

//namespace JunctionX.iOS.Storyboards.Main.Sources
//{
//    public class DashBoardSource : MvxTableViewSource
//    {
//        public ICommand FetchMoreCommand { get; set; }
//		private bool isCoupon;
//        private UIViewController vc;
//        private static readonly NSString CampaignCellIdentifier = new NSString("CampaignCell");
//        private static readonly NSString AdvertCellIdentifier = new NSString("AdvertCell");
//        private static readonly NSString OnlineShopCellIdentifier = new NSString("OnlineShopCell");
//        private ImpressionTracker impressionTracker;
//        private nfloat lastContentOffset = 0;
//        private nfloat tempContentOffset = 0;
//        private int countUp = 0;
//        public int countSearchChanged = 0;
//        private UITableView tableView;
//        private NSLayoutConstraint nSLayoutConstraint;
//        private nfloat epsilon;
//        protected DashBoardSource(UITableView tableView)
//            : base(tableView)
//        {
           
//        }

//        public DashBoardSource(UITableView tableView, bool isCoupon, UIViewController vc, NSLayoutConstraint nSLayoutConstraint) : base(tableView)
//		{
//            DeselectAutomatically = true;
//            this.vc = vc;
//            this.tableView = tableView;
//            this.nSLayoutConstraint = nSLayoutConstraint;
//            tableView.RegisterNibForCellReuse(UINib.FromName(CampaignCell.Key, NSBundle.MainBundle), CampaignCell.Key);
//            tableView.RegisterNibForCellReuse(UINib.FromName(OnlineShopCell.Key, NSBundle.MainBundle), OnlineShopCell.Key);
//            //tableView.RegisterNibForCellReuse(UINib.FromName(ShopCell.Key, NSBundle.MainBundle), ShopCell.Key);
//            tableView.RegisterNibForCellReuse(UINib.FromName(AdvertCell.Key, NSBundle.MainBundle), AdvertCell.Key);
//            this.isCoupon = isCoupon;
//            if (vc is OffersView || vc is CouponsView)
//            {
//                impressionTracker = new ImpressionTracker(0.5);
//                impressionTracker.OnVisible += wrapper => ((DashboardViewModelBase)((MvxViewController)vc).ViewModel).TrackCamapignView(wrapper.Campaign);
//            }
//        }

//        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
//        {
//            var item = GetItemAt(indexPath);
//            var cell = GetOrCreateCellFor(tableView, indexPath, item);

//            if (cell is IMvxBindable bindable)
//            {
//                var bindingContext = bindable.BindingContext as MvxTaskBasedBindingContext;

//                var isTaskBasedBindingContextAndHasAutomaticDimension = bindingContext != null && tableView.RowHeight == UITableView.AutomaticDimension;

//                // RunSynchronously must be called before DataContext is set
//                if (isTaskBasedBindingContextAndHasAutomaticDimension)
//                    bindingContext.RunSynchronously = true;

//                bindable.DataContext = item;

//                // If AutomaticDimension is used, xib based cells need to re-layout everything after bindings are applied
//                // otherwise the cell height will be wrong
//                if (isTaskBasedBindingContextAndHasAutomaticDimension)
//                    cell.LayoutIfNeeded();
//            }

//            return cell;
//        }

//        public override nfloat EstimatedHeight(UITableView tableView, NSIndexPath indexPath)
//        {
//            return 275f;
//        }

//        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath,
//                                                              object item)
//        {

//            NSString cellIdentifier;
//            UITableViewCell cell;

//            if (item is CampaignWrapper)
//            {
//                cellIdentifier = CampaignCellIdentifier;
//                cell = CampaignCell.Create();
//            }
//            else if (item is AdvertWrapper)
//            {
//                cellIdentifier = AdvertCellIdentifier;
//                cell = AdvertCell.Create();

//            }
//            else if (item is OnlineShopWrapper)
//            {
//                cellIdentifier = OnlineShopCellIdentifier;
//                cell = OnlineShopCell.Create();
//            }
//            else
//            {
//                throw new ArgumentException("Unknown cell of type " + item.GetType().Name);
//            }
//            if (indexPath.Row >= GetItemSourceCount() - 4)
//                FetchMoreCommand?.Execute(null);
//            //cell = TableView.DequeueReusableCell(cellIdentifier, indexPath);
//            //if (item is CampaignWrapper)
//            //    cell = CampaignCell.Create();

//            //var bindable = cell as IMvxDataConsumer;
//            //if (bindable != null)
//            //    bindable.DataContext = item;
//            return cell;
//        }

//        public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
//        {
//            var bindable = cell as IMvxDataConsumer;
//            if (bindable != null)
//                bindable.DataContext = null;
//            bindable.DataContext = GetItemAt(indexPath);

//        }


//        private int GetItemSourceCount()
//        {
//            if (ItemsSource == null)
//                return 0;
//            return ItemsSource.Count();
//        }

//        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
//        {
//            if (GetItemAt(indexPath) is OnlineShopWrapper)
//            if (GetItemAt(indexPath) is OnlineShopWrapper)
//                ((OnlineShopWrapper)GetItemAt(indexPath)).Open();
//            if (GetItemAt(indexPath) is CampaignWrapper)
//                ((CampaignWrapper)GetItemAt(indexPath)).Open();
//        }

//        public override UIView GetViewForHeader(UITableView tableView, nint section)
//        {
//            var v = new UIView(new CGRect(0, 0, 1f, 18f));
//            return v;
//        }

//        [Export("scrollViewDidScroll:")]
//        public override void Scrolled(UIScrollView scrollView)
//        {
//            impressionTracker.OnScrolled(TableView);
//            if (vc is OffersView)
//            {
//                if (((OffersView)vc).ViewModel.IsFetchingMoreOffers.Value || ((OffersView)vc).IsKeyboardUp)
//                {
//                    if (((OffersView)vc).IsKeyboardUp && countSearchChanged < 1)
//                    {
//                        countSearchChanged++;
//                        tableView.SetContentOffset(CGPoint.Empty, false);
//                    }
//                    return;
//                }
//            }
//            if (vc is CouponsView)
//            {
//                if (((CouponsView)vc).IsKeyboardUp && countSearchChanged < 1)
//                {
//                    countSearchChanged++;
//                    tableView.SetContentOffset(CGPoint.Empty, false);
//                    return;
//                }
//            }
//            if (vc is OffersView)
//                epsilon = -120f;
//            else if (vc is CouponsView)
//                epsilon = -74f;
//            if (TableView.ContentSize.Height < vc.View.Frame.Height + 100f)
//                return;
//            if (!(nSLayoutConstraint.Constant < 0))
//            {
//                if (vc is OffersView && nSLayoutConstraint != null)
//                {
//                    if (((OffersView)vc).ViewModel.IsRefreshingOffers.Value || tableView.ContentOffset.Y <= 0)
//                        return;
//                }
//                if (vc is CouponsView && nSLayoutConstraint != null)
//                {
//                    if (((CouponsView)vc).ViewModel.IsRefreshingOffers.Value || ((CouponsView)vc).ViewModel.IsFetchingMoreOffers.Value || tableView.ContentOffset.Y <= 0)
//                        return;
//                }
//            }
//            if (lastContentOffset < tableView.ContentOffset.Y)
//            {
//                countUp++;
//                Console.WriteLine("Up");
//                if (countUp < 3)
//                    return;
//                if (nSLayoutConstraint.Constant > epsilon)
//                {
//                    var temp = nSLayoutConstraint.Constant;
//                    temp -= (nfloat)Math.Abs(tableView.ContentOffset.Y - lastContentOffset);
//                    if (temp < epsilon)
//                        nSLayoutConstraint.Constant = epsilon;
//                    else
//                        nSLayoutConstraint.Constant -= (nfloat)Math.Abs(tableView.ContentOffset.Y - lastContentOffset);
//                    //if (lastContentOffset <= 0)
//                    //    tableView.SetContentOffset(new CGPoint(lastContentOffset, 0), false);
//                    //    else
//                    //        tableView.SetContentOffset(new CGPoint(0f, lastContentOffset), false);
//                }
//            }
//            else if (lastContentOffset > tableView.ContentOffset.Y)
//            {
//                countUp = 0;
//                Console.WriteLine("Down");
//                if (nSLayoutConstraint.Constant < 0)
//                {
//                    var temp = nSLayoutConstraint.Constant;
//                    temp += (nfloat)Math.Abs(tableView.ContentOffset.Y - lastContentOffset);
//                    if (temp > 0)
//                        nSLayoutConstraint.Constant = 0f;
//                    else
//                        nSLayoutConstraint.Constant += (nfloat)Math.Abs(tableView.ContentOffset.Y - lastContentOffset);
//                   // tableView.SetContentOffset(new CGPoint(0f, lastContentOffset), false);
//                }
//            }

//            if ( tableView.ContentSize.Height > vc.View.Frame.Size.Height && tableView.ContentOffset.Y >= tableView.ContentSize.Height - tableView.Frame.Size.Height ) {
//                 tableView.SetContentOffset(new CGPoint(tableView.ContentOffset.X, tableView.ContentSize.Height - tableView.Frame.Size.Height), animated: false);
//            }
//            tempContentOffset = lastContentOffset;
//            lastContentOffset = tableView.ContentOffset.Y;
//        }
//    }
//}
