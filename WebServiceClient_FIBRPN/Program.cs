using SeatReservation;
using System.ServiceModel;

Console.WriteLine(args);
String url = "http://localhost:8080/WebService_FIBRPN/Cinema";
String row = "H";
int column = 31;
String task = "";
try
{
    url = args[0];
    row = args[1];
    column = int.Parse(args[2]);
    task = args[3];
}
catch {}

CinemaClient? client = null;
try
{
    var binding = new BasicHttpBinding();
    var endpoint = new EndpointAddress(new Uri(url));
    client = new CinemaClient(binding, endpoint);
    var response = await client.GetAllSeatsAsync();
    Console.WriteLine(response);
}
finally
{
    if (client is not null) await client.CloseAsync();
}