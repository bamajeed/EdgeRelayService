DISCLAIMER: This code is provided as is without support and with minimum error checking. The code in this repo is not meant for use in production.

This code uses the libraries from https://github.com/AndreasM009/azure-relay-portbridge/tree/master/AzureRelayPortBridge

Build the docker container in project PortBridgeServiceEdge and push it to your container registry.
Create an IoT Edge device and add the module PortBridgeServiceEdge to the device.

Create a relay namespace and a hybrid connection. https://docs.microsoft.com/en-us/azure/azure-relay/relay-create-namespace-portal
You can use the key for the RootManageSharedAccessKey policy. Or you can generate policies for send, receive or send/receive. 
Typically the device side needs Listen privileges while the client side needs send privileges. A policy with send and receive should work for both.

Use the TokenGenerator project to get a token for the relay service. 
You will need to enter the relevant values in the program to generate the token.
Run the TokenGenerator and copy the resulting Token which looks like : SharedAccessSignature sr=**************************************************

The module has two direct methods StartRelay and StopRelay

To start the relay service invoke StartRelay method with the following method body:

{
  "HybridConnectionServerHost": {
    "ServiceBusNamespace": "{service bus namespace}",
    "ServiceBusSasToken": "{token generated as above}",
    "ForwardingRules": [
      {
        "ServiceBusConnectionName": "{name of Hybrid Connection}",
        "TargetHostname": "{IP or hostname of target}",
        "TargetPorts": "{target port e.g. 22}",
        "InstanceCount": 1 (or more if needed)
      }
    ]
  }
}

if the method invocation is successful you should get a listener connection on the Hybridconnection portal 

Now start the PortBridgeClient application (after entering the correct values in the appsettings.json), remember that the client needs "send" rights.

If all is ok then you can now start the connection to the device for example using ssh to localhost on port 2222 (based on the port used in the client's appsetting.json)

When finished make sure to stop the relay service by invoking a direct method on the module with method name StopRelay with empty method payload

The module should work even if the device is behind an http proxy. There is no need to have open ports on the device.

Currenlty there is not much error checking so the payload for the method StartRelay needs to be correct otherwise it will crash.
