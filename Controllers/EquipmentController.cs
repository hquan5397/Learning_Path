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
    public class EquipmentController : ApiController
    {
       
        public IHttpActionResult GetAllEquipments()
        {
            IList<EquipmentViewModel> equipments = null;
            using (var context = new EquipmentEntities())
            {
                equipments = context.EQUIPMENTs.Select(s => new EquipmentViewModel()
                {
                    ID = s.ID,
                    Type = s.Type,
                    Name = s.Name,
                    Status = s.Status,
                    Description = s.Description,
                    UserName = s.UserName
                }).ToList<EquipmentViewModel>();
            }
            if (equipments.Count == 0)
            {
                return NotFound();
            }
            return Ok(equipments);
        }
      
        public IHttpActionResult GetEquipment(string id)
        {
            EquipmentViewModel equipment = new EquipmentViewModel();
            using (var context = new EquipmentEntities())
            {
                EQUIPMENT emp = context.EQUIPMENTs.Where(s => s.ID == id).FirstOrDefault();
               
                if (emp != null)
                {
                    equipment.ID = emp.ID;
                    equipment.Type = emp.Type;
                    equipment.Name = emp.Name;
                    equipment.Status = emp.Status;
                    equipment.Description = emp.Description;           
                }
                else
                    return NotFound();
            }    
            return Ok(equipment);
        }
        [Route("api/Create")]
        public IHttpActionResult PostNewEquipment([FromBody]EquipmentViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");
            using(var context = new EquipmentEntities())
            {
               var result= context.INSERT_EQUIPMENT(model.Type, model.Name, model.Status, model.Description);
                if (result == 1)
                {
                    context.SaveChanges();
                    return Ok();
                }
                else
                    return BadRequest("Server error");
            }
        }
        [Route("api/Edit")]
        public IHttpActionResult PutEquipment([FromBody]EquipmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Not a valid model");
            }
            using(var context = new EquipmentEntities())
            {
                var existingEquipment = context.EQUIPMENTs.Where(s => s.ID == model.ID).
                                                                FirstOrDefault<EQUIPMENT>();
                if (existingEquipment != null)
                {
                    existingEquipment.Type = model.Type;
                    existingEquipment.Name = model.Name;
                    existingEquipment.Status = model.Status;
                    existingEquipment.Description = model.Description;
                    context.SaveChanges();
                }
                else
                    return NotFound();
            }
            return Ok();
        }
       
        public IHttpActionResult DeleteEquipment(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid request");
            }
            using(var context = new EquipmentEntities())
            {
                var equip = context.EQUIPMENTs.Where(s => s.ID == id).FirstOrDefault();
                if (equip == null)
                    return NotFound();
                context.Entry(equip).State = System.Data.Entity.EntityState.Deleted;
                context.SaveChanges();
                return Ok();
            }
        }

        [HttpPut]
        [Route("api/Assign")]     
        public IHttpActionResult AssignToEmp([FromBody]AssignModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid request");
            }
            using(var context = new EquipmentEntities())
            {
                var equip = context.EQUIPMENTs.Where(s => s.ID == model.IDEquip).FirstOrDefault();
                var emp = context.AspNetUsers.Where(s => s.UserName == model.UserName).FirstOrDefault();
                if (equip == null || emp == null)
                    return NotFound();
                var result = context.ASSIGN_TO_EMPLOYEE(model.IDEquip,model.UserName);
                if (result == 1)
                    return Ok();
                else
                    return BadRequest("Server Error");
            }
        }

        [HttpGet]
        [Route("api/Unassign")]
        public IHttpActionResult Unassign(string IDEquip)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid request");
            using(var context = new EquipmentEntities())
            {
                try
                {
                    var result = context.UNASSIGN_EQUIPMENT(IDEquip);
                    if (result == 1)
                        return Ok();
                    else
                        return BadRequest("Failed to unassign");
                }
                catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        [HttpGet]
        [Route("api/ShowAssigned")]
        public IHttpActionResult ShowAssignedEquip(string IDEmployee)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid request");
            using(var context = new EquipmentEntities())
            {
                try
                {
                    var listAssigned = context.Database.SqlQuery<EquipmentViewModel>("LIST_EQUIP_ASSIGNED "+"'"+IDEmployee+"'").ToList();
                    return Ok(listAssigned);
                }
                catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet]
        [Route("api/GetRequests")]
        public IHttpActionResult GetRequests()
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid request");
            using(var context = new EquipmentEntities())
            {
                try
                {
                    var requests = context.REQUESTs.Select(s => new RequestViewModel()
                    {
                        ID = s.ID,
                        UserName = s.UserName,
                        DateRequest = s.DateRequest,
                        EquipmentType = s.EquipmentType,
                        Status = s.Status
                    }).ToList<RequestViewModel>();
                    return Ok(requests);
                }
                catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet]
        [Route("api/GetEquipsNonAssignByType")]
        public IHttpActionResult GetEquipsNonAssignByType(string Type)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid request");
            using(var context = new EquipmentEntities())
            {
                try
                {
                    var equips = context.EQUIPMENTs.Where(s => s.Type == Type && s.UserName == null).Select(s=> new EquipmentViewModel() {
                        ID = s.ID,
                        Type = s.Type,
                        Name = s.Name,
                        Status = s.Status,
                        Description = s.Description
                    }).ToList<EquipmentViewModel>();
                    return Ok(equips);
                }
                catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet]
        [Route("api/Tick")]
        public IHttpActionResult TickRequest(string IDRequest, string IDEquipment,string IDEmployee)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid request");
            using (var context = new EquipmentEntities())
            {
                try
                {
                    var result = context.ASSIGN_TO_EMPLOYEE(IDEquipment, IDEmployee);
                    if (result == 1)
                    {
                        var tick = context.TICK_REQUEST(IDRequest);
                        if (tick == 1)
                            return Ok("success");
                        else
                            return BadRequest("tick error with request record");
                    }
                    return BadRequest("Server error");
                }
                catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
             }
        }
    }
}
