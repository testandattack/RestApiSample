using System.Collections.Generic;
using System.Threading.Tasks;
using ContosoRest.Database.DbModels;
using ContosoRest.Models.Enum;

namespace ContosoRest.Database.Interfaces
{
    public interface IContosoStore
    {
        // Create
        Task<Contoso> CreateContosoAsync(Contoso contoso);
        
        // Read
        Task<Contoso> GetContosoAsync(int Id);
        Task<List<Contoso>> GetContosoAsync();

        // Update
        Task<Contoso> UpdateContosoAsync(Contoso contoso);

        // Delete
        Task<OperationResult> DeleteContosoAsync(Contoso item);

    }
}
