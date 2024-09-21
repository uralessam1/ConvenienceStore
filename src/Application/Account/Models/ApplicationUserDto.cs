﻿using ConvenienceStoreApi.Domain.Entities;

namespace ConvenienceStoreApi.Application.Security.Models;

public class ApplicationUserDto
{
    public string UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public bool IsActive { get; set; }
    public string? Role { get; set; }
}