using Microsoft.Azure.Relay;
using System;
using System.Threading.Tasks;

namespace AzureRelayPortBridge
{
    internal class StaticSasTokenProvider : TokenProvider
    {
        private readonly string _sasTokenString;

        public StaticSasTokenProvider(string sasTokenString)
        {
            _sasTokenString = sasTokenString;
        }

        protected override Task<SecurityToken> OnGetTokenAsync(string audience, TimeSpan validFor)
        {
            var securityToken = new SasToken(_sasTokenString);
            return Task.FromResult<SecurityToken>(securityToken);
        }
    }

    class SasToken : SecurityToken
    {
        public const int MaxKeyNameLength = 256;
        public const int MaxKeyLength = 256;
        public const string SharedAccessSignature = "SharedAccessSignature";
        public const string SignedResource = "sr";
        public const string Signature = "sig";
        public const string SignedKeyName = "skn";
        public const string SignedExpiry = "se";
        public const string SignedResourceFullFieldName = SharedAccessSignature + " " + SignedResource;
        public const string SasKeyValueSeparator = "=";
        public const string SasPairSeparator = "&";

        public SasToken(string tokenString)
            : base(
                  tokenString,
                  audienceFieldName: SignedResourceFullFieldName,
                  expiresOnFieldName: SignedExpiry,
                  keyValueSeparator: SasKeyValueSeparator,
                  pairSeparator: SasPairSeparator)
        {
        }
    }
}
