using Newtonsoft.Json;

namespace Ecommerce.Core.Domain.RepositoryContracts;

public class AuthModel
{
    public string Message { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public bool IsAuthentecated { get; set; }
    public List<string> Roles { get; set; }
    public string Token { get; set; }

    public DateTime ExpireAt { get; set; }
    [JsonIgnore]
    public string RefreshToken { get; set; }

    public DateTime RefreshTokenExpireAt { get; set; }
}