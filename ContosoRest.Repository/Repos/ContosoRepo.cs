using ContosoRest.Database.Interfaces;
using ContosoRest.Interfaces.Repository;
using ContosoRest.Models.Domain;
using ContosoRest.Models.Enum;
using ContosoRest.Repository.Extensions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContosoRest.Repository.Repos
{
    public class ContosoRepo : IContosoRepo
    {
        private readonly ILogger<ContosoRepo> _logger;
        private readonly IContosoStore _contosoStore;
        
        public ContosoRepo(IContosoStore contosoStore, ILogger<ContosoRepo> logger)
        {
            _contosoStore = contosoStore;
            _logger = logger;
        }

        public async Task<ContosoModel> CreateContosoAsync(ContosoModel contosoModel)
        {
            var result = await _contosoStore.CreateContosoAsync(contosoModel.ToContoso());
            return result.ToContosoModel();
        }

        public async Task<ContosoModel> GetContosoAsync(int Id)
        {
            var set = await _contosoStore.GetContosoAsync(Id);
            return set.ToContosoModel();
        }

        public async Task<List<ContosoModel>> GetContosoAsync()
        {
            var setList = await _contosoStore.GetContosoAsync();
            return setList.ToContosoModels();
        }

        public async Task<ContosoModel> UpdateContosoAsync(ContosoModel contosoModel)
        {
            var result = await _contosoStore.UpdateContosoAsync(contosoModel.ToContoso());
            return result.ToContosoModel();
        }

        public async Task<OperationResult> DeleteContosoAsync(ContosoModel contosoModel)
        {
            return await _contosoStore.DeleteContosoAsync(contosoModel.ToContoso());
        }
    }
}
