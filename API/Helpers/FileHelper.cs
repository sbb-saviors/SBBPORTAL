using System.Net.Http.Headers;
using Newtonsoft.Json;

public static class FileHelper
{
    public static async Task<string> UploadFileToCDN(IFormFile file, string guid, string klasorYolu)
    {
        if (file == null || file.Length == 0)
            return null!;

        using (var client = new HttpClient())
        {

            var fileName = $"{guid}{Path.GetExtension(file.FileName)}";

            var uploadUrl = "https://cdn.samsun.bel.tr/upload.php";

            using (var content = new MultipartFormDataContent())
            {
                // Dosyayı ekle
                var fileContent = new StreamContent(file.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                content.Add(fileContent, "file", fileName);

                content.Add(new StringContent(klasorYolu), "path");

                try
                {
                    var response = await client.PostAsync(uploadUrl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<CdnResponse>(jsonResponse);

                        if (result != null && result.Status == "success")
                        {
                            return result.Path!;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("CDN yükleme hatası: " + ex.Message);
                }
            }
        }
        return null!;
    }

    public static async Task<string> ToBase64(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return null!;

        using (var memoryStream = new MemoryStream())
        {
            // Dosyayı belleğe kopyala
            await file.CopyToAsync(memoryStream);

            // Bellekten byte dizisini al
            byte[] fileBytes = memoryStream.ToArray();

            // Dosyanın MIME türünü belirle
            string mimeType = GetMimeType(file.FileName);

            // MIME türü ile Base64 formatını birleştir
            return $"data:{mimeType};base64,{Convert.ToBase64String(fileBytes)}";
        }
    }

    private static string GetMimeType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLower();

        return extension switch
        {
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            ".pdf" => "application/pdf",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            _ => "application/octet-stream", // Genel tür, bilinmeyen dosya türleri için
        };
    }


    public class CdnResponse
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
        public string? Path { get; set; }
    }
}
