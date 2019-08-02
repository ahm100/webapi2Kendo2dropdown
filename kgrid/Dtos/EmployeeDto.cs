using kgrid.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kgrid.Dtos
{
    public class EmployeeDto
    {
        [Key]
        public int EmployeeId { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string Company { get; set; }


        //public  List<EmpCountryDto> empCountries { get; set; }

       

    }
}