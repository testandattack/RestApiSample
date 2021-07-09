using ContosoRest.Models.Domain;
using ContosoRest.Models.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContosoRest.Interfaces.Repository
{
    public interface IContosoRepo
    {
        Task<ContosoModel> CreateContosoAsync(ContosoModel ContosoModel);
        
        Task<List<ContosoModel>> GetContosoAsync();

        Task<ContosoModel> GetContosoAsync(int Id);

        Task<ContosoModel> UpdateContosoAsync(ContosoModel Contoso);
                
        Task<OperationResult> DeleteContosoAsync(ContosoModel ContosoModel);
    }
}
