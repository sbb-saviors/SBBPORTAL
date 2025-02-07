using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.ViewModel.SSS.FormModel
{
    public class SssFormModel
    {
        public long? Id { get; set; }

        public string? SoruBaslik { get; set; }

        public string? SoruCevap { get; set; }

        public DateTime? OlusturmaTarihi { get; set; }
    }
}
