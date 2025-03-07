using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.ViewModel.FoodsDate.FormModel
{
    public class FoodDateFormModel
    {
        public long Id { get; set; }

        public DateTime? Tarih { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool? SilindiMi { get; set; }
    }


    public class FoodDateFoodFormModel
    {
        public long TarihId { get; set; }

        public List<long>? FoodId { get; set; }
    }
}
