UCreate [![Build status](https://ci.appveyor.com/api/projects/status/60v4v2cbl6nxmf0q)](https://ci.appveyor.com/project/nicbell/ucreate)
=======

Create DocTypes, DataTypes and MediaTypes for Umbraco 7 using code-first approach. Inspired by [USiteBuilder](https://github.com/spopovic/uSiteBuilder).

Available on NuGet
---
```
PM> Install-Package UCreate
```

Usage
---

Adding HTTP module to ```web.config```

```xml
...
<system.web>
	<httpModules>
		...
		<add name="CmsSyncHttpModule" type="NicBell.UCreate.CmsSyncHttpModule, NicBell.UCreate" />
	</httpModules>
</system.web>
...
<system.webServer>
	...
	<modules runAllManagedModulesForAllRequests="true">
		...
		<add name="CmsSyncHttpModule" type="NicBell.UCreate.CmsSyncHttpModule, NicBell.UCreate" />
	</modules>
	...
</system.webServer>
...
```
DocType exmple
---
```csharp
[DocType(Name = "Page With Title",
    Icon = "icon-zip color-blue",
    AllowedAsRoot = true,
    AllowedTemplates = new[] { "PageWithTitle" },
    DefaultTemplate = "PageWithTitle")]
public class PageWithTitle : BaseDocType
{
    [Property(Alias = "heading", TypeName = PropertyTypes.Textstring, Description = "Heading for page", Mandatory = true, TabName = "Content")]
    public string Heading { get; set; }
}
```

DataType with prevalues example
---
```csharp
[DataType(EditorAlias = Umbraco.Core.Constants.PropertyEditors.ColorPickerAlias,
    Name = "Nice Color Picker",
    Key = "1bfca1e7-95d0-485e-bd94-9fe9c2b8821f",
    DBType = DataTypeDatabaseType.Nvarchar)]
public class NiceColorPicker : IHasPreValues
{
    /// <summary>
    /// Implementing PreValues
    /// </summary>
    public IDictionary<string, PreValue> PreValues
    {
        get
        {
            return new Dictionary<string, PreValue> {
                {"1", new PreValue("ff00ff")},
                {"2", new PreValue("1f00f1")},
                {"3", new PreValue("123123")},
                {"4", new PreValue("ffffff")}
            };
        }
    }
}
```

MediaType example
---
```csharp
[MediaType(Name = "Folder With Cover",
    Icon = "icon-folder color-blue",
    AllowedAsRoot = true,
    IsContainer = true,
    AllowedTypes = new[] { "FolderWithCover", "Image" })]
public class FolderWithCover
{
    [Property(Alias = "coverImage", TypeName = PropertyTypes.MediaPicker, Description = "Cover image.", Mandatory = true)]
    public string CoverImage { get; set; }
}
```

Using DocTypes on the front-end
---
In order to use your doctypes on the front-end you need to enable the `PublishedContentModel` factory.
```csharp
protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
{
    var types = PluginManager.Current.ResolveTypes<PublishedContentModel>();
    var factory = new PublishedContentModelFactory(types);
    PublishedContentModelFactoryResolver.Current.SetFactory(factory);
}
```
Using the doctypes.
```html
@inherits Umbraco.Web.Mvc.UmbracoTemplatePage<NicBell.UCreate.Test.Test.PageWithTitle>

@{
    Layout = null;
}

<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>PageWithTitle</title>
</head>
<body>
    <h1>@Model.Content.Heading</h1>
</body>
</html>
```