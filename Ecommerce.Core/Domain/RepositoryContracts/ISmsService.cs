using Twilio.Rest.Api.V2010.Account;

namespace Ecommerce.Core.Domain.RepositoryContracts;

public interface ISmsService
{
    MessageResource SendSmsAsync(string phoneNumber, string body);
}