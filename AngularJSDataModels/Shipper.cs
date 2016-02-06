using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace AngularJSDataModels
{   
    public class Shipper
    {
        [Key]
        public int ShipperID { get; set; }    
        public string ShipperName { get; set; }     
    }

}
