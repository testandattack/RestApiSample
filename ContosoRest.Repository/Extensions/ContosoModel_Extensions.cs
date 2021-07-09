using ContosoRest.Database.DbModels;
using ContosoRest.Models.Domain;
using System.Collections.Generic;
using System.Linq;

namespace ContosoRest.Repository.Extensions
{
    public static class ContosoModel_Extensions
    {
        public static Contoso ToContoso(this ContosoModel ContosoModel)
        {
            var contoso = new Contoso();
            contoso.Description = ContosoModel.Description;
            contoso.Id = ContosoModel.Id;

            return contoso;
        }

        public static List<Contoso> ToContosos(this List<ContosoModel> ContosoModels)
        {
            List<Contoso> contosos = new List<Contoso>();
            foreach(var ContosoModel in ContosoModels)
            {
                contosos.Add(ContosoModel.ToContoso());
            }
            return contosos;
        }

        public static ContosoModel ToContosoModel(this Contoso contoso)
        {
            if (contoso == null)
                return null;

            var ContosoModel = new ContosoModel
            {
                Id = contoso.Id,
                Description = contoso.Description,
            };
            return ContosoModel;
        }

        public static List<ContosoModel> ToContosoModels(this List<Contoso> contosos)
        {
            List<ContosoModel> ContosoModels = new List<ContosoModel>();
            ContosoModels = contosos.Select(s => new ContosoModel
                {
                    Id = s.Id,
                    Description = s.Description,
                }).ToList();

            return ContosoModels;
        }
    }
}
