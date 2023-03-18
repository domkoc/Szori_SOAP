using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System.ServiceModel;
using System;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SeatReservation
{
    public class Cinema : ICinema
    {
        private static CinemaRoom? room;
        private static FaultException<CinemaException> RoomUninitializedException = new FaultException<CinemaException>(
               new CinemaException() {
                   ErrorCode = 99,
                   ErrorMessage = "Cinema room is not yet initialized"
               }
            );

        public async Task InitAsync(int rows, int columns)
        {
            if ((1 <= rows && rows <= 26) && (1 <= columns && columns <= 100))
            {
                room = new CinemaRoom(rows, columns);
            }
            else
            {
                CinemaException exception = new CinemaException();
                exception.ErrorCode = 1;
                exception.ErrorMessage = "Cannot create room without seats or bigger than 26 rows or 100 columns.";
                Exception e = new Exception();
                throw new FaultException<CinemaException>(exception);
            }
        }

        public async Task<Seat[]> GetAllSeatsAsync()
        {
            if (room is not null) return room.GetAllSeats().ToArray();

            else return new List<Seat>().ToArray();
        }

        public async Task<SeatStatus> GetSeatStatusAsync(Seat seat)
        {
            if (room is null) throw RoomUninitializedException;

            SeatStatus? tempSeatStatus = room.GetSeatStatus(seat);

            if (tempSeatStatus is SeatStatus value) return value;
            else
            {
                CinemaException exception = new CinemaException();
                exception.ErrorCode = 2;
                exception.ErrorMessage = "Unable to find seat with given id.";
                Exception e = new Exception();
                throw new FaultException<CinemaException>(exception);
            }
        }

        public async Task<string> LockAsync(Seat seat, int count)
        {
            if (room is null) throw RoomUninitializedException;

            string? lockId = room.LockSeats(seat, count);

            if (lockId is not null) return lockId;
            else
            {
                CinemaException exception = new CinemaException();
                exception.ErrorCode = 3;
                exception.ErrorMessage = "Unable to lock seats from given id.";
                Exception e = new Exception();
                throw new FaultException<CinemaException>(exception);
            }
        }

        public async Task UnlockAsync(string lockId)
        {
            if (room is null) throw RoomUninitializedException;

            bool success = room.UnlockSeats(lockId);
            if (!success)
            {
                CinemaException exception = new CinemaException();
                exception.ErrorCode = 4;
                exception.ErrorMessage = "Unable to unlock seats with given lockid.";
                Exception e = new Exception();
                throw new FaultException<CinemaException>(exception);
            }
        }

        public async Task ReserveAsync(string lockId)
        {
            if (room is null) throw RoomUninitializedException;

            bool success = room.ReserveSeats(lockId);
            if (!success)
            {
                CinemaException exception = new CinemaException();
                exception.ErrorCode = 5;
                exception.ErrorMessage = "Unable to reserve seats with given lockid.";
                Exception e = new Exception();
                throw new FaultException<CinemaException>(exception);
            }
        }

        public async Task BuyAsync(string lockId)
        {
            if (room is null) throw RoomUninitializedException;

            bool success = room.BuySeats(lockId);
            if (!success)
            {
                CinemaException exception = new CinemaException();
                exception.ErrorCode = 6;
                exception.ErrorMessage = "Unable to buy seats with given lockid.";
                Exception e = new Exception();
                throw new FaultException<CinemaException>(exception);
            }
        }

    }
}
