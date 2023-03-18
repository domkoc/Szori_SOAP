using SeatReservation;
using System.ServiceModel;

string url = "";
string row = "";
string column = "";
string task = "";
try
{
    url = args[0];
    row = args[1];
    column = args[2];
    task = args[3];
}
catch {
    throw new ArgumentException("Not all arguments provided.");
}

CinemaClient? client = null;
try
{
    var binding = new BasicHttpBinding();
    var endpoint = new EndpointAddress(new Uri(url));
    client = new CinemaClient(binding, endpoint);

    var seat = new Seat();
    seat.Row = row;
    seat.Column = column;
    var count = 1;

    try
    {
        switch (task)
        {
            case "Lock":
                await client.LockAsync(seat, count);
                break;
            case "Reserve":
                var reserveLockId = await client.LockAsync(seat, count);
                await client.ReserveAsync(reserveLockId);
                break;
            case "Buy":
                var buyLockId = await client.LockAsync(seat, count);
                await client.BuyAsync(buyLockId);
                break;
            default:
                throw new ArgumentException("Invalid task operation.");
        }
    }
    catch (FaultException<CinemaException> e)
    {
        Console.WriteLine(e.Detail.ErrorCode); // TODO: Hogy printeljük?
        Console.WriteLine(e.Detail.ErrorMessage); // TODO: Hogy printeljük?
        Console.WriteLine(e.Detail.ToString()); // TODO: Hogy printeljük?
        Console.WriteLine(e.StackTrace); // TODO: Hogy printeljük?
    }
}
finally
{
    if (client is not null) await client.CloseAsync();
}
