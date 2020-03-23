using API_16_3_2020.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_16_3_2020.Controllers
{
    [Authorize(Roles ="Employee")]
    public class RequestController : ApiController
    {
        [Route("api/Request/Post")]
        public IHttpActionResult PostRequest([FromBody]RequestViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid request");
            using(var context = new EquipEntities())
            {
                try
                {
                    var result = context.INSERT_REQUEST(model.IDEmployee, model.UserName, model.EquipmentType);
                    if (result == 1)
                    {
                        context.SaveChanges();
                        return Ok();
                    }
                        
                    else
                        return BadRequest("server error");
                }
                catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        [HttpGet]
        [Route("api/Request/GetListOfTypeEquipment")]
        public IHttpActionResult GetListOfTypeEquipment()
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid request");
            using(var context = new EquipEntities())
            {
                try
                {
                    var types = context.EQUIPMENTs.GroupBy(s=> s.Type).ToList();
                    List<string> Types = new List<string>();
                    foreach(var item in types)
                    {
                        Types.Add(item.Key);
                    }
                    return Ok(Types);
                }
                catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        
    }
}
