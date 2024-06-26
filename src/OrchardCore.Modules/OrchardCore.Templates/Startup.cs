using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.Admin;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.Deployment;
using OrchardCore.DisplayManagement;
using OrchardCore.Modules;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.Navigation;
using OrchardCore.Recipes;
using OrchardCore.ResourceManagement;
using OrchardCore.Security.Permissions;
using OrchardCore.Templates.Controllers;
using OrchardCore.Templates.Deployment;
using OrchardCore.Templates.Recipes;
using OrchardCore.Templates.Services;
using OrchardCore.Templates.Settings;

namespace OrchardCore.Templates
{
    public class Startup : StartupBase
    {
        private readonly AdminOptions _adminOptions;

        public Startup(IOptions<AdminOptions> adminOptions)
        {
            _adminOptions = adminOptions.Value;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<ResourceManagementOptions>, ResourceManagementOptionsConfiguration>();

            services.AddScoped<IShapeBindingResolver, TemplatesShapeBindingResolver>();
            services.AddScoped<PreviewTemplatesProvider>();
            services.AddScoped<TemplatesManager>();
            services.AddScoped<IPermissionProvider, Permissions>();
            services.AddScoped<INavigationProvider, AdminMenu>();
            services.AddRecipeExecutionStep<TemplateStep>();

            // Template shortcuts in settings
            services.AddScoped<IContentPartDefinitionDisplayDriver, TemplateContentPartDefinitionDriver>();
            services.AddScoped<IContentTypeDefinitionDisplayDriver, TemplateContentTypeDefinitionDriver>();
            services.AddScoped<IContentTypePartDefinitionDisplayDriver, TemplateContentTypePartDefinitionDriver>();

            services.AddDeployment<AllTemplatesDeploymentSource, AllTemplatesDeploymentStep, AllTemplatesDeploymentStepDriver>();

            services.AddScoped<AdminTemplatesManager>();
            services.AddScoped<IPermissionProvider, AdminTemplatesPermissions>();
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            var templateControllerName = typeof(TemplateController).ControllerName();

            routes.MapAreaControllerRoute(
                name: "Templates.Index",
                areaName: "OrchardCore.Templates",
                pattern: _adminOptions.AdminUrlPrefix + "/Templates",
                defaults: new { controller = templateControllerName, action = nameof(TemplateController.Index) }
            );

            routes.MapAreaControllerRoute(
                name: "Templates.Admin",
                areaName: "OrchardCore.Templates",
                pattern: _adminOptions.AdminUrlPrefix + "/Templates/Admin",
                defaults: new { controller = templateControllerName, action = nameof(TemplateController.Admin) }
            );

            routes.MapAreaControllerRoute(
                name: "Templates.Create",
                areaName: "OrchardCore.Templates",
                pattern: _adminOptions.AdminUrlPrefix + "/Templates/Create",
                defaults: new { controller = templateControllerName, action = nameof(TemplateController.Create) }
            );

            routes.MapAreaControllerRoute(
                name: "Templates.Edit",
                areaName: "OrchardCore.Templates",
                pattern: _adminOptions.AdminUrlPrefix + "/Templates/Edit/{name}",
                defaults: new { controller = templateControllerName, action = nameof(TemplateController.Edit) }
            );
        }
    }

    [Feature("OrchardCore.AdminTemplates")]
    public class AdminTemplatesStartup : StartupBase
    {
        private readonly AdminOptions _adminOptions;

        public AdminTemplatesStartup(IOptions<AdminOptions> adminOptions)
        {
            _adminOptions = adminOptions.Value;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IShapeBindingResolver, AdminTemplatesShapeBindingResolver>();
            services.AddScoped<AdminPreviewTemplatesProvider>();
            services.AddScoped<INavigationProvider, AdminTemplatesAdminMenu>();
            services.AddRecipeExecutionStep<AdminTemplateStep>();
            services.AddDeployment<AllAdminTemplatesDeploymentSource, AllAdminTemplatesDeploymentStep, AllAdminTemplatesDeploymentStepDriver>();
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaControllerRoute(
                name: "Templates.Admin",
                areaName: "OrchardCore.Templates",
                pattern: _adminOptions.AdminUrlPrefix + "/Templates/Admin",
                defaults: new { controller = typeof(TemplateController).ControllerName(), action = nameof(TemplateController.Admin) }
            );
        }
    }
}
