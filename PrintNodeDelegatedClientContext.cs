using System;

namespace PrintNode.Net
{
    public class PrintNodeDelegatedClientContext : IDisposable
    {
        public static PrintNodeDelegatedClientContext Current { get; private set; }

        internal PrintNodeDelegatedClientContextAuthenticationMode AuthenticationMode;

        internal string AuthenticationValue { get; private set; }

        public PrintNodeDelegatedClientContext(int accountId)
        {
            AuthenticationValue = accountId.ToString();
            AuthenticationMode = PrintNodeDelegatedClientContextAuthenticationMode.Id;
            Current = this;
        }

        public PrintNodeDelegatedClientContext(string email)
        {
            AuthenticationValue = email;
            AuthenticationMode = PrintNodeDelegatedClientContextAuthenticationMode.Email;
            Current = this;
        }

        public void Dispose()
        {
            Current = null;
        }
    }
}