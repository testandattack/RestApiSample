using GtcRest.Database.DbModels;
using GtcRest.Models.Domain;
using System.Collections.Generic;
using System.Linq;

namespace GtcRest.Repository.Extensions
{
    public static class GtcModel_Extensions
    {
        public static Gtc ToGtc(this GtcModel GtcModel)
        {
            var gtc = new Gtc();
            gtc.Description = GtcModel.Description;
            gtc.Id = GtcModel.Id;

            return gtc;
        }

        public static List<Gtc> ToGtcs(this List<GtcModel> GtcModels)
        {
            List<Gtc> gtcs = new List<Gtc>();
            foreach(var GtcModel in GtcModels)
            {
                gtcs.Add(GtcModel.ToGtc());
            }
            return gtcs;
        }

        public static GtcModel ToGtcModel(this Gtc gtc)
        {
            if (gtc == null)
                return null;

            var GtcModel = new GtcModel
            {
                Id = gtc.Id,
                Description = gtc.Description,
            };
            return GtcModel;
        }

        public static List<GtcModel> ToGtcModels(this List<Gtc> gtcs)
        {
            List<GtcModel> GtcModels = new List<GtcModel>();
            GtcModels = gtcs.Select(s => new GtcModel
                {
                    Id = s.Id,
                    Description = s.Description,
                }).ToList();

            return GtcModels;
        }
    }
}
