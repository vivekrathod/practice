using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _83OODesignParkingLot
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    public class ParkingLot
    {
        public readonly int Capacity;
        private readonly ParkingSpace[] _parkingSpaces;

        public ParkingLot(int capacity = 100)
        {
            Capacity = capacity;
            _parkingSpaces = new ParkingSpace[Capacity];
        }

        public ParkingSpace[] ParkingSpaces { get { return _parkingSpaces;} }

    }

    public class ParkingSpace
    {
        private Car _car;

        public bool IsFree { get { return _car == null; }}

        public void ParkCar(Car car)
        {
            _car = car;
        }

        public void DriveOut()
        {
            _car = null;
        }
    }

    public class Car
    {
    }

    public class Driver
    {
        private readonly Car _car;
        private ParkingSpace _parkingSpace;

        public Driver(Car car)
        {
            _car = car;
        }

        public void ParkCar(ParkingLot parkingLot)
        {
            foreach (ParkingSpace parkingSpace in parkingLot.ParkingSpaces)
            {
                if (parkingSpace.IsFree)
                {
                    parkingSpace.ParkCar(_car);
                    _parkingSpace = parkingSpace;
                    return;
                }
            }
            
            throw new Exception("Parking full");
        }

        public void DriveOut()
        {
            _parkingSpace.DriveOut();
        }

    }
}
