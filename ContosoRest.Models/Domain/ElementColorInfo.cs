using System;
using System.Collections.Generic;
using System.Text;

namespace ContosoRest.Models.Domain
{
    public class ElementColorInfo
    {
        public int Id { get; set; }

        public int ColorModelId { get; set; }

        public string BrickLinkColorName { get; set; }

        public bool InCollection { get; set; }

        public int? QtyInCollection { get; set; }

        public int? NumSellers { get; set; }

        public decimal? AvgPrice { get; set; }
        
        public decimal? MedianPrice { get; set; }

        public decimal? NinetyPercentPrice { get; set; }

        public bool UseForMosaicCreation { get; set; }
    }
}
