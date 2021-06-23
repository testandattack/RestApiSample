using GtcRest.Interfaces.Repository;
using GtcRest.Interfaces.Service;
using GtcRest.Models.Domain;
using GtcRest.Models.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using GtcRest.Models.Enum;

namespace GtcRest.Service.Services
{
    public class GtcService : IGtcService
    {
        private readonly IGtcRepo _gtcRepo;
        private readonly ILogger<GtcService> _logger;
        private readonly Settings _settings;

        public GtcService(IOptionsSnapshot<Settings> settings, ILogger<GtcService> logger, IGtcRepo gtcRepo)
        {
            _logger = logger;
            _gtcRepo = gtcRepo;
            _settings = settings.Value;
        }

        public async Task<List<GtcModel>> GetGtcAsync()
        {
            var setList = await _gtcRepo.GetGtcAsync();
            return setList;
        }

        public async Task<GtcModel> GetGtcAsync(int id)
        {
            var gtc = await _gtcRepo.GetGtcAsync(id);
            return gtc;
        }

        public async Task<GtcModel> CreateGtcAsync(GtcModel GtcModel)
        {
            var result = await _gtcRepo.CreateGtcAsync(GtcModel);
            if (result == null)
            {
                _logger.LogWarning("_gtcRepo.CreateGtcAsync returned a null value.");
                return null;
            }
            else
            {
                return result;
            }
        }

        public async Task<GtcModel> UpdateGtcAsync(GtcModel GtcModel)
        {
            var newGtcModel = await _gtcRepo.UpdateGtcAsync(GtcModel);
            return newGtcModel;
        }

        public async Task<OperationResult> DeleteGtcAsync(int id)
        {
            var gtc = await _gtcRepo.GetGtcAsync(id);
            if(gtc == null)
            {
                return OperationResult.NotFound;
            }
            return await _gtcRepo.DeleteGtcAsync(gtc);
        }
    }
}
