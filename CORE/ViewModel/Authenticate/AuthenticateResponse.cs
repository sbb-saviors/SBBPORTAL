
using CORE.Models;

namespace CORE.ViewModels.Authenticate
{
    public class AuthenticateResponse
    {
        public long? Id { get; set; }
        public string? DisplayName { get; set; }
        public string? Username { get; set; }
        public string? Token { get; set; }
        public string? Role { get; set; }
        public string? Guid { get; set; }
        public string? Fotograf { get; set; }

        public AuthenticateResponse(ikys_user user, string jwtToken)
        {
            Id = user.Id;
            DisplayName = user.DisplayName;
            Username = user.UserName;
            Token = jwtToken;
            Role = user.Role;
            Guid = user.Guid;
            Fotograf = user.Fotograf;
        }
    }
}
