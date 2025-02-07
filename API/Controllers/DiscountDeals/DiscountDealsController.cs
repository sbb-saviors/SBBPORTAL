using API.Helpers;
using CORE.Models;
using CORE.ViewModel.DiscountDeals.FormModel;
using CORE.ViewModel.SSS.FormModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq.Expressions;

namespace API.Controllers.DiscountDeals
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountDealsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DiscountDealsController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost("List", Name = "DiscountDealsList")]
        public IActionResult List(int status = 0, string kategori = "HEPSİ")
        {
            try
            {
                IQueryable<indirim_anlasmalari> model = _context.indirim_anlasmalaris.Where(w => (status == 0 ? w.SilindiMi == false : 1 == 1) && (kategori == "HEPSİ" ? 1 == 1 : w.Kategori == kategori));



                var searchFields = new Dictionary<string, Expression<Func<indirim_anlasmalari, string>>>
                {
                    { "Baslik", item => (item.Baslik ?? "").ToLower() ?? "" },
                };

                var (data, recordsTotal, recordsFiltered) = DataTableUtils.ApplyDataTableParameters(
                    model,
                    Request,
                    searchFields,
                    "Id");


                return Ok(new { data, recordsTotal, recordsFiltered, message = "Success", statusCode = "200", section = "List" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { data = ex.Message.ToString(), message = "Error", statusCode = "500", section = "List" });
            }
        }


        [HttpGet("Get", Name = "DiscountDealsGet")]
        public IActionResult Get([BindRequired] int Id)
        {
            try
            {

                if (Id == 0)
                {
                    return BadRequest(new { data = "", message = "Id is required", statusCode = "400", section = "Get" });
                }

                var model = _context.indirim_anlasmalaris.FirstOrDefault(w => w.Id == Id);

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

        [HttpPost("Add", Name = "DiscountDealsAdd")]
        public IActionResult AddAsync([FromForm] DiscountDealsFormModel values)
        {
            try
            {
                var user = HttpContext.Items["User"] as ikys_user;

                /*var userData = _context.Users.FirstOrDefault(x => x.Id == values.UserId);

                if (userData == null)
                {
                    return BadRequest(new { data = "", message = "Error: User Not Found", statusCode = "400", section = "Add" });
                }*/


                var model = new indirim_anlasmalari();
                model.Kategori = values.Kategori;
                model.Baslik = values.Baslik;
                model.Aciklama = values.Aciklama;
                model.OlusturmaTarihi = DateTime.Now;
                _context.indirim_anlasmalaris.Add(model);
                _context.SaveChanges();

                return Ok(new { data = "", message = "Success", statusCode = "200", section = "Add" });
            }
            catch (Exception)
            {
                return BadRequest(new { data = "", message = "Error: ", statusCode = "500", section = "Add" });
            }
        }


        [HttpPost("Update", Name = "DiscountDealsUpdate")]
        public IActionResult UpdateAsync([FromForm] DiscountDealsFormModel values)
        {
            try
            {
                var user = HttpContext.Items["User"] as ikys_user;

                /*var userData = _context.ikys_users.FirstOrDefault(x => x.Id == values.UserId);

                if (userData == null)
                {
                    return BadRequest(new { data = "", message = "Error: User Not Found", statusCode = "400", section = "Add" });
                }*/

                var data = _context.indirim_anlasmalaris.FirstOrDefault(x => x.Id == values.Id);
                if (data == null)
                {
                    return BadRequest(new { data = "", message = "Error: Record Not Found", statusCode = "400", section = "Add" });
                }

                data.Baslik = values.Baslik;
                data.Aciklama = values.Aciklama;
                data.Kategori = values.Kategori;
                _context.indirim_anlasmalaris.Update(data);
                _context.SaveChanges();

                return Ok(new { data = "", message = "Success", statusCode = "200", section = "Add" });
            }
            catch (Exception)
            {
                return BadRequest(new { data = "", message = "Error: ", statusCode = "500", section = "Add" });
            }
        }


        [HttpPost("Delete", Name = "DiscountDealsDelete")]
        public IActionResult Delete([BindRequired] int Id)
        {
            try
            {
                if (Id == 0)
                {
                    return BadRequest(new { data = "", message = "Id is required", statusCode = "400", section = "Delete" });
                }
                var model = _context.indirim_anlasmalaris.FirstOrDefault(w => w.Id == Id);
                if (model != null)
                {

                    model.SilindiMi = true;
                    _context.indirim_anlasmalaris.Update(model);
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
