using Microsoft.AspNetCore.Authorization;

namespace JwtSvcAdGroup.Services;
public class GroupAuthorizationHandler : AuthorizationHandler<GroupAuthorizationRequirement>
{
	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GroupAuthorizationRequirement requirement)
	{
		if (context.User.Claims.Any(c => c.Type == "groups" && c.Value == requirement.GroupName))
		{
			context.Succeed(requirement);
		}

		return Task.CompletedTask;
	}
}


public class GroupAuthorizationRequirement : IAuthorizationRequirement
{
	public string GroupName { get; }

	public GroupAuthorizationRequirement(string groupName)
	{
		GroupName = groupName;
	}
}
