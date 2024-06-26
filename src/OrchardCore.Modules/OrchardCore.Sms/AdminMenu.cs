using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.Navigation;
using OrchardCore.Sms.Controllers;

namespace OrchardCore.Sms;

public class AdminMenu : INavigationProvider
{
    protected readonly IStringLocalizer S;

    public AdminMenu(IStringLocalizer<AdminMenu> stringLocalizer)
    {
        S = stringLocalizer;
    }

    public Task BuildNavigationAsync(string name, NavigationBuilder builder)
    {
        if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
        {
            return Task.CompletedTask;
        }

        builder
            .Add(S["Configuration"], configuration => configuration
                .Add(S["Settings"], settings => settings
                    .Add(S["SMS"], S["SMS"].PrefixPosition(), sms => sms
                        .AddClass("sms")
                        .Id("sms")
                        .Action("Index", "Admin", new { area = "OrchardCore.Settings", groupId = SmsSettings.GroupId })
                        .Permission(SmsPermissions.ManageSmsSettings)
                        .LocalNav()
                    )
                    .Add(S["SMS Test"], S["SMS Test"].PrefixPosition(), sms => sms
                        .AddClass("smstest")
                        .Id("smstest")
                        .Action(nameof(AdminController.Test), typeof(AdminController).ControllerName(), new { area = "OrchardCore.Sms" })
                        .Permission(SmsPermissions.ManageSmsSettings)
                        .LocalNav()
                    )
                )
            );

        return Task.CompletedTask;
    }
}
