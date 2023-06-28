namespace IdentityServer.Application.Features.Queries.Users.GetCurrentUser;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQueryRequest, ResponseModel<GetCurrentUserQueryResponse>>
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public GetCurrentUserQueryHandler(IMapper mapper, IUserService userService)
    {
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<ResponseModel<GetCurrentUserQueryResponse>> Handle(GetCurrentUserQueryRequest request, CancellationToken cancellationToken)
    {
        User user = await _userService.GetAsync();

        var userMap = _mapper.Map<GetCurrentUserQueryResponse>(user);

        return await ResponseModel<GetCurrentUserQueryResponse>.SuccessAsync(userMap);
    }
}
