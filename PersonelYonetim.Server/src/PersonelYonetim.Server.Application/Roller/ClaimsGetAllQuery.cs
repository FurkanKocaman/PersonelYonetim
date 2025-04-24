using MediatR;
using PersonelYonetim.Server.Domain.RoleClaim;

namespace PersonelYonetim.Server.Application.Roller;
public sealed record ClaimsGetAllQuery(
    ) :IRequest<List<ClaimsGetAllQueryResponse>>;

public sealed class ClaimsGetAllQueryResponse
{
    public string Label { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}


internal sealed class ClaimsGetAllQueryHandler(
    ) : IRequestHandler<ClaimsGetAllQuery, List<ClaimsGetAllQueryResponse>>
{
    public Task<List<ClaimsGetAllQueryResponse>> Handle(ClaimsGetAllQuery request, CancellationToken cancellationToken)
    {
        var allPermissions = new List<string>
        {
            Permissions.ViewPersonel, Permissions.CreatePersonel, Permissions.EditPersonel, Permissions.DeletePersonel,
            Permissions.ViewKurumsalYapi, Permissions.CreateKurumsalYapi, Permissions.EditKurumsalYapi, Permissions.DeleteKurumsalYapi,
            Permissions.ViewIzinler, Permissions.CreateIzinler, Permissions.ApproveIzinler,
            Permissions.ViewRaporlar
        };
        List<ClaimsGetAllQueryResponse> claims = new List<ClaimsGetAllQueryResponse >();

        foreach(var permission in allPermissions)
        {
            ClaimsGetAllQueryResponse claim = new()
            {
                Label = permission,
                Value = permission
            };
            claims.Add(claim);
        }

        return Task.FromResult(claims);
    }
}
