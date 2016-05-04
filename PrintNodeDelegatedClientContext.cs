using System;

namespace PrintNode.Net
{
    public class PrintNodeDelegatedClientContext : IDisposable
    {
        public static PrintNodeDelegatedClientContext Current { get; private set; }
        internal int AccountId { get; private set; }

        public PrintNodeDelegatedClientContext(int accountId)
        {
            AccountId = accountId;
            Current = this;
        }

        public void Dispose()
        {
            Current = null;
        }
    }
}