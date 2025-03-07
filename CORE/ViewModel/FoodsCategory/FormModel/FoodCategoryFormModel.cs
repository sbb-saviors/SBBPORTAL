using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.ViewModel.FoodsCategory.FormModel
{
    public class FoodCategoryFormModel
    {
        public long Id { get; set; }

        public string? KategoriAdi { get; set; }

        public bool? SilindiMi { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
