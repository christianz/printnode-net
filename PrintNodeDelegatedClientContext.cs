using System;

namespace PrintNode.Net
{
    public class PrintNodeDelegatedClientContext : IDisposable
    {
        public static PrintNodeDelegatedClientContext Current { get; private set; }
        internal string ClientId { get; private set; }

        public PrintNodeDelegatedClientContext(string clientId)
        {
            if (Current != null)
            {
                throw new Exception("Nested client contexts are not supported.");
            }

            ClientId = clientId;
            Current = this;
        }

        public void Dispose()
        {
            Current = null;
        }
    }
}