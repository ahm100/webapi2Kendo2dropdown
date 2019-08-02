using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using kgrid.Dtos;
using kgrid.Models;

namespace kgrid.Controllers
{
    [RoutePrefix("api/Count")]

    public class CountriesController : ApiController
    {
        private Emp db = new Emp();

        internal IQueryable<EmpCountries> GetCountryList()
        {
            return db.EmpCountries;
        }

        //GET: api/Countries
        [Route("")]
        public IHttpActionResult GetEmpCountries()
        {
            var x = GetCountryList();
            return Ok(x);
        }


        // GET: api/Countries/5
        [Route("{empid}")]
        [ResponseType(typeof(EmpCountries))]
        public IHttpActionResult GetEmpCountriesByEmpId(int empid)
        {
            var empCountries = GetCountryList().Where(x => x.EmpId == empid).ToList<EmpCountries>();
            
            if (empCountries == null)
            {
                return NotFound();
            }

            var countryDtos = Mapper.Map<List<EmpCountries>, List<EmpCountryDto>>(empCountries);

            //return Ok(countryDtos);
            return Ok(empCountries); // Bacuase Lazy loading got disabled it doesnt make issue to send without mapping
        }

        // PUT: api/Countries/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmpCountries(int id, EmpCountries empCountries)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != empCountries.EmpCountryId)
            {
                return BadRequest();
            }

            db.Entry(empCountries).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpCountriesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Countries
        [ResponseType(typeof(EmpCountries))]
        public IHttpActionResult PostEmpCountries(EmpCountries empCountries)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EmpCountries.Add(empCountries);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = empCountries.EmpCountryId }, empCountries);
        }

        // DELETE: api/Countries/5
        [ResponseType(typeof(EmpCountries))]
        public IHttpActionResult DeleteEmpCountries(int id)
        {
            EmpCountries empCountries = db.EmpCountries.Find(id);
            if (empCountries == null)
            {
                return NotFound();
            }

            db.EmpCountries.Remove(empCountries);
            db.SaveChanges();

            return Ok(empCountries);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmpCountriesExists(int id)
        {
            return db.EmpCountries.Count(e => e.EmpCountryId == id) > 0;
        }
    }
}