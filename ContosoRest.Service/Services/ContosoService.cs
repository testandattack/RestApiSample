using ContosoRest.Interfaces.Repository;
using ContosoRest.Interfaces.Service;
using ContosoRest.Models.Domain;
using ContosoRest.Models.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContosoRest.Models.Enum;

namespace ContosoRest.Service.Services
{
    public class ContosoService : IContosoService
    {
        private readonly IContosoRepo _contosoRepo;
        private readonly ILogger<ContosoService> _logger;
        private readonly Settings _settings;

        public ContosoService(IOptionsSnapshot<Settings> settings, ILogger<ContosoService> logger, IContosoRepo contosoRepo)
        {
            _logger = logger;
            _contosoRepo = contosoRepo;
            _settings = settings.Value;
        }

        public async Task<ContosoModel> CreateContosoAsync(ContosoModel ContosoModel)
        {
            var result = await _contosoRepo.CreateContosoAsync(ContosoModel);
            if (result == null)
            {
                _logger.LogWarning("_contosoRepo.CreateContosoAsync returned a null value.");
                return null;
            }
            else
            {
                return result;
            }
        }

        public async Task<List<ContosoModel>> GetContosoAsync()
        {
            var setList = await _contosoRepo.GetContosoAsync();
            return setList;
        }

        public async Task<ContosoModel> GetContosoAsync(int id)
        {
            var contoso = await _contosoRepo.GetContosoAsync(id);
            return contoso;
        }

        public async Task<ContosoModel> UpdateContosoAsync(ContosoModel ContosoModel)
        {
            var newContosoModel = await _contosoRepo.UpdateContosoAsync(ContosoModel);
            return newContosoModel;
        }

        public async Task<OperationResult> DeleteContosoAsync(int id)
        {
            var contoso = await _contosoRepo.GetContosoAsync(id);
            if(contoso == null)
            {
                return OperationResult.NotFound;
            }
            return await _contosoRepo.DeleteContosoAsync(contoso);
        }
    }
}
