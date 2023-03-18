using System;
using System.ServiceModel;

namespace SeatReservation
{
    public class CinemaRoom
    {
        public class CinemaRow
        {
            public class CinemaSeat
            {
                public int ColumnNumber;
                public SeatStatus seatStatus;
                public string? Lockid;

                public CinemaSeat(int columnNumber)
                {
                    this.ColumnNumber = columnNumber;
                    this.seatStatus = SeatStatus.Free;
                    this.Lockid = null;
                }
            }
            private int RowNumber;
            public char RowId
            {
                get
                {
                    return RowIdFromNumber(RowNumber);
                }
                set
                {
                    RowNumber = RowNumberFromId(value);
                }
            }
            public List<CinemaSeat> seats;

            public CinemaRow(int rowNumber, int columns)
            {
                this.RowNumber = rowNumber;
                seats = new List<CinemaSeat>();
                for (int i = 1; i <= columns; i++)
                {
                    this.seats.Add(new CinemaSeat(i));
                }
            }

            public char RowIdFromNumber(int number)
            {
                return (char)(number + 64);
            }

            public int RowNumberFromId(char id)
            {
                return char.ToUpper(id) - 64;
            }

            public Seat[] GetAllSeats()
            {
                List<Seat> tempSeats = new List<Seat>();
                foreach (var seat in seats)
                {
                    Seat tempSeat = new Seat();
                    tempSeat.Row = this.RowId.ToString();
                    tempSeat.Column = seat.ColumnNumber.ToString();
                    tempSeats.Add(tempSeat);
                }
                return tempSeats.ToArray();
            }

            public string? LockSeats(int startingSeatNo, int count)
            {
                if (startingSeatNo < 1 || count < 1) return null;
                if (seats.Count - (startingSeatNo - 1) <= count) return null;

                CinemaSeat firstSeat = seats.Find(seat => seat.ColumnNumber == startingSeatNo);

                if (firstSeat is not null)
                {
                    int firstIdx = seats.IndexOf(firstSeat);
                    var tempSeats = seats.GetRange(firstIdx, count);
                    foreach (var tempSeat in tempSeats)
                    {
                        if (tempSeat.seatStatus is not SeatStatus.Free) return null;
                    }
                    string lockId = Guid.NewGuid().ToString();
                    foreach (var tempSeat in tempSeats)
                    {
                        tempSeat.seatStatus = SeatStatus.Locked;
                        tempSeat.Lockid = lockId;
                    }
                    return lockId;
                }
                return null;
            }
        }

        public List<CinemaRow> cinemaRows;

        public CinemaRoom(int rows, int columns)
        {
            this.cinemaRows = new List<CinemaRow>();
            for (int i = 1; i <= rows; i++)
            {
                this.cinemaRows.Add(new CinemaRow(i, columns));
            }
        }

        public List<Seat> GetAllSeats()
        {
            List<Seat> tempSeats = new List<Seat>();
            foreach (var row in cinemaRows)
            {
                tempSeats.AddRange(row.GetAllSeats());
            }
            return tempSeats;
        }

        public SeatStatus? GetSeatStatus(Seat seat)
        {
            SeatStatus? tempSeatStatus = null;
            CinemaRow tempRow = this.cinemaRows.Find(row => row.RowId == seat.Row[0]);
            if (tempRow is not null)
            {
                CinemaRow.CinemaSeat tempSeat = tempRow.seats.Find(column => column.ColumnNumber.ToString() == seat.Column);
                if (tempSeat is not null) tempSeatStatus = tempSeat.seatStatus;
            }
            return tempSeatStatus;
        }

        public string? LockSeats(Seat startingSeat, int count)
        {
            CinemaRow tempRow = this.cinemaRows.Find(row => row.RowId == startingSeat.Row[0]);
            if (tempRow is not null)
            {
                return tempRow.LockSeats(int.Parse(startingSeat.Column), count);
            }
            else
            {
                return null;
            }
        }

        public bool UnlockSeats(string lockId)
        {
            bool isSuccessful = false;
            foreach (var cinemaRow in cinemaRows)
            {
                foreach (var cinemaSeat in cinemaRow.seats)
                {
                    if (cinemaSeat.Lockid == lockId)
                    {
                        if (cinemaSeat.seatStatus is not SeatStatus.Locked) return false;
                        isSuccessful = true;
                        cinemaSeat.Lockid = null;
                        cinemaSeat.seatStatus = SeatStatus.Free;
                    }
                }
            }
            return isSuccessful;
        }

        public bool ReserveSeats(string lockId)
        {
            bool isSuccessful = false;
            foreach (var cinemaRow in cinemaRows)
            {
                foreach (var cinemaSeat in cinemaRow.seats)
                {
                    if (cinemaSeat.Lockid == lockId)
                    {
                        if (cinemaSeat.seatStatus is not SeatStatus.Locked) return false;
                        isSuccessful = true;
                        cinemaSeat.seatStatus = SeatStatus.Reserved;
                    }
                }
            }
            return isSuccessful;
        }

        public bool BuySeats(string lockId)
        {
            bool isSuccessful = false;
            foreach (var cinemaRow in cinemaRows)
            {
                foreach (var cinemaSeat in cinemaRow.seats)
                {
                    if (cinemaSeat.Lockid == lockId)
                    {
                        if (cinemaSeat.seatStatus is not SeatStatus.Locked && cinemaSeat.seatStatus is not SeatStatus.Reserved) return false;
                        isSuccessful = true;
                        cinemaSeat.seatStatus = SeatStatus.Sold;
                    }
                }
            }
            return isSuccessful;
        }
    }
}

