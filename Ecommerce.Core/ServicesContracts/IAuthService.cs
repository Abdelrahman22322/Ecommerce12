﻿using Ecommerce.Core.Domain.RepositoryContracts;

namespace Ecommerce.Core.ServicesContracts;

public interface IAuthService
{
    public Task<AuthModel> RegisterAsync(RegisterDto model);

    public Task<AuthModel> LoginAsync(LoginModel model);

    Task<string> AddRoleAsync(AddRoleDto model);
}