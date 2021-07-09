using System.Collections.Generic;
using System.Threading.Tasks;
using ContosoRest.Models.Domain;
using ContosoRest.Models.Enum;

namespace ContosoRest.Interfaces.Service
{
    public interface IContosoService
    {
        Task<ContosoModel> CreateContosoAsync(ContosoModel contoso);

        Task<List<ContosoModel>> GetContosoAsync();

        Task<ContosoModel> GetContosoAsync(int id);

        Task<ContosoModel> UpdateContosoAsync(ContosoModel contoso);

        Task<OperationResult> DeleteContosoAsync(int id);
    }
}
