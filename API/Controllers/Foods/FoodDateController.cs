using API.Helpers;
using CORE.Models;
using CORE.ViewModel.FoodsCategory.FormModel;
using CORE.ViewModel.FoodsDate.FormModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API.Controllers.Foods
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodDateController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FoodDateController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("List", Name = "FoodsDateList")]
        public IActionResult List(int status = 0)
        {
            try
            {
                IQueryable<yemek_tarihleri> model = _context.yemek_tarihleris.Where(w => (status == 0 ? w.SilindiMi == false : 1 == 1));



                var searchFields = new Dictionary<string, Expression<Func<yemek_tarihleri, string>>>
                {
                    { "Tarih", item => (item.Tarih.ToString() ?? "").ToLower() ?? "" },
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


        [HttpGet("Get", Name = "FoodsDateGet")]
        public IActionResult Get([BindRequired] int Id)
        {
            try
            {

                if (Id == 0)
                {
                    return BadRequest(new { data = "", message = "Id is required", statusCode = "400", section = "Get" });
                }

                var model = _context.yemek_tarihleris.FirstOrDefault(w => w.Id == Id);

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


        [HttpPost("Add", Name = "FoodsDateAdd")]
        public IActionResult ADD([FromForm] FoodDateFormModel values)
        {
            try
            {
                var user = HttpContext.Items["User"] as ikys_user;

                /* var userData = _context.Users.FirstOrDefault(x => x.Id == values.UserId);

                 if (userData == null)
                 {
                     return BadRequest(new { data = "", message = "Error: User Not Found", statusCode = "400", section = "Add" });
                 }*/


                var model = new yemek_tarihleri();
                model.Tarih = values.Tarih;
                model.CreatedDate = DateTime.Now;
                _context.yemek_tarihleris.Add(model);
                _context.SaveChanges();

                return Ok(new { data = "", message = "Success", statusCode = "200", section = "Add" });
            }
            catch (Exception)
            {
                return BadRequest(new { data = "", message = "Error: ", statusCode = "500", section = "Add" });
            }
        }


        [HttpPost("Update", Name = "FoodsDateUpdate")]
        public IActionResult Update([FromForm] FoodDateFormModel values)
        {
            try
            {

                var user = HttpContext.Items["User"] as ikys_user;

                /* if (values.Id == 0)
                 {
                     return BadRequest(new { data = "", message = "Id is required", statusCode = "400", section = "Update" });
                 }*/

                var model = _context.yemek_tarihleris.FirstOrDefault(w => w.Id == values.Id);

                if (model != null)
                {
                    model.Tarih = values.Tarih;

                    _context.yemek_tarihleris.Update(model);
                    _context.SaveChanges();

                    return Ok(new { data = "", message = "Success", statusCode = "200", section = "Update" });
                }
                else
                {
                    return NotFound(new { data = "", message = "Error", statusCode = "404", section = "Update" });
                }
            }
            catch (Exception)
            {
                return BadRequest(new { data = "", message = "Error: ", statusCode = "500", section = "Add" });
            }
        }

        [HttpPost("Delete", Name = "FoodsDateDelete")]
        public IActionResult Delete([BindRequired] int Id)
        {
            try
            {
                if (Id == 0)
                {
                    return BadRequest(new { data = "", message = "Id is required", statusCode = "400", section = "Delete" });
                }
                var model = _context.yemek_tarihleris.FirstOrDefault(w => w.Id == Id);
                if (model != null)
                {

                    model.SilindiMi = true;
                    _context.yemek_tarihleris.Update(model);
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


        [HttpPost("FoodAdd", Name = "FoodsDateFoodAdd")]
        public IActionResult FoodAdd([FromForm] FoodDateFoodFormModel values)
        {
            try
            {
                if (values.TarihId == 0)
                {
                    return BadRequest(new { data = "", message = "TarihId is required", statusCode = "400", section = "FoodAdd" });
                }

                if (values.FoodId.Count() <= 0)
                {
                    return BadRequest(new { data = "", message = "FoodId is required", statusCode = "400", section = "FoodAdd" });
                }


                foreach (var item in values.FoodId)
                {
                    var model = new yemek_tarihleri_yemekler();
                    model.TarihId = values.TarihId;
                    model.YemekId = item;
                    model.CreatedDate = DateTime.Now;
                    _context.yemek_tarihleri_yemeklers.Add(model);
                    _context.SaveChanges();
                };

                return Ok(new { data = "", message = "Success", statusCode = "200", section = "Add" });
            }
            catch (Exception)
            {
                return BadRequest(new { data = "", message = "Error: ", statusCode = "500", section = "Add" });
            }
        }

        [HttpPost("FoodList", Name = "FoodsDateFoodList")]
        public IActionResult FoodList(int tarihId, int status = 0)
        {
            try
            {
                if (tarihId < 0)
                {
                    return BadRequest(new { data = "", message = "TarihId is required", statusCode = "400", section = "List" });
                }

                IQueryable<yemek_tarihleri_yemekler> model = _context.yemek_tarihleri_yemeklers.Include(i => i.Tarih).Include(i => i.Yemek).Where(w => (status == 0 ? w.SilindiMi == false : 1 == 1) && w.TarihId == tarihId);



                var searchFields = new Dictionary<string, Expression<Func<yemek_tarihleri_yemekler, string>>>
                {

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

        [HttpPost("FoodDelete", Name = "FoodsDateFoodDelete")]
        public IActionResult FoodDelete([BindRequired] int Id)
        {
            try
            {
                if (Id == 0)
                {
                    return BadRequest(new { data = "", message = "Id is required", statusCode = "400", section = "Delete" });
                }
                var model = _context.yemek_tarihleri_yemeklers.FirstOrDefault(w => w.Id == Id);
                if (model != null)
                {

                    model.SilindiMi = true;
                    _context.yemek_tarihleri_yemeklers.Update(model);
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
