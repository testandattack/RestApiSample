using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BricklinkSharp.Client;

namespace Bricklink_API_Client
{
    public interface IBricklinkAdapter
    {
        Task<List<SuperSubSetItem>> GetSetInventory(string itemId);

        Task<CatalogItem> GetAnItem(ItemType itemType, string itemId);
    }
}
