using BricklinkSharp.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using Bricklink_API_Client;

namespace LegoColors
{
    public class KnownColorsForElement
    {
        #region -- Properties -----
        [JsonIgnore]
        public ColorModels colorModels { get; private set; }

        public List<ElementColorInfo> elementColorInfoList { get; set; }
        #endregion

        #region -- Constructors -----
        public KnownColorsForElement()
        {
            colorModels = new ColorModels("ldrawColors.json");
        }

        public KnownColorsForElement(ColorModels globalColorModels)
        {
            this.colorModels = globalColorModels;
        }

        public KnownColorsForElement(string fileName)
        {
            colorModels = new ColorModels("ldrawColors.json");
            LoadKnownColorsForElement(fileName);
        }
        #endregion

        #region -- Methods -----
        public void GetKnownColorsForElement(string elementId)
        {
            elementColorInfoList = new List<ElementColorInfo>();


            BricklinkApiClient client = new BricklinkApiClient();

            var elementKnownColors = client.GetKnownColors(elementId).GetAwaiter().GetResult();

            int identitySeed = 1;
            foreach (var color in elementKnownColors)
            {
                var elementColor = colorModels.colors.Where(c =>
                    c.BlId == color.ColorId)
                    .FirstOrDefault();
                if (elementColor != null)
                {
                    Console.Write($"\rElement: {elementId}\tColor: {elementColor.BlId}\t\t");
                    var elementPriceGuide = client.GetPriceGuide(elementId, ItemType.Part, color.ColorId).GetAwaiter().GetResult();
                    ElementColorInfo elementColorInfo = new ElementColorInfo();
                    elementColorInfo.Id = identitySeed;
                    elementColorInfo.BrickLinkColorName = elementColor.BlName;
                    elementColorInfo.ColorModelId = elementColor.Id;
                    elementColorInfo.InCollection = false;
                    elementColorInfo.UseForMosaicCreation = (elementColor.Material.ToUpper() == "SOLID");
                    elementColorInfo.QtyInCollection = null;
                    elementColorInfo.NumSellers = elementPriceGuide.PriceDetails.Length;
                    elementColorInfo.AvgPrice = elementPriceGuide.QuantityAveragePrice;
                    SetPriceInfo(elementPriceGuide.PriceDetails, ref elementColorInfo);

                    elementColorInfoList.Add(elementColorInfo);
                    identitySeed++;
                }
            }
        }

        private void SetPriceInfo(PriceDetail[] prices, ref ElementColorInfo elementColorInfo)
        {
            if (prices.Length > 10)
            {
                var priceList = prices.AsQueryable().OrderBy(x => x.UnitPrice);
                elementColorInfo.MedianPrice = prices[prices.Length / 2].UnitPrice;
                elementColorInfo.NinetyPercentPrice = prices[(int)(prices.Length * 0.9)].UnitPrice;
            }
            else
                Console.WriteLine("Too few prices\t\t\t");
        }

        public void LoadKnownColorsForElement(string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                elementColorInfoList = JsonConvert.DeserializeObject<List<ElementColorInfo>>(sr.ReadToEnd());
            }
        }

        public void SaveKnownColorsForElement(string fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileName, false))
            {
                sw.Write(JsonConvert.SerializeObject(elementColorInfoList, Formatting.Indented));
            }
        }
        #endregion
    }
}
