using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ContosoRest.Models.Shared;
using BricklinkSharp.Client;

namespace Bricklink_API_Client
{
    public class BricklinkAdapter : IBricklinkAdapter
    {
        #region -- Properties -----
        private readonly ILogger<BricklinkAdapter> _logger;
        private readonly Settings _settings;

        private IBricklinkClient _blClient { get; set; }

        #region -- Properties for BL API-----
        public string CatalogItemType_Minifig = "MINIFIG";
        public string CatalogItemType_PART = "PART";
        public string CatalogItemType_SET = "SET";
        public string CatalogItemType_BOOK = "BOOK";
        public string CatalogItemType_GEAR = "GEAR";
        public string CatalogItemType_CATALOG = "CATALOG";
        public string CatalogItemType_INSTRUCTION = "INSTRUCTION";
        public string CatalogItemType_UNSORTED_LOT = "UNSORTED_LOT";
        public string CatalogItemType_ORIGINAL_BOX = "ORIGINAL_BOX";

        public string API_BaseUrl = @"https://api.bricklink.com/api/store/v1";
        public string DateTimeFormat = @"yyyy-MM-dd'T'HH:mm:ss.SSSZ";

        public string Catalog_GetItem = @"/items/{type}/{no}";
        public string Catalog_GetItemImage = @"/items/{type}/{no}/images/{color_Id}";
        public string Catalog_GetPrice_Current = @"/items/{type}/{no}/price?guide_type=stock";
        public string Catalog_GetPrice_Sold = @"/items/{type}/{no}/price?guide_type=sold";
        public string Catalog_GetItemKnownColors = @"/items/{type}/{no}/colors";
        #endregion
        #endregion

        #region -- Constructor -----
        public BricklinkAdapter(IOptionsSnapshot<Settings> settings, ILogger<BricklinkAdapter> logger)
        {
            _logger = logger;
            _settings = settings.Value;
        }

        private void InitializeClient()
        {
            try
            {
                // http://apidev.bricklink.com/redmine/projects/bricklink-api/wiki/Getting_Started
                //Seller Name geoffgr
                //StoreName   Graybricks
                BricklinkClientConfiguration.Instance.ConsumerKey = _settings.brickLinkApiSettings.ConsumerKey;
                BricklinkClientConfiguration.Instance.ConsumerSecret = _settings.brickLinkApiSettings.ConsumerSecret;

                BricklinkClientConfiguration.Instance.TokenValue = _settings.brickLinkApiSettings.TokenValue;
                BricklinkClientConfiguration.Instance.TokenSecret = _settings.brickLinkApiSettings.TokenSecret;

                // https://github.com/gebirgslok/BricklinkSharp
                _blClient = BricklinkClientFactory.Build();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }
        #endregion


        public async Task<BricklinkSharp.Client.CatalogItem> GetAnItem(BricklinkSharp.Client.ItemType itemType, string itemId)
        {
            string fullItemId;
            if (itemId.Contains("-"))
                fullItemId = itemId;
            else
                fullItemId = $"{itemId}-1";

            var catalogItem = await _blClient.GetItemAsync(itemType, fullItemId);
            return catalogItem;
        }

        public async Task<List<BricklinkSharp.Client.SuperSubSetItem>> GetSetInventory(string itemId)
        {
            string fullItemId;
            if (itemId.Contains("-"))
                fullItemId = itemId;
            else
                fullItemId = $"{itemId}-1";

            List<SuperSubSetItem> items = new List<SuperSubSetItem>();

            _logger.LogInformation("Getting inventory for {ItemId}", fullItemId);
            try
            {
                var subsets = await _blClient.GetSubsetsAsync(ItemType.Set, fullItemId, breakMinifigs: false);
                for (int x = 0; x < subsets.Length; x++)
                {
                    items.Add(subsets[x].Entries[0].Item);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Got exception on set {fullItemId}", fullItemId);
            }
            return items;
        }
    }
}
