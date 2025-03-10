using API.Helpers;
using CORE.Models;
using CORE.ViewModel.Gallery.FormModel;
using CORE.ViewModel.News.FormModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq.Expressions;

namespace API.Controllers.Gallery
{
    [Route("api/[controller]")]
    [ApiController]
    public class GalleryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GalleryController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost("List", Name = "GalleryList")]
        public IActionResult List(int status = 0, string galeriId = "")
        {
            try
            {
                IQueryable<galeri> model = _context.galeris.Where(w => (status == 0 ? w.SilindiMi == false : 1 == 1) && w.GaleriId == galeriId);



                var searchFields = new Dictionary<string, Expression<Func<galeri, string>>>{};

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


        [HttpGet("Get", Name = "GalleryGet")]
        public IActionResult Get([BindRequired] int Id)
        {
            try
            {

                if (Id == 0)
                {
                    return BadRequest(new { data = "", message = "Id is required", statusCode = "400", section = "Get" });
                }

                var model = _context.galeris.FirstOrDefault(w => w.Id == Id);

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


        [HttpPost("Add", Name = "GalleryAdd")]
        public async Task<IActionResult> AddAsync([FromForm] GalleryFormModel values)
        {
            try
            {
                var user = HttpContext.Items["User"] as ikys_user;


                if (values.file == null)
                {
                    return BadRequest(new { data = "", message = "File is required", statusCode = "400", section = "Add" });
                }
                var modelDocs = new galeri();
                modelDocs.GaleriId = values.GaleriId;
                modelDocs.Fotograf = await FileHelper.UploadFileToCDN(values.file, Guid.NewGuid().ToString(), "SbbPortalHaberlerGaleri");
                _context.galeris.Add(modelDocs);
                _context.SaveChanges();

                /*if (values.Fotograf?.Count() > 0)
                {
                    foreach (var item in values.Fotograf)
                    {
                        var modelDocs = new galeri();
                        modelDocs.GaleriId = values.GaleriId;
                        modelDocs.Fotograf = await FileHelper.UploadFileToCDN(item, Guid.NewGuid().ToString(), "SbbPortalHaberlerGaleri");
                        _context.galeris.Add(modelDocs);
                        _context.SaveChanges();
                    }
                }*/

                return Ok(new { data = "", message = "Success", statusCode = "200", section = "Add" });
            }
            catch (Exception)
            {
                return BadRequest(new { data = "", message = "Error: ", statusCode = "500", section = "Add" });
            }
        }


        [HttpPost("Delete", Name = "GalleryDelete")]
        public IActionResult Delete([BindRequired] int Id)
        {
            try
            {
                if (Id == 0)
                {
                    return BadRequest(new { data = "", message = "Id is required", statusCode = "400", section = "Delete" });
                }
                var model = _context.galeris.FirstOrDefault(w => w.Id == Id);
                if (model != null)
                {

                    model.SilindiMi = true;
                    _context.galeris.Update(model);
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
