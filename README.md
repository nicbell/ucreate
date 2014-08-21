UCreate [![Build status](https://ci.appveyor.com/api/projects/status/60v4v2cbl6nxmf0q)](https://ci.appveyor.com/project/nicbell/ucreate)
=======

Create doc types, media types and data types for Umbraco 7 using a code-first approach. Inspired by [USiteBuilder](https://github.com/spopovic/uSiteBuilder).

Available on NuGet
---
```
PM> Install-Package UCreate
```

Usage
---

The only configuration you'll need to get started is an app setting in your ```web.config```. This tells UCreate to sync your doc types, media types and data types on application start.

```xml
...
<appSettings>
    ...
    <add key="UCreateSyncEnabled" value="true" />
    ...
</appSettings>
...
```

DocType example
---
Doc types support property inheritance. Here is a list of available [icons](http://nicbell.github.io/ucreate/icons.html).
```csharp
[DocType(Name = "Page With Title",
    Icon = "icon-zip color-blue",
    AllowedAsRoot = true,
    AllowedTemplates = new[] { "PageWithTitle" },
    DefaultTemplate = "PageWithTitle")]
public class PageWithTitle : BaseDocType
{
    public PageWithTitle(IPublishedContent content) : base(content)
    { }

    [Property(Alias = "heading", TypeName = PropertyTypes.Textstring, Description = "Heading for page", Mandatory = true, TabName = "Content")]
    public string Heading { get; set; }
}
```

MediaType example
---
Media types support property inheritance.
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

Strongly typed views
---
In order to use your doc types on the front-end you need to enable the `PublishedContentModel` factory. UCreate can do this for you if add the following app setting:
```xml
...
<appSettings>
    ...
    <add key="UCreatePublishedModelsEnabled" value="true" />
    ...
</appSettings>
...
```
Then using the doc types in your views is pretty simple.
```html
@inherits Umbraco.Web.Mvc.UmbracoTemplatePage<NicBell.UCreate.Test.DocTypes.PageWithTitle>

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
