using API.Helpers;
using CORE.Models;
using CORE.ViewModel.Foods.FormModel;
using CORE.ViewModel.FoodsCategory.FormModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq.Expressions;

namespace API.Controllers.Foods
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodCategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FoodCategoryController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost("List", Name = "FoodsCategoryList")]
        public IActionResult List(int status = 0)
        {
            try
            {
                IQueryable<yemek_kategorileri> model = _context.yemek_kategorileris.Where(w => (status == 0 ? w.SilindiMi == false : 1 == 1));



                var searchFields = new Dictionary<string, Expression<Func<yemek_kategorileri, string>>>
                {
                    { "KategoriAdi", item => (item.KategoriAdi ?? "").ToLower() ?? "" },
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


        [HttpGet("Get", Name = "FoodsCategoryGet")]
        public IActionResult Get([BindRequired] int Id)
        {
            try
            {

                if (Id == 0)
                {
                    return BadRequest(new { data = "", message = "Id is required", statusCode = "400", section = "Get" });
                }

                var model = _context.yemek_kategorileris.FirstOrDefault(w => w.Id == Id);

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


        [HttpPost("Add", Name = "FoodsCategoryAdd")]
        public IActionResult ADD([FromForm] FoodCategoryFormModel values)
        {
            try
            {
                var user = HttpContext.Items["User"] as ikys_user;

                /* var userData = _context.Users.FirstOrDefault(x => x.Id == values.UserId);

                 if (userData == null)
                 {
                     return BadRequest(new { data = "", message = "Error: User Not Found", statusCode = "400", section = "Add" });
                 }*/


                var model = new yemek_kategorileri();
                model.KategoriAdi = values.KategoriAdi;
                model.CreatedDate = DateTime.Now;
                _context.yemek_kategorileris.Add(model);
                _context.SaveChanges();

                return Ok(new { data = "", message = "Success", statusCode = "200", section = "Add" });
            }
            catch (Exception)
            {
                return BadRequest(new { data = "", message = "Error: ", statusCode = "500", section = "Add" });
            }
        }


        [HttpPost("Update", Name = "FoodsCategoryUpdate")]
        public IActionResult Update([FromForm] FoodCategoryFormModel values)
        {
            try
            {

                var user = HttpContext.Items["User"] as ikys_user;

                /* if (values.Id == 0)
                 {
                     return BadRequest(new { data = "", message = "Id is required", statusCode = "400", section = "Update" });
                 }*/

                var model = _context.yemek_kategorileris.FirstOrDefault(w => w.Id == values.Id);

                if (model != null)
                {
                    model.KategoriAdi = values.KategoriAdi;

                    _context.yemek_kategorileris.Update(model);
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


        [HttpPost("Delete", Name = "FoodsCategoryDelete")]
        public IActionResult Delete([BindRequired] int Id)
        {
            try
            {
                if (Id == 0)
                {
                    return BadRequest(new { data = "", message = "Id is required", statusCode = "400", section = "Delete" });
                }
                var model = _context.yemek_kategorileris.FirstOrDefault(w => w.Id == Id);
                if (model != null)
                {

                    model.SilindiMi = true;
                    _context.yemek_kategorileris.Update(model);
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
