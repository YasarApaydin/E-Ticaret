namespace E_Ticaret.Domain.Common
{
    public sealed record UserListDto(
     Guid Id,
     string FullName,
     string Email,
     bool EmailConfirmed,
     bool IsDeleted,
     bool IsLocked,
     DateTime CreatedAt,
     int OrdersCount
 );
}
