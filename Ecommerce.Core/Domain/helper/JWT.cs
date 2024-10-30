namespace Ecommerce.Core.Domain.helper;

public class JWT
{

    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int DurationInMinutes { get; set; }


}

public class Twilio
{
    public string AccountSid { get; set; }
    public string AuthToken { get; set; }
    public string PhoneNumber { get; set; }

}