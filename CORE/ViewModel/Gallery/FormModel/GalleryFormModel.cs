using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.ViewModel.Gallery.FormModel
{
    public class GalleryFormModel
    {
        public long Id { get; set; }

        public string? GaleriId { get; set; }

        public List<IFormFile>? Fotograf { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool? SilindiMi { get; set; }
    }
}
