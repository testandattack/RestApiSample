using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoRest.Database.DbModels
{
    [Table("Contoso")]
    public partial class Contoso
    {
        public int Id { get; set; }

        public string Description { get; set; }
    }
}
