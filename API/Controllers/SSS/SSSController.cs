using API.Helpers;
using CORE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult List()
        {
            try
            {
                var adasd = Request.Headers["status"].FirstOrDefault();
                IQueryable<sikca_sorulan_sorular> model = _context.sikca_sorulan_sorulars;



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
    }
}
