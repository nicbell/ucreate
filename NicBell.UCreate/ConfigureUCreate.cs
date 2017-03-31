using System;
using System.Configuration;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;

namespace NicBell.UCreate
{
    public class ConfigureUCreate : ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            if (ConfigurationManager.AppSettings["UCreatePublishedModelsEnabled"] == null || !Convert.ToBoolean(ConfigurationManager.AppSettings["UCreatePublishedModelsEnabled"]))
                return;

            //Add published content factory
            var types = PluginManager.Current.ResolveTypes<PublishedContentModel>();
            var factory = new PublishedContentModelFactory(types);
            PublishedContentModelFactoryResolver.Current.SetFactory(factory);
        }


        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            if (ConfigurationManager.AppSettings["UCreateSyncEnabled"] == null || !Convert.ToBoolean(ConfigurationManager.AppSettings["UCreateSyncEnabled"]))
                return;

            //Sync doctypes, mediatypes, datatypes
            CmsSyncManger.Synchronize();
        }
    }
}
