using System.Text.Json.Nodes;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;

namespace OrchardCore.ContentFields.Settings;

public sealed class YoutubeFieldSettingsDriver : ContentPartFieldDefinitionDisplayDriver<YoutubeField>
{
    public override IDisplayResult Edit(ContentPartFieldDefinition partFieldDefinition, BuildEditorContext context)
    {
        return Initialize<YoutubeFieldSettings>("YoutubeFieldSetting_Edit", model =>
        {
            var settings = partFieldDefinition.Settings.ToObject<YoutubeFieldSettings>();

            model.Height = model.Height != default ? model.Height : 315;
            model.Width = model.Width != default ? model.Width : 560;
        }).Location("Content");
    }

    public async override Task<IDisplayResult> UpdateAsync(ContentPartFieldDefinition partFieldDefinition, UpdatePartFieldEditorContext context)
    {
        var model = new YoutubeFieldSettings();
        await context.Updater.TryUpdateModelAsync(model, Prefix);

        context.Builder.WithSettings(model);

        return Edit(partFieldDefinition, context);
    }
}
