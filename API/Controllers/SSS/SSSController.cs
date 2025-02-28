﻿using API.Helpers;
using CORE.Models;
using CORE.ViewModel.SSS.FormModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq.Expressions;

namespace API.Controllers.SSS
{
    [Route("api/[controller]")]
    [ApiController]
    public class SSSController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SSSController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost("List", Name = "SSSList")]
        public IActionResult List(int status = 0)
        {
            try
            {
                IQueryable<sikca_sorulan_sorular> model = _context.sikca_sorulan_sorulars.Where(w => (status == 0 ? w.SilindiMi == false : 1 == 1));



                var searchFields = new Dictionary<string, Expression<Func<sikca_sorulan_sorular, string>>>
                {
                    { "SoruBaslik", item => (item.SoruBaslik ?? "").ToLower() ?? "" },
                };

                var (data, recordsTotal, recordsFiltered) = DataTableUtils.ApplyDataTableParameters(
                    model,
                    Request,
                    searchFields,
                    "Id");
              

                return Ok(new { data = data, recordsTotal, recordsFiltered, message = "Success", statusCode = "200", section = "List" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { data = ex.Message.ToString(), message = "Error", statusCode = "500", section = "List" });
            }
        }

        [HttpGet("Get", Name = "SSSGet")]
        public IActionResult Get([BindRequired] int Id)
        {
            try
            {

                if (Id == 0)
                {
                    return BadRequest(new { data = "", message = "Id is required", statusCode = "400", section = "Get" });
                }

                var model = _context.sikca_sorulan_sorulars.FirstOrDefault(w => w.Id == Id);

                if (model == null)
                {
                    return BadRequest(new { data = "", message = "Record not found", statusCode = "404", section = "Get" });
                }


                return Ok(new { data = model, message = "Success", statusCode = "200", section = "Get" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { data = ex.Message.ToString(), message = "Error", statusCode = "500", section = "Get" });
            }
        }

        [HttpPost("Add", Name = "SSSAdd")]
        public IActionResult AddAsync([FromForm] SssFormModel values)
        {
            try
            {
                var user = HttpContext.Items["User"] as ikys_user;

                /*var userData = _context.Users.FirstOrDefault(x => x.Id == values.UserId);

                if (userData == null)
                {
                    return BadRequest(new { data = "", message = "Error: User Not Found", statusCode = "400", section = "Add" });
                }*/


                var model = new sikca_sorulan_sorular();
                model.SoruBaslik = values.SoruBaslik;
                model.SoruCevap = values.SoruCevap;
                model.OlusturmaTarihi = DateTime.Now;
                _context.sikca_sorulan_sorulars.Add(model);
                _context.SaveChanges();

                return Ok(new { data = "", message = "Success", statusCode = "200", section = "Add" });
            }
            catch (Exception)
            {
                return BadRequest(new { data = "", message = "Error: ", statusCode = "500", section = "Add" });
            }
        }


        [HttpPost("Update", Name = "SSSUpdate")]
        public IActionResult UpdateAsync([FromForm] SssFormModel values)
        {
            try
            {
                var user = HttpContext.Items["User"] as ikys_user;

                /*var userData = _context.ikys_users.FirstOrDefault(x => x.Id == values.UserId);

                if (userData == null)
                {
                    return BadRequest(new { data = "", message = "Error: User Not Found", statusCode = "400", section = "Add" });
                }*/

                var data = _context.sikca_sorulan_sorulars.FirstOrDefault(x => x.Id == values.Id);
                if (data == null)
                {
                    return BadRequest(new { data = "", message = "Error: Record Not Found", statusCode = "400", section = "Add" });
                }

                data.SoruBaslik = values.SoruBaslik;
                data.SoruCevap = values.SoruCevap;
                _context.sikca_sorulan_sorulars.Update(data);
                _context.SaveChanges();

                return Ok(new { data = "", message = "Success", statusCode = "200", section = "Add" });
            }
            catch (Exception)
            {
                return BadRequest(new { data = "", message = "Error: ", statusCode = "500", section = "Add" });
            }
        }


        [HttpPost("Delete", Name = "SSSDelete")]
        public IActionResult Delete([BindRequired] int Id)
        {
            try
            {
                if (Id == 0)
                {
                    return BadRequest(new { data = "", message = "Id is required", statusCode = "400", section = "Delete" });
                }
                var model = _context.sikca_sorulan_sorulars.FirstOrDefault(w => w.Id == Id);
                if (model != null)
                {

                    model.SilindiMi = true;
                    _context.sikca_sorulan_sorulars.Update(model);
                    _context.SaveChanges();

                    return Ok(new { data = "", message = "Success", statusCode = "200", section = "Delete" });
                }
                else
                {
                    return NotFound(new { data = "", message = "Error", statusCode = "404", section = "Delete" });
                }
            }
            catch (Exception)
            {
                return BadRequest(new { data = "", message = "Error", statusCode = "500", section = "Delete" });
            }
        }
    }
}
