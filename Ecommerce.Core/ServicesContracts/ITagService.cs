using System.Linq.Expressions;
using Ecommerce.Core.Domain.Entities;

namespace Ecommerce.Core.ServicesContracts;

public interface ITagService
{
    Task<Tag?> DetermineTagAsync(string tagName);
    public Task AddAsync(Tag? tag);


    Task<Tag> UpdateAsync(int tagId, string tagName);

    Task<Tag> DeleteAsync(int tagId);
    // Task AddAsync(ProductTag productTag);
    Task<IEnumerable<Tag>> FindAsync(Expression<Func<Tag, bool>?> func);
    Task<Tag?> FindAsync1(Expression<Func<Tag, bool>?> func);
}

