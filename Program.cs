using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkingSystem
{
    public class ParkingLot
    {
        private class Slot
        {
            public int SlotNumber { get; set; }
            public string? RegistrationNumber { get; set; }
            public string? Color { get; set; }
            public string? Type { get; set; } // "Mobil" or "Motor"
            public bool IsOccupied { get; set; }
        }

        private readonly List<Slot> _slots;

        public ParkingLot(int size)
        {
            _slots = new List<Slot>();
            for (int i = 1; i <= size; i++)
            {
                _slots.Add(new Slot { SlotNumber = i, IsOccupied = false });
            }
        }

        public void Park(string registrationNumber, string color, string type)
        {
            var availableSlot = _slots.FirstOrDefault(s => !s.IsOccupied);
            if (availableSlot == null)
            {
                Console.WriteLine("Sorry, parking lot is full");
                return;
            }

            if (type != "Mobil" && type != "Motor")
            {
                Console.WriteLine("Only Mobil and Motor are allowed");
                return;
            }

            availableSlot.RegistrationNumber = registrationNumber;
            availableSlot.Color = color;
            availableSlot.Type = type;
            availableSlot.IsOccupied = true;

            Console.WriteLine($"Allocated slot number: {availableSlot.SlotNumber}");
        }

        public void Leave(int slotNumber)
        {
            var slot = _slots.FirstOrDefault(s => s.SlotNumber == slotNumber);
            if (slot == null || !slot.IsOccupied)
            {
                Console.WriteLine("Slot is already free or invalid slot number");
                return;
            }

            slot.IsOccupied = false;
            slot.RegistrationNumber = null;
            slot.Color = null;
            slot.Type = null;

            Console.WriteLine($"Slot number {slotNumber} is free");
        }

        public void Status()
        {
            Console.WriteLine("Slot\tNo.\t\tType\t\tRegistration No\tColour");
            foreach (var slot in _slots.Where(s => s.IsOccupied))
            {
                Console.WriteLine($"{slot.SlotNumber}\t{slot.RegistrationNumber}\t{slot.Type}\t{slot.Color}");
            }
        }

        public void GetVehiclesByType(string type)
        {
            var count = _slots.Count(s => s.IsOccupied && s.Type == type);
            Console.WriteLine(count);
        }

        public void GetRegistrationNumbersByPlateType(string plateType)
        {
            var filteredSlots = _slots.Where(s => s.IsOccupied &&
                                                  ((plateType == "odd" && int.Parse(s.RegistrationNumber!.Split('-')[1]) % 2 != 0) ||
                                                   (plateType == "even" && int.Parse(s.RegistrationNumber!.Split('-')[1]) % 2 == 0)));

            var registrationNumbers = filteredSlots.Select(s => s.RegistrationNumber);
            Console.WriteLine(string.Join(", ", registrationNumbers));
        }

        public void GetRegistrationNumbersByColor(string color)
        {
            var registrationNumbers = _slots.Where(s => s.IsOccupied && s.Color == color)
                                            .Select(s => s.RegistrationNumber);
            Console.WriteLine(string.Join(", ", registrationNumbers));
        }

        public void GetSlotNumbersByColor(string color)
        {
            var slotNumbers = _slots.Where(s => s.IsOccupied && s.Color == color)
                                     .Select(s => s.SlotNumber);
            Console.WriteLine(string.Join(", ", slotNumbers));
        }

        public void GetSlotNumberByRegistrationNumber(string registrationNumber)
        {
            var slot = _slots.FirstOrDefault(s => s.IsOccupied && s.RegistrationNumber == registrationNumber);
            if (slot != null)
            {
                Console.WriteLine(slot.SlotNumber);
            }
            else
            {
                Console.WriteLine("Not found");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ParkingLot? parkingLot = null;

            while (true)
            {
                var command = Console.ReadLine()?.Split(' ');
                if (command == null || command.Length == 0) continue;

                switch (command[0])
                {
                    case "create_parking_lot":
                        int size = int.Parse(command[1]);
                        parkingLot = new ParkingLot(size);
                        Console.WriteLine($"Created a parking lot with {size} slots");
                        break;

                    case "park":
                        if (parkingLot != null)
                        {
                            parkingLot.Park(command[1], command[2], command[3]);
                        }
                        else
                        {
                            Console.WriteLine("Parking lot is not created yet.");
                        }
                        break;

                    case "leave":
                        if (parkingLot != null)
                        {
                            parkingLot.Leave(int.Parse(command[1]));
                        }
                        else
                        {
                            Console.WriteLine("Parking lot is not created yet.");
                        }
                        break;

                    case "status":
                        parkingLot?.Status();
                        break;

                    case "type_of_vehicles":
                        parkingLot?.GetVehiclesByType(command[1]);
                        break;

                    case "registration_numbers_for_vehicles_with_odd_plate":
                        parkingLot?.GetRegistrationNumbersByPlateType("odd");
                        break;

                    case "registration_numbers_for_vehicles_with_even_plate":
                        parkingLot?.GetRegistrationNumbersByPlateType("even");
                        break;

                    case "registration_numbers_for_vehicles_with_colour":
                        parkingLot?.GetRegistrationNumbersByColor(command[1]);
                        break;

                    case "slot_numbers_for_vehicles_with_colour":
                        parkingLot?.GetSlotNumbersByColor(command[1]);
                        break;

                    case "slot_number_for_registration_number":
                        parkingLot?.GetSlotNumberByRegistrationNumber(command[1]);
                        break;

                    case "exit":
                        return;

                    default:
                        Console.WriteLine("Invalid command");
                        break;
                }
            }
        }
    }
}
