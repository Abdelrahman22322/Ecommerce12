using Ecommerce.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Ecommerce.Infrastructure.Repository;

public class SmsService(IOptions<Core.Domain.helper.Twilio> twilio) : ISmsService
{
    private readonly Core.Domain.helper.Twilio _twilio = twilio.Value;

    public MessageResource SendSmsAsync(string phoneNumber, string body)
    {
        TwilioClient.Init(_twilio.AccountSid,_twilio.AuthToken);
        return MessageResource
            .Create(
                body: body,
                from: new Twilio.Types.PhoneNumber(_twilio.PhoneNumber),
                to: new Twilio.Types.PhoneNumber(phoneNumber)
            );

    }
}