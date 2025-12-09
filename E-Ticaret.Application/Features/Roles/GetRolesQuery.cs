using E_Ticaret.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Ticaret.Application.Features.Roles;
    public sealed record GetRolesQuery:IRequest<List<GetRolesResponse>>;

public sealed record GetRolesResponse(Guid Id, string Name);
internal sealed class GetRolesQueryHandler(IRoleRepository roleRepository ) : IRequestHandler<GetRolesQuery, List<GetRolesResponse>>
{


    public async Task<List<GetRolesResponse>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var response = await roleRepository.GetAll().Select(p => new GetRolesResponse(p.Id, p.Name!)).ToListAsync(cancellationToken);

        return response;
    }
}