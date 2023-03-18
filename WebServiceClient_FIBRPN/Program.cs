using SeatReservation;
using System.ServiceModel;

bool testing = false;
Console.WriteLine(args);
if (!testing)
{
    string url = "http://localhost:8080/WebService_FIBRPN/Cinema";
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

}
else
{
    string url = "http://localhost:8080/WebService_FIBRPN/Cinema";
    string row = "C";
    string column = "3";
    string task = "";
    try
    {
        url = args[0];
        row = args[1];
        column = args[2];
        task = args[3];
    }
    catch {}

    CinemaClient? client = null;
    try
    {
        var binding = new BasicHttpBinding();
        var endpoint = new EndpointAddress(new Uri(url));
        client = new CinemaClient(binding, endpoint);
        int rows = 5;
        int columns = 10;
        Seat testSeat = new Seat();
        testSeat.Row = "A";
        testSeat.Column = "2";
        try
        {
            Console.WriteLine("Init test:");
            await client.InitAsync(rows, columns);

            Console.WriteLine("GetAllSeats test:");
            var allSeats = await client.GetAllSeatsAsync();
            foreach (var seat in allSeats)
            {
                Console.WriteLine($"{seat.Row}:{seat.Column}");
            }

            Console.WriteLine("GetSeatStatus test:");
            var testSeatStatus = await client.GetSeatStatusAsync(testSeat);
            Console.WriteLine($"SeatStatus: {testSeatStatus}");

            Console.WriteLine("Lock test:");
            var testLock = await client.LockAsync(testSeat, 1);
            Console.WriteLine($"lockId: {testLock}");
            testSeatStatus = await client.GetSeatStatusAsync(testSeat);
            Console.WriteLine($"SeatStatus: {testSeatStatus}");
            Seat tempSeat = new Seat();
            tempSeat.Row = testSeat.Row;
            tempSeat.Column = "1";
            testLock = await client.LockAsync(tempSeat, 1);
            Console.WriteLine($"lockId: {testLock}");

            Console.WriteLine("Unlock test:");
            testSeatStatus = await client.GetSeatStatusAsync(tempSeat);
            Console.WriteLine($"SeatStatus: {testSeatStatus}");
            await client.UnlockAsync(testLock);
            testSeatStatus = await client.GetSeatStatusAsync(tempSeat);
            Console.WriteLine($"SeatStatus: {testSeatStatus}");

            Console.WriteLine("Reserve test:");
            testSeatStatus = await client.GetSeatStatusAsync(tempSeat);
            Console.WriteLine($"SeatStatus: {testSeatStatus}");
            await client.ReserveAsync(testLock);
            testSeatStatus = await client.GetSeatStatusAsync(tempSeat);
            Console.WriteLine($"SeatStatus: {testSeatStatus}");

            Console.WriteLine("Buy test:");
            testSeatStatus = await client.GetSeatStatusAsync(tempSeat);
            Console.WriteLine($"SeatStatus: {testSeatStatus}");
            await client.BuyAsync(testLock);
            testSeatStatus = await client.GetSeatStatusAsync(tempSeat);
            Console.WriteLine($"SeatStatus: {testSeatStatus}");
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
}
Console.ReadKey();