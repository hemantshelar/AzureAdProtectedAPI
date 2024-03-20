using Microsoft.AspNetCore.Authorization;

namespace JwtSvcAdGroup.Services;
public class AdminGroupAuthorizationHandler : AuthorizationHandler<AdminGroupAuthorizationRequirement>
{
	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminGroupAuthorizationRequirement requirement)
	{
		if (context.User.Claims.Any(c => c.Type == "groups" && c.Value == requirement.GroupName))
		{
			context.Succeed(requirement);
		}

		return Task.CompletedTask;
	}
}

public class NonAdminGroupAuthorizationHandler : AuthorizationHandler<NonAdminGroupAuthorizationRequirement>
{
	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, NonAdminGroupAuthorizationRequirement requirement)
	{
		if (context.User.Claims.Any(c => c.Type == "groups" && c.Value == requirement.GroupName))
		{
			context.Succeed(requirement);
		}

		return Task.CompletedTask;
	}
}


public class AdminGroupAuthorizationRequirement : IAuthorizationRequirement
{
	public string GroupName { get; }

	public AdminGroupAuthorizationRequirement(string groupName)
	{
		GroupName = groupName;
	}
}

public class NonAdminGroupAuthorizationRequirement : IAuthorizationRequirement
{
	public string GroupName { get; }

	public NonAdminGroupAuthorizationRequirement(string groupName)
	{
		GroupName = groupName;
	}
}
