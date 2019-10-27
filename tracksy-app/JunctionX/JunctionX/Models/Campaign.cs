using System;

namespace JunctionX.Models
{
    public class Campaign
    {
        public int? Id { get; set; }
        public int? EmpyrOfferId { get; set; }
        public int? PublicShopId { get; set; }
        public string ShopName { get; set; }
        public string ShopLogoUrl { get; set; }
        public string ShopMarketingImageUrl { get; set; }
        public string ShopPhone { get; set; }
        public string ShopAddress { get; set; }
        public string ShopWebsite { get; set; }
        public Coordinate ShopLocation { get; set; }
        public string CoverUrl { get; set; }
        public string CampaignType { get; set; }
        public string CampaignName { get; set; }
        public string Description { get; set; }
        public string CashbackAmount { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime? AvailableTo { get; set; }
        public string UseConditions { get; set; }
        public string ShareText { get; set; }

        //local properties
        public string CampaignTypeImage { get; set; }
    }
}
