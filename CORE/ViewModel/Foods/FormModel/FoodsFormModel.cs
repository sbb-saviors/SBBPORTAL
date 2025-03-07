using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.ViewModel.Foods.FormModel
{
    public class FoodsFormModel
    {
        public long? Id { get; set; }

        public string YemekAdi { get; set; } = null!;

        public string? Aciklama { get; set; }

        public DateTime? OlusturmaTarihi { get; set; }

        public bool? SilindiMi { get; set; }

        public IFormFile? Fotograf { get; set; }

        public long? KategoriId { get; set; }
    }
}
