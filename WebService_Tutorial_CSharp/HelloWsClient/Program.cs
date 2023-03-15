using HelloWsClient;
using System.ServiceModel;
HelloClient? hello = null;
try
{
    var binding = new BasicHttpBinding();
    var endpoint = new EndpointAddress(new Uri("http://localhost:5052/HelloService"));
    hello = new HelloClient(binding, endpoint);
    var response = await hello.SayHelloAsync("me");
    Console.WriteLine(response);
}
finally
{
    if (hello is not null) await hello.CloseAsync();
}