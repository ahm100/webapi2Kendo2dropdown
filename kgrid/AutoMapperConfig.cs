using AutoMapper;
using kgrid.Dtos;
using kgrid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kgrid
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize((config) =>
            {
                config.CreateMap<EmployeeList, EmployeeDto>().ReverseMap();
                config.CreateMap<EmpCountries, EmpCountryDto>().ReverseMap();
            });
        }
    }
}