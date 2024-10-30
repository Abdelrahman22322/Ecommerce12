using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Infrastructure.DbContext;

public class TokenCleanupService : ITokenCleanupService
{
    private readonly ApplicationDbContext _context;

    public TokenCleanupService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CleanupExpiredTokensAsync()
    {
        var expiredTokens = _context.PasswordResetTokens
            .Where(t => t.ExpiryDate < DateTime.UtcNow);

        _context.PasswordResetTokens.RemoveRange(expiredTokens);
        await _context.SaveChangesAsync();
    }
}