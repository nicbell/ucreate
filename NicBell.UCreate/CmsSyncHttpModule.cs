using System;
using System.Web;

namespace NicBell.UCreate
{
    public class CmsSyncHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += BeginRequest;
        }

        void BeginRequest(object sender, EventArgs e)
        {
            CmsSyncManger.SynchronizeIfNotSynchronized();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
