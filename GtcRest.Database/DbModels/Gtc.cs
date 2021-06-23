using System.ComponentModel.DataAnnotations.Schema;

namespace GtcRest.Database.DbModels
{
    [Table("Gtc")]
    public partial class Gtc
    {
        public int Id { get; set; }

        public string Description { get; set; }
    }
}
