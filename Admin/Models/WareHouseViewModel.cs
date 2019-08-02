using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEshop.Areas.Admin.Models
{
    public class WareHouseViewModel
    {
        public int ProductID { get; set; }

        public string ProductTitle { get; set; }

        public int Count { get; set; }
    }
}