using Microsoft.AspNetCore.Authorization;

namespace Application.Api.Controllers._Shared;

[Authorize]
public class BaseAuthorizationController() : BaseApplicationController()
{
}

