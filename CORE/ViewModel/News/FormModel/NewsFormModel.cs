using Microsoft.AspNetCore.Http;

namespace CORE.ViewModel.News.FormModel
{
    public class NewsFormModel
    {
        public long? Id { get; set; }

        public string? Baslik { get; set; }

        public string? Aciklama { get; set; }

        public string? Icerik { get; set; }

        public string? GaleriId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool? SilindiMi { get; set; }

        public IFormFile? Kapak { get; set; }
    }
}
