using Microsoft.AspNetCore.Http;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Users.Models;

namespace OrchardCore.Users.Drivers;

public sealed class UserMenuDisplayDriver : DisplayDriver<UserMenu>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserMenuDisplayDriver(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override Task<IDisplayResult> DisplayAsync(UserMenu model, BuildDisplayContext context)
    {
        var results = new List<IDisplayResult>
        {
            View("UserMenuItems__Title", model)
            .Location("Detail", "Header:5")
            .Location("DetailAdmin", "Header:5")
            .Differentiator("Title"),

            View("UserMenuItems__SignedUser", model)
            .Location("DetailAdmin", "Content:1")
            .Differentiator("SignedUser"),

            View("UserMenuItems__Profile", model)
            .Location("Detail", "Content:5")
            .Location("DetailAdmin", "Content:5")
            .Differentiator("Profile"),

            View("UserMenuItems__SignOut", model)
            .Location("Detail", "Content:100")
            .Location("DetailAdmin", "Content:100")
            .Differentiator("SignOut"),
        };

        if (_httpContextAccessor.HttpContext.User.HasClaim("Permission", "AccessAdminPanel"))
        {
            results.Add(View("UserMenuItems__Dashboard", model)
                .Location("Detail", "Content:1.1")
                .Differentiator("Dashboard"));
        }

        return CombineAsync(results);
    }
}
