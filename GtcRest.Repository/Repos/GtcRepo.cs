using GtcRest.Database.Interfaces;
using GtcRest.Interfaces.Repository;
using GtcRest.Models.Domain;
using GtcRest.Models.Enum;
using GtcRest.Repository.Extensions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GtcRest.Repository.Repos
{
    public class GtcRepo : IGtcRepo
    {
        private readonly ILogger<GtcRepo> _logger;
        private readonly IGtcStore _gtcStore;
        
        public GtcRepo(IGtcStore gtcStore, ILogger<GtcRepo> logger)
        {
            _gtcStore = gtcStore;
            _logger = logger;
        }

        public async Task<GtcModel> CreateGtcAsync(GtcModel gtcModel)
        {
            var result = await _gtcStore.CreateGtcAsync(gtcModel.ToGtc());
            return result.ToGtcModel();
        }

        public async Task<GtcModel> GetGtcAsync(int Id)
        {
            var set = await _gtcStore.GetGtcAsync(Id);
            return set.ToGtcModel();
        }

        public async Task<List<GtcModel>> GetGtcAsync()
        {
            var setList = await _gtcStore.GetGtcAsync();
            return setList.ToGtcModels();
        }

        public async Task<GtcModel> UpdateGtcAsync(GtcModel gtcModel)
        {
            var result = await _gtcStore.UpdateGtcAsync(gtcModel.ToGtc());
            return result.ToGtcModel();
        }

        public async Task<OperationResult> DeleteGtcAsync(GtcModel gtcModel)
        {
            return await _gtcStore.DeleteGtcAsync(gtcModel.ToGtc());
        }
    }
}
