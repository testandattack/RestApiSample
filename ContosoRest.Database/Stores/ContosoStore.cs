using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoRest.Database.DbModels;
using ContosoRest.Database.Interfaces;
using ContosoRest.Models.Enum;
using Serilog;

namespace ContosoRest.Database.Stores
{
    public class ContosoStore : IContosoStore
    {
        private readonly UserContext _db;
        private readonly ILogger<ContosoStore> _logger;

        public ContosoStore(UserContext db, ILogger<ContosoStore> logger)
        {
            _db = db;
            _logger = logger;
        }

        #region -- Contoso -----
        // Create___________________________________
        public async Task<Contoso> CreateContosoAsync(Contoso contoso)
        {
            var existingSet = await _db.Contosos.FirstOrDefaultAsync(e =>
                e.Description == contoso.Description);

            if (existingSet != null)
            {
                return null;
            }
            var result = _db.Contosos.Add(contoso);
            await _db.SaveChangesAsync();
            return contoso;
        }

        
        // Read_____________________________________
        public async Task<Contoso> GetContosoAsync(int Id)
        {
            return await _db.Contosos.Where(e => e.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<List<Contoso>> GetContosoAsync()
        {
            return await _db.Contosos.ToListAsync();
        }

        
        // Update___________________________________
        public async Task<Contoso> UpdateContosoAsync(Contoso contoso)
        {
            Contoso existingSet;
            try
            {
                existingSet = await _db.Contosos.SingleAsync(c =>
                    c.Id == contoso.Id);

                await _db.SaveChangesAsync();
                return existingSet;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "UpdateContosoAsync failed trying to update {@contoso}", contoso);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateContosoAsync had an unknown error trying to update {@contoso}", contoso);
                throw;
            }
        }

        
        // Delete__________________________________
        public async Task<OperationResult> DeleteContosoAsync(Contoso item)
        {
            try
            {
                _db.Contosos.Remove(item);
                int numChanged = await _db.SaveChangesAsync();
                if(numChanged == 1)
                {
                    return OperationResult.Deleted;
                }
                return OperationResult.BadRequest;
            }
            catch (Exception ex)
            {
                Log.ForContext<ContosoStore>().Error(ex, "DeleteContosoAsync failed with id={Id}", item.Id);
            }
            return OperationResult.Exception;
        }
        #endregion
    }
}
