using GtcRest.Models.Domain;
using GtcRest.Models.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GtcRest.Interfaces.Repository
{
    public interface IGtcRepo
    {
        Task<GtcModel> CreateGtcAsync(GtcModel GtcModel);
        
        Task<GtcModel> GetGtcAsync(int Id);
        
        Task<List<GtcModel>> GetGtcAsync();
        
        Task<GtcModel> UpdateGtcAsync(GtcModel Gtc);
                
        Task<OperationResult> DeleteGtcAsync(GtcModel GtcModel);
    }
}
