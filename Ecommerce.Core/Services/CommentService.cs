using AutoMapper;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CommentService : ICommentService
{
    private readonly IGenericRepository<Comment> _commentRepository;
    private readonly IMapper _mapper;
    private readonly AhoCorasickAutomaton _ahoCorasick;

    public CommentService(IMapper mapper, IGenericRepository<Comment> commentRepository, IEnumerable<string> bannedWords)
    {
        _mapper = mapper;
        _commentRepository = commentRepository;
        _ahoCorasick = new AhoCorasickAutomaton(bannedWords);
    }

    public async Task<CommentDto> AddCommentAsync(CreateCommentDto createCommentDto)
    {
        createCommentDto.Content = _ahoCorasick.FilterComment(createCommentDto.Content);

        var comment = _mapper.Map<Comment>(createCommentDto);
        await _commentRepository.AddAsync(comment);
        await _commentRepository.SaveAsync();
        return _mapper.Map<CommentDto>(comment);
    }

    public async Task<CommentDto> GetCommentByIdAsync(int id)
    {
        var comment = await _commentRepository.GetByIdAsync(id);
        return _mapper.Map<CommentDto>(comment);
    }

    public async Task<IEnumerable<CommentDto>> GetCommentsByProductIdAsync(int productId)
    {
        var comments = await _commentRepository.GetAllAsync(c => c.ProductId == productId);
        return _mapper.Map<IEnumerable<CommentDto>>(comments);
    }

    public async Task<CommentDto> UpdateCommentAsync(int id, UpdateCommentDto updateCommentDto)
    {
        var comment = await _commentRepository.GetByIdAsync(id);
        if (comment == null)
        {
            return null;
        }

        updateCommentDto.Content = _ahoCorasick.FilterComment(updateCommentDto.Content);

        _mapper.Map(updateCommentDto, comment);
        await _commentRepository.SaveAsync();
        return _mapper.Map<CommentDto>(comment);
    }

    public async Task<bool> DeleteCommentAsync(int id)
    {
        var comment = await _commentRepository.GetByIdAsync(id);
        if (comment == null)
        {
            return false;
        }

        await _commentRepository.DeleteAsync(comment);
        await _commentRepository.SaveAsync();
        return true;
    }
}
