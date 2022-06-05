using System.Security.Claims;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using GeekShopping.IdetityServer.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace GeekShopping.IdentityServer.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManeger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;

        public ProfileService(UserManager<ApplicationUser> userManeger, 
            RoleManager<IdentityRole> roleManager,
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory)
        {
            _userManeger = userManeger;
            _roleManager = roleManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var id = context.Subject.GetSubjectId();
            var user = await _userManeger.FindByIdAsync(id);
            var userClaims = await _userClaimsPrincipalFactory.CreateAsync(user);

            var claims = userClaims.Claims.ToList();
            claims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
            claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));

            if (_userManeger.SupportsUserRole)
            {
                var roles = await _userManeger.GetRolesAsync(user);

                foreach (var role in roles)
                {
                    claims.Add(new Claim(JwtClaimTypes.Role, role));

                    if (_roleManager.SupportsRoleClaims)
                    {
                        var identityRole = await _roleManager.FindByNameAsync(role);

                        if (identityRole != null)
                            claims.AddRange(await _roleManager.GetClaimsAsync(identityRole));
                    }
                }
            }

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var id = context.Subject.GetSubjectId();
            var user = await _userManeger.FindByIdAsync(id);
            context.IsActive = user != null;
        }
    }
}