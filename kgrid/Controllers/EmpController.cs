using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using kgrid.Models;
using kgrid.Dtos;
using System.Collections.Generic;
using AutoMapper;
using Newtonsoft.Json;
using System.Net.Http;
using Kendo.DynamicLinq;
using System;

namespace kgrid.Controllers
{
    //[EnableCors(origins: "http://localhost:57702", headers: "*", methods: "*")]
    [RoutePrefix("api/Emp")]
    public class EmpController : ApiController
    {
        private Emp db = new Emp();

        public class DataSourceRequest
        {
            public int Take { get; set; }
            public int Skip { get; set; }
            public IEnumerable<Sort> Sort { get; set; }
            public Filter Filter { get; set; }
        }

       

        // GET: api/Emp
        internal IQueryable<EmployeeList> GetEmployeeList()
        {
            return db.EmployeeList;
        }

        [Route("")]
        public DataSourceResult Get(HttpRequestMessage requestMessage)
        {
            DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
                // The request is in the format GET api/products?{take:10,skip:0} and ParseQueryString treats it as a key without value
                requestMessage.RequestUri.ParseQueryString().GetKey(0)
            );

            return db.EmployeeList
                //Entity Framework can page only sorted data
                .OrderBy(x => x.EmployeeId)
                .Select(product => new
                {
                    // Skip the EntityState and EntityKey properties inherited from EF. It would break model binding.
                    product.EmployeeId,
                    product.FirstName,
                    product.LastName,
                    product.Company
                })
            .ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
        }


        //GET: api/Countries
        [Route("fordrop")]
        public IHttpActionResult GetEmps()
        {
            var result = GetEmployeeList().ToList().OrderBy(x => x.EmployeeId);

            return Ok(result);
        }


        // PUT: api/Emp/5
        //[ResponseType(typeof(void))]
        [Route("{id}")]
        [HttpPut]
        public IHttpActionResult PutEmployeeList(int id, EmployeeList employeeList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employeeList.EmployeeId)
            {
                return BadRequest();
            }

            db.Entry(employeeList).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeListExists(id))
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

        // POST: api/Emp
        //[ResponseType(typeof(EmployeeList))]
        [Route("")]
        [HttpPost]
        public IHttpActionResult PostEmployeeList([FromBody] EmployeeList employeeList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EmployeeList.Add(employeeList);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = employeeList.EmployeeId }, employeeList);
        }

        // DELETE: api/Emp/5
        // [ResponseType(typeof(EmployeeList))]
        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteEmployeeList(int id)
        {
            EmployeeList employeeList = db.EmployeeList.Find(id);
            if (employeeList == null)
            {
                return NotFound();
            }

            db.EmployeeList.Remove(employeeList);
            db.SaveChanges();

            return Ok(employeeList);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeListExists(int id)
        {
            return db.EmployeeList.Count(e => e.EmployeeId == id) > 0;
        }
    }
}