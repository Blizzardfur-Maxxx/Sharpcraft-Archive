using System;

namespace SharpCraft.Core.Util.Logging
{
    public interface IErrorHandler
    {
        void OnError(object source, Exception ex);
    }
}
