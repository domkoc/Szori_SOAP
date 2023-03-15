using System.ComponentModel.DataAnnotations;

namespace SeatReservation
{
    public class Cinema : ICinema
    {
        public Task BuyAsync(string lockId)
        {
            throw new NotImplementedException();
        }

        public Task<Seat[]> GetAllSeatsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<SeatStatus> GetSeatStatusAsync(Seat seat)
        {
            throw new NotImplementedException();
        }

        public Task InitAsync(int rows, int columns)
        {
            throw new NotImplementedException();
        }

        public Task<string> LockAsync(Seat seat, int count)
        {
            throw new NotImplementedException();
        }

        public Task ReserveAsync(string lockId)
        {
            throw new NotImplementedException();
        }

        public Task UnlockAsync(string lockId)
        {
            throw new NotImplementedException();
        }
    }
}
