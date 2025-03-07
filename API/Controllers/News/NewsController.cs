using API.Helpers;
using CORE.Models;
using CORE.ViewModel.Foods.FormModel;
using CORE.ViewModel.News.FormModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq.Expressions;

namespace API.Controllers.News
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NewsController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost("List", Name = "NewsList")]
        public IActionResult List(int status = 0)
        {
            try
            {
                IQueryable<haberler> model = _context.haberlers.Where(w => (status == 0 ? w.SilindiMi == false : 1 == 1));



                var searchFields = new Dictionary<string, Expression<Func<haberler, string>>>
                {
                    { "Baslik", item => (item.Baslik ?? "").ToLower() ?? "" },
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


        [HttpGet("Get", Name = "NewsGet")]
        public IActionResult Get([BindRequired] int Id)
        {
            try
            {

                if (Id == 0)
                {
                    return BadRequest(new { data = "", message = "Id is required", statusCode = "400", section = "Get" });
                }

                var model = _context.haberlers.FirstOrDefault(w => w.Id == Id);

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


        [HttpPost("Add", Name = "NewsAdd")]
        public async Task<IActionResult> AddAsync([FromForm] NewsFormModel values)
        {
            try
            {
                var user = HttpContext.Items["User"] as ikys_user;


                var model = new haberler();
                model.Baslik = values.Baslik;
                model.Aciklama = values.Aciklama;
                model.Icerik = values.Icerik;
                model.CreatedDate = DateTime.Now;
                model.GaleriId = Guid.NewGuid().ToString();

                if (values.Kapak != null)
                {
                    model.Kapak = await FileHelper.UploadFileToCDN(values.Kapak, Guid.NewGuid().ToString(), "SbbPortalHaberlerGaleri");
                }
                _context.haberlers.Add(model);
                _context.SaveChanges();

                return Ok(new { data = "", message = "Success", statusCode = "200", section = "Add" });
            }
            catch (Exception)
            {
                return BadRequest(new { data = "", message = "Error: ", statusCode = "500", section = "Add" });
            }
        }


        [HttpPost("Update", Name = "NewsUpdate")]
        public async Task<IActionResult> Update([FromForm] NewsFormModel values)
        {
            try
            {

                var user = HttpContext.Items["User"] as ikys_user;

                if (values.Id == 0)
                 {
                     return BadRequest(new { data = "", message = "Id is required", statusCode = "400", section = "Update" });
                 }

                var model = _context.haberlers.FirstOrDefault(w => w.Id == values.Id);

                if (model != null)
                {
                    model.Baslik = values.Baslik;
                    model.Aciklama = values.Aciklama;
                    model.Icerik = values.Icerik;


                    if (values.Kapak != null)
                    {
                        model.Kapak = await FileHelper.UploadFileToCDN(values.Kapak, Guid.NewGuid().ToString(), "SbbPortalYemekler");
                    }
                    _context.haberlers.Update(model);
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


        [HttpPost("Delete", Name = "NewsDelete")]
        public IActionResult Delete([BindRequired] int Id)
        {
            try
            {
                if (Id == 0)
                {
                    return BadRequest(new { data = "", message = "Id is required", statusCode = "400", section = "Delete" });
                }
                var model = _context.haberlers.FirstOrDefault(w => w.Id == Id);
                if (model != null)
                {

                    model.SilindiMi = true;
                    _context.haberlers.Update(model);
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
