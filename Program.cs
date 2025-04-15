namespace Airline_Reservation_Console_System_By_List
{
    internal class Program
    {
        // Flight data using lists
        static List<string> flightCodes = new List<string>();
        static List<string> fromCities = new List<string>();
        static List<string> toCities = new List<string>();
        static List<DateTime> departureTimes = new List<DateTime>();
        static List<int> durations = new List<int>();
        static List<int> fare = new List<int>();

        // Booking data using lists
        static List<string> bookingIDs = new List<string>();
        static List<string> passengerNames = new List<string>();
        static List<string> bookedFlightCodes = new List<string>();



        static void StartSystem()
        {
            DisplayWelcomeMessage();


            while (true)
            {
                int choice = ShowMainMenu();
                switch (choice)
                {
                    case 1: // Book a Flight
                        Console.Write("Enter passenger name: ");
                        string name = Console.ReadLine();
                        Console.Write("Enter flight code (optional): ");
                        string code = Console.ReadLine();
                        if (string.IsNullOrEmpty(code))
                        {
                            code = "Default001";
                        }
                        if (ValidateFlightCode(code))
                        {
                            string bookid = BookFlight(name, code);
                            Console.WriteLine($"Booking successful! Booking ID: {bookid}");

                            // Calculate fare
                            Console.Write("Enter number of tickets: ");
                            int numTickets = int.Parse(Console.ReadLine());
                            int basePrice = fare[flightCodes.IndexOf(code)];
                            Console.Write("Enter discount (0 if none): ");
                            int discount = int.Parse(Console.ReadLine());

                            int totalFare = CalculateFare(basePrice, numTickets, discount);
                            Console.WriteLine($"Total fare: ${totalFare}");
                        }
                        else
                        {
                            Console.WriteLine("Invalid flight code.");
                        }
                        break;

                    case 2:
                        Console.Write("Enter passenger name to cancel booking: ");
                        string passengerToCancel = null;
                        CancelFlightBooking(out passengerToCancel);
                        break;

                    case 3:
                        DisplayAllFlights();
                        break;

                    case 4:
                        Console.Write("Enter flight code: ");
                        string fcode = Console.ReadLine();
                        DisplayFlightDetails(fcode);
                        break;

                    case 5:
                        Console.Write("Enter destination city: ");
                        string city = Console.ReadLine();
                        SearchBookingsByDestination(city);
                        break;

                    case 6:
                        Console.WriteLine("Adding a new flight...");
                        Console.Write("Flight Code: ");
                        string newFlightCode = Console.ReadLine();
                        Console.Write("From City: ");
                        string fromCity = Console.ReadLine();
                        Console.Write("To City: ");
                        string toCity = Console.ReadLine();
                        Console.Write("Departure Time (yyyy-mm-dd hh:mm): ");
                        DateTime departureTime = DateTime.Parse(Console.ReadLine());
                        Console.Write("Duration (in hours): ");
                        int duration = int.Parse(Console.ReadLine());
                        Console.Write("Price: ");
                        int price = int.Parse(Console.ReadLine());
                        AddFlight(newFlightCode, fromCity, toCity, departureTime, duration, price);
                        break;

                    case 7:
                        Console.Write("Enter flight code to update: ");
                        string updateCode = Console.ReadLine();
                        Console.Write("Enter new departure time (yyyy-mm-dd hh:mm): ");
                        DateTime newDepartureTime = DateTime.Parse(Console.ReadLine());
                        UpdateFlightDeparture(ref newDepartureTime);
                        break;

                    case 8:
                        ExitApplication();
                        return;

                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
        //================= Welcome Message =================

        static void DisplayWelcomeMessage()
        {
            Console.WriteLine("=====Welcome to the Airline Reservation System!=========");
        }

        //================= Main Menu =================
        static int ShowMainMenu()
        {
            Console.WriteLine("=====Main Menu=====");
            Console.WriteLine("1. Book a Flight");
            Console.WriteLine("2. Cancel a Booking");
            Console.WriteLine("3. View All Flights");
            Console.WriteLine("4. View Flight Details (by flight code)");
            Console.WriteLine("5. Search Bookings by Destination");
            Console.WriteLine("6. Add flight");
            Console.WriteLine("7. Update Flight Departure");
            Console.WriteLine("8. Exit");
            Console.Write("Select an option (1-8): ");
            int input = int.Parse(Console.ReadLine());
            Console.Clear();
            return input;
        }

        //================= Exit Application =================
        static void ExitApplication()
        {
            Console.WriteLine("Thank you for using the Airline Reservation System. Goodbye!");
            return;
        }

        //================= Flight add =================
        static void AddFlight(string flightCode, string fromCity, string toCity,
                             DateTime departureTime, int duration, int price)
        {
            flightCodes.Add(flightCode);
            fromCities.Add(fromCity);
            toCities.Add(toCity);
            departureTimes.Add(departureTime);
            durations.Add(duration);
            fare.Add(price);
        }

        // ================= Flight Display ==================
        static void DisplayAllFlights()
        {
            for (int i = 0; i < flightCodes.Count; i++)
            {
                Console.WriteLine("Flight Code: " + flightCodes[i]);
                Console.WriteLine("From: " + fromCities[i]);
                Console.WriteLine("To: " + toCities[i]);
                Console.WriteLine("Departure: " + departureTimes[i]);
                Console.WriteLine("Duration: " + durations[i] + " hours");
                Console.WriteLine("Price: $" + fare[i]);
                Console.WriteLine();
            }
        }

        //================ Flight Search ===========
        static bool FindFlightByCode(string code)
        {
            int index = flightCodes.IndexOf(code);
            if (index != -1)
            {
                Console.WriteLine("Flight found: " + flightCodes[index]);
                Console.WriteLine("From: " + fromCities[index]);
                Console.WriteLine("To: " + toCities[index]);
                Console.WriteLine("Departure: " + departureTimes[index]);
                Console.WriteLine("Duration: " + durations[index] + " hours");
                Console.WriteLine("Price: $" + fare[index]);
                return true;
            }
            return false;
        }

        //================ Flight Departure Update ===========
        static void UpdateFlightDeparture(ref DateTime departure)
        {
            Console.WriteLine("Enter flight code to update: ");
            string flightCode = Console.ReadLine();
            int index = flightCodes.IndexOf(flightCode);
            if (index != -1)
            {
                departureTimes[index] = departure;
                Console.WriteLine("Flight departure updated successfully.");
            }
        }

        //================= Booking Cancellation ===========
        public static void CancelFlightBooking(out string canceled)
        {
            canceled = null;
            Console.Write("Enter passenger name to cancel booking: ");
            canceled = Console.ReadLine();
            bool found = false;

            for (int i = 0; i < passengerNames.Count; i++)
            {
                if (passengerNames[i] == canceled)
                {
                    if (ConfirmAction($"cancel booking for {passengerNames[i]}"))
                    {
                        bookingIDs.RemoveAt(i);
                        passengerNames.RemoveAt(i);
                        bookedFlightCodes.RemoveAt(i);
                        found = true;
                        Console.WriteLine("Booking cancelled successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Booking cancellation aborted.");
                    }
                    break;
                }
            }

            if (!found)
            {
                Console.WriteLine("No booking found for the given passenger name.");
            }
        }

        // ================ Flight Booking =================
        static string BookFlight(string passengerName, string flightCode = "Default001")
        {
            // Implementation here
            return "";
        }

        //================= Flight Code Validation ===========
        static bool ValidateFlightCode(string flightCode)
        {
            // Implementation here
            return false;
        }

        //================ Booking ID Generation ===========
        static string GenerateBookingID(string passengerName)
        {
            // Implementation here
            return "";
        }

        //=============== Flight Details ==================
        static void DisplayFlightDetails(string code)
        {
            // Implementation here
        }

        // ============== Search Bookings =================
        static void SearchBookingsByDestination(string toCity)
        {
            // Implementation here
        }

        //============== Fare Calculation =================
        static int CalculateFare(int basePrice, int numTickets)
        {
            // Implementation here
            return 0;
        }

        static double CalculateFare(double basePrice, int numTickets)
        {
            // Implementation here
            return 0;
        }

        static int CalculateFare(int basePrice, int numTickets, int discount)
        {
            // Implementation here
            return 0;
        }

        //================ Confirm Action =================
        static bool ConfirmAction(string action)
        {
            // Implementation here
            return false;
        }

        static void Main(string[] args)
        {
            {
                StartSystem();
            }
        }



    }
}
