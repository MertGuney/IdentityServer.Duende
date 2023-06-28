namespace IdentityServer.Application.Common.Mapping;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<User, GetCurrentUserQueryResponse>();
    }
}
