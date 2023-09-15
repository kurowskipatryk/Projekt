using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Rate
    {
        [Key]
        public Guid Id_Rate { get; set; }
        public Guid Id_Table { get; set; }
        public NBPTable Table{ get; set; }
        public string Currency { get; set; }
        public string Code { get; set; }
        public double Mid { get; set; }
        public double Bid { get; set; }
        public double Ask { get; set; }
    }
}
