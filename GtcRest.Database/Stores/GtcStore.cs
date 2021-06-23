using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GtcRest.Database.DbModels;
using GtcRest.Database.Interfaces;
using GtcRest.Models.Enum;
using Serilog;

namespace GtcRest.Database.Stores
{
    public class GtcStore : IGtcStore
    {
        private readonly UserContext _db;
        private readonly ILogger<GtcStore> _logger;

        public GtcStore(UserContext db, ILogger<GtcStore> logger)
        {
            _db = db;
            _logger = logger;
        }

        #region -- Gtc -----
        // Create___________________________________
        public async Task<Gtc> CreateGtcAsync(Gtc gtc)
        {
            var existingSet = await _db.Gtcs.FirstOrDefaultAsync(e =>
                e.Description == gtc.Description);

            if (existingSet != null)
            {
                return null;
            }
            var result = _db.Gtcs.Add(gtc);
            await _db.SaveChangesAsync();
            return gtc;
        }

        
        // Read_____________________________________
        public async Task<Gtc> GetGtcAsync(int Id)
        {
            return await _db.Gtcs.Where(e => e.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<List<Gtc>> GetGtcAsync()
        {
            return await _db.Gtcs.ToListAsync();
        }

        
        // Update___________________________________
        public async Task<Gtc> UpdateGtcAsync(Gtc gtc)
        {
            Gtc existingSet;
            try
            {
                existingSet = await _db.Gtcs.SingleAsync(c =>
                    c.Description == gtc.Description);

                await _db.SaveChangesAsync();
                return existingSet;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "UpdateGtcAsync failed trying to update {@gtc}", gtc);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateGtcAsync had an unknown error trying to update {@gtc}", gtc);
                throw;
            }
        }

        
        // Delete__________________________________
        public async Task<OperationResult> DeleteGtcAsync(Gtc item)
        {
            try
            {
                _db.Gtcs.Remove(item);
                int numChanged = await _db.SaveChangesAsync();
                if(numChanged == 1)
                {
                    return OperationResult.Deleted;
                }
                return OperationResult.BadRequest;
            }
            catch (Exception ex)
            {
                Log.ForContext<GtcStore>().Error(ex, "DeleteGtcAsync failed with id={Id}", item.Id);
            }
            return OperationResult.Exception;
        }
        #endregion
    }
}
