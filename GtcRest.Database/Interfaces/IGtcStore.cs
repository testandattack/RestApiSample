using System.Collections.Generic;
using System.Threading.Tasks;
using GtcRest.Database.DbModels;
using GtcRest.Models.Enum;

namespace GtcRest.Database.Interfaces
{
    public interface IGtcStore
    {
        // Create
        Task<Gtc> CreateGtcAsync(Gtc gtc);
        
        // Read
        Task<Gtc> GetGtcAsync(int Id);
        Task<List<Gtc>> GetGtcAsync();

        // Update
        Task<Gtc> UpdateGtcAsync(Gtc gtc);

        // Delete
        Task<OperationResult> DeleteGtcAsync(Gtc item);

    }
}
