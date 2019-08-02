namespace kgrid.Models
{
    using kgrid.Dtos;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EmpCountries
    {
        [Key]
        public int EmpCountryId { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        public int EmpId { get; set; }

        public virtual EmployeeList EmployeeList { get; set; }

        public static implicit operator EmpCountryDto(EmpCountries oc)
        {
            return new EmpCountries()
            {
               EmpCountryId=oc.EmpCountryId,
               Country=oc.Country,
               EmpId=oc.EmpId
            };
        }

    }
}
