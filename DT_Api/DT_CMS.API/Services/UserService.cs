using System.Data;
using Dapper;
using DT_CMS.API.Data;
using DT_CMS.Core.DTOs.Common;
using DT_CMS.Core.DTOs.Users;
using DT_CMS.Core.Entities;
using DT_CMS.Core.Exceptions;
using DT_CMS.Core.Interfaces.Services;
using DT_CMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DT_CMS.API.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _db;
    private readonly DapperHelper _dapper;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(ApplicationDbContext db, DapperHelper dapper, IPasswordHasher passwordHasher)
    {
        _db = db;
        _dapper = dapper;
        _passwordHasher = passwordHasher;
    }

    /// <summary>Read-side: list/search always goes through dbo.sp_GetUsers_Search via Dapper.</summary>
    public async Task<PagedResultDto<UserDto>> GetUsersAsync(QueryParamsDto query)
    {
        using var connection = _dapper.CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@Search", query.Search, DbType.String);
        parameters.Add("@Page", query.Page, DbType.Int32);
        parameters.Add("@PageSize", query.PageSize, DbType.Int32);

        var command = new CommandDefinition("dbo.sp_GetUsers_Search", parameters, commandType: CommandType.StoredProcedure);

        using var multi = await connection.QueryMultipleAsync(command);
        var totalCount = await multi.ReadFirstAsync<int>();
        var items = (await multi.ReadAsync<UserDto>()).AsList();

        return new PagedResultDto<UserDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<UserDto> GetUserByIdAsync(int id)
    {
        var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id)
            ?? throw new NotFoundException("User", id);
        return ToDto(user);
    }

    /// <summary>Write-side: Create/Update/Delete always go through EF Core + LINQ.</summary>
    public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
    {
        if (await _db.Users.AsNoTracking().AnyAsync(u => u.Username == dto.Username))
            throw new AppException($"Tên tài khoản '{dto.Username}' đã tồn tại.");

        if (await _db.Users.AsNoTracking().AnyAsync(u => u.Email == dto.Email))
            throw new AppException($"Email '{dto.Email}' đã tồn tại.");

        var user = new User
        {
            Username = dto.Username,
            PasswordHash = _passwordHasher.Hash(dto.Password),
            FullName = dto.FullName,
            Email = dto.Email,
            IsActive = dto.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return ToDto(user);
    }

    public async Task<UserDto> UpdateUserAsync(int id, UpdateUserDto dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id)
            ?? throw new NotFoundException("User", id);

        user.FullName = dto.FullName;
        user.Email = dto.Email;
        user.IsActive = dto.IsActive;
        user.UpdatedAt = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(dto.Password))
            user.PasswordHash = _passwordHasher.Hash(dto.Password);

        await _db.SaveChangesAsync();

        return ToDto(user);
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id)
            ?? throw new NotFoundException("User", id);

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
    }

    private static UserDto ToDto(User user) => new()
    {
        Id = user.Id,
        Username = user.Username,
        FullName = user.FullName,
        Email = user.Email,
        IsActive = user.IsActive,
        CreatedAt = user.CreatedAt
    };
}
