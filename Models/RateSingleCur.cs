using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RateSingleCur
    {
        [Key]
        public Guid Id_Rate { get; set; }
        public string no { get; set; }
        public double Bid { get; set; }
        public double Ask { get; set; }
        public double Mid { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
