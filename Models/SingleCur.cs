using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SingleCur
    {
        public Guid Id_Table { get; set; }
        public string Table { get; set; }
        public string currency { get; set; }
        public string code { get; set; }
        public ICollection<RateSingleCur> Rates { get; set; }
    }
}
