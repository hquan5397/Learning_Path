using API_16_3_2020.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_16_3_2020.Controllers
{
    [Authorize(Roles = "Manager")]
    public class EmployeeController : ApiController
    {
        public IHttpActionResult GetAllEmployee()
        {
            IList<EmployeeViewModel> employees = null;
            using (var context = new EquipmentEntities())
            {
                employees = context.AspNetUsers.Select(s => new EmployeeViewModel()
                {
                    UserName = s.UserName,
                    Email = s.Email,
                    PhoneNumber=s.PhoneNumber
                }).ToList<EmployeeViewModel>();
            }
            if (employees.Count == 0)
            {
                return NotFound();
            }
            return Ok(employees);
        }
    }
}
