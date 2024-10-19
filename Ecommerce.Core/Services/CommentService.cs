using AutoMapper;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

public class CommentService : ICommentService
{
    private readonly IGenericRepository<Comment> _commentRepository;
    private readonly IMapper _mapper;
    private readonly AhoCorasickAutomaton _ahoCorasick;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CommentService(IMapper mapper, IGenericRepository<Comment> commentRepository, IEnumerable<string> bannedWords, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _commentRepository = commentRepository;
        _ahoCorasick = new AhoCorasickAutomaton(bannedWords);
        _httpContextAccessor = httpContextAccessor;
    }

    private int GetUserIdFromContext()
    {
        var user = _httpContextAccessor.HttpContext?.User
                   ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "uid")
                          ?? throw new UnauthorizedAccessException("User is not authorized.");

        return int.Parse(userIdClaim.Value);
    }

    public async Task<CommentDto> AddCommentAsync(CreateCommentDto createCommentDto)
    {
        var userId = GetUserIdFromContext();
      //  createCommentDto.UserId = userId;
        createCommentDto.Content = _ahoCorasick.FilterComment(createCommentDto.Content);

        var comment = _mapper.Map<Comment>(createCommentDto);
        comment.UserId = userId;
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

    public async Task<CommentDto> UpdateCommentAsync( UpdateCommentDto updateCommentDto)
    {
        var userId = GetUserIdFromContext();
        var comment = await _commentRepository.GetByIdAsync(userId);
        if (comment == null || comment.UserId != userId)
        {
            return null;
        }

        updateCommentDto.Content = _ahoCorasick.FilterComment(updateCommentDto.Content);    

        _mapper.Map(updateCommentDto, comment);
        await _commentRepository.UpdateAsync(comment);

        await _commentRepository.SaveAsync();
        return _mapper.Map<CommentDto>(comment);
    }

    public async Task<bool> DeleteCommentAsync()
    {
        var userId = GetUserIdFromContext();
        var comment = await _commentRepository.GetByIdAsync(userId);
        if (comment == null || comment.UserId != userId)
        {
            return false;
        }

        await _commentRepository.DeleteAsync(comment);
        await _commentRepository.SaveAsync();
        return true;
    }
}
