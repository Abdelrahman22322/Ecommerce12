﻿using Ecommerce.Core.Domain.RepositoryContracts;

namespace Ecommerce.Core.ServicesContracts;

public interface ICommentService
{
    Task<CommentDto> AddCommentAsync(CreateCommentDto createCommentDto);
    Task<CommentDto> GetCommentByIdAsync(int id);
    Task<IEnumerable<CommentDto>> GetCommentsByProductIdAsync(int productId);
    Task<CommentDto> UpdateCommentAsync(UpdateCommentDto updateCommentDto);
    Task<bool> DeleteCommentAsync();
}