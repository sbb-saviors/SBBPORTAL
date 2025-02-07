using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.ViewModel.DiscountDeals.FormModel
{
    public class DiscountDealsFormModel
    {
        public long? Id { get; set; }

        public string? Kategori { get; set; }

        public string? Baslik { get; set; }

        public string? Aciklama { get; set; }

        public DateTime? OlusturmaTarihi { get; set; }

        public bool? SilindiMi { get; set; }
    }
}
