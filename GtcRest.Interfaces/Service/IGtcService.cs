﻿using System.Collections.Generic;
using System.Threading.Tasks;
using GtcRest.Models.Domain;
using GtcRest.Models.Enum;

namespace GtcRest.Interfaces.Service
{
    public interface IGtcService
    {
        Task<List<GtcModel>> GetGtcAsync();

        Task<GtcModel> GetGtcAsync(int id);

        Task<GtcModel> CreateGtcAsync(GtcModel gtc);

        Task<GtcModel> UpdateGtcAsync(GtcModel gtc);

        Task<OperationResult> DeleteGtcAsync(int id);
    }
}