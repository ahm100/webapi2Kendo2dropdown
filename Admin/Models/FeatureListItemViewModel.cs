using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEshop.Areas.Admin.Models
{
    public class FeatureListItemViewModel
    {
        public int FeatureID { get; set; }

        public string Title { get; set; }
        public string FeatureValue { get; set; }
    }
}