﻿namespace Ecommerce.Core.Domain.RepositoryContracts;

public class CreateCommentDto
{
    public int ProductId { get; set; }
  
    public string Content { get; set; }
}