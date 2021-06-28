using Microsoft.Azure.ServiceBus.Primitives;
using System;

namespace TokenGenerator
{
    public class Program
    {
        public static void Main()
        {
            //fill in the nameSpaceURI, relayAccessPolicyName, relayAccessKey and choose the duration of the token
            //The Listener needs Listen access (on the device) and the CLient needs Send Access. So we are combining them here in one policy (or use different keys)
            string nameSpaceURI = "***..servicebus.windows.net";
            string relayAccessPolicyName = "{Policy Name}";
            string relayAccessKey = "{Policy Key}";
            int tokenValidityMinutes = 60;
            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(relayAccessPolicyName, relayAccessKey);
            var token = tokenProvider.GetTokenAsync(nameSpaceURI, TimeSpan.FromMinutes(tokenValidityMinutes)).Result;
            Console.WriteLine($"Token: {token.TokenValue}");
        }
    }

}

