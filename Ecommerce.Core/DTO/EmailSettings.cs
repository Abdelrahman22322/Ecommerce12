﻿namespace Ecommerce.Core.Domain.RepositoryContracts;

public class EmailSettings
{

    public string FromEmail { get; set; }
    public string SmtpServer { get; set; }
    public string Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}