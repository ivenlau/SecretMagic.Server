using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using  SecretMagic.API.Services;

namespace SecretMagic.API.Authorization
{
    // This class contains logic for determining whether MinimumAgeRequirements in authorizaiton
    // policies are satisfied or not
    public class ProtectedAuthorizationHandler : AuthorizationHandler<ProtectedRequirement>
    {
        private readonly ILogger<ProtectedAuthorizationHandler> logger;
        private readonly Services.IAuthorizationService authorizationService;

        public ProtectedAuthorizationHandler(ILogger<ProtectedAuthorizationHandler> logger, Services.IAuthorizationService authorizationService)
        {
            this.logger = logger;
            this.authorizationService = authorizationService;
        }

        // Check whether a given MinimumAgeRequirement is satisfied or not for a particular context
        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProtectedRequirement requirement)
        {
            // Log as a warning so that it's very clear in sample output which authorization policies 
            // (and requirements/handlers) are in use
            logger.LogWarning("Evaluating authorization requirement for Resource >= {resource}", requirement.Resource);

            // Check the role
            var uidClaim = context.User.FindFirst(c => c.Type == "UID");
            if (uidClaim != null)
            {
                if(!string.IsNullOrEmpty(requirement.Resource))
                {
                    string [] resources;
                    if(requirement.Resource.Contains(','))
                    {
                        resources = requirement.Resource.Split(',');
                    }
                    else
                    {
                        resources = new string[]{ requirement.Resource };
                    }
                    var isAuthorized = await authorizationService.IsAuthorized(uidClaim.Value, resources);
                    if (isAuthorized)
                    {
                        context.Succeed(requirement);
                    }
                }
            }
            else
            {
                logger.LogInformation("No Role claim present");
            }
        }
    }
}