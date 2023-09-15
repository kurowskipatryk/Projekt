using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class NBPTable
    {
        [Key]
        public Guid Id_Table { get; set; }
        public string Table { get; set; }
        public string No { get; set; }
        public DateTime EffectiveDate { get; set; }
        public ICollection<Rate> Rates { get; set; }
    }
}
