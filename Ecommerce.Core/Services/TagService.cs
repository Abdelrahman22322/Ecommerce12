using System.Linq.Expressions;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;

namespace Ecommerce.Core.Services;

public class TagService : ITagService
{
    private readonly IGenericRepository<Tag?> _tagRepository;

    public TagService(IGenericRepository<Tag?> tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<Tag?> DetermineTagAsync(string tagName)
    {
        var existingTags = await _tagRepository.FindAsync(x => x.Name == tagName);
        var existingTag = existingTags?.FirstOrDefault();
        if (existingTag != null)
        {
            return existingTag;
        }

        var newTag = new Tag
        {
            Name = tagName
        };

        await _tagRepository.AddAsync(newTag);
        await _tagRepository.SaveAsync(); // Added SaveAsync to ensure the tag is saved in the database
        return newTag;
    }

    public async Task<Tag> UpdateAsync(int tagId, string tagName)
    {

        var tag = await _tagRepository.GetByIdAsync(tagId);
        if (tag == null)
        {
            throw new Exception("Tag not found");
        }

        tag.Name = tagName;
        await _tagRepository.UpdateAsync(tag);
        return tag;
    }

    public async Task<Tag> DeleteAsync(int tagId)
    {

        var tag = await _tagRepository.GetByIdAsync(tagId);
        if (tag == null)
        {
            throw new Exception("Tag not found");
        }

        await _tagRepository.DeleteAsync(tag);
        return tag;
    }

    public async Task<IEnumerable<Tag>> FindAsync(Expression<Func<Tag, bool>?> func)
    {
       return await _tagRepository.FindAsync(func);
    }

    public async Task<Tag?> FindAsync1(Expression<Func<Tag, bool>?> func)
    {

        return await _tagRepository.FindAsync1(func);
    }

    public async Task AddAsync(Tag? tag)
    {

        await _tagRepository.AddAsync(tag);
        await _tagRepository.SaveAsync();

    }
}



 
