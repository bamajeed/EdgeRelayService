using System.Collections.Generic;

namespace AzureRelayPortBridge
{
    public class HybridConnectionServerOptions
    {
        public string ServiceBusNamespace { get; set; }
        public string ServiceBusSasToken { get; set; }
        public List<ForwardingRule> ForwardingRules { get; set; }

        public class ForwardingRule
        {
            public string ServiceBusConnectionName { get; set; }
            public string TargetHostname { get; set; }
            public string TargetPorts { get; set; }
            public int InstanceCount { get; set; }
        }
    }
}
