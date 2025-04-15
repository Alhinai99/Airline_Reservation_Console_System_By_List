namespace Airline_Reservation_Console_System_By_List
{
    internal class Program
    {
        // Flight data 
        static List<string> flightCodes = new List<string>();
        static List<string> fromCities = new List<string>();
        static List<string> toCities = new List<string>();
        static List<DateTime> departureTimes = new List<DateTime>();
        static List<int> durations = new List<int>();
        static List<int> fare = new List<int>();

        // Booking data 
        static List<string> bookingIDs = new List<string>();
        static List<string> passengerNames = new List<string>();
        static List<string> bookedFlightCodes = new List<string>();



        static void StartSystem()
        {
            DisplayWelcomeMessage(); //call function to display welcome message


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
                        if (ValidateFlightCode(code)) //chck if the flgiht code is validate 
                        {
                            string bookid = BookFlight(name, code); // call function to book flight with the input entered early (name and code)
                            Console.WriteLine($"Booking successful! Booking ID: {bookid}");

                            // Calculate fare
                            Console.Write("Enter number of tickets: ");
                            int numTickets = int.Parse(Console.ReadLine());
                            int basePrice = fare[flightCodes.IndexOf(code)];
                            Console.Write("Enter discount (0 if none): ");
                            int discount = int.Parse(Console.ReadLine());
                            if (discount != 0)
                            {
                                int totalFare = CalculateFare(basePrice, numTickets, discount); // Calculate total fare with discount

                                Console.WriteLine($"Total fare: ${totalFare}");
                            }
                            else
                            {
                                double totalFare = CalculateFare(basePrice, numTickets); // Calculate total fare
                                Console.WriteLine($"Total fare: ${totalFare}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid flight code.");
                        }
                            break;
                        

                    case 2: // Cancel a Booking
                        Console.Write("Enter passenger name to cancel booking: ");
                        string passengerToCancel = null;
                        CancelFlightBooking(out passengerToCancel);
                        break;

                    case 3: // View All Flights
                        DisplayAllFlights();
                        break;

                    case 4: // View Flight Details
                        Console.Write("Enter flight code: ");
                        string fcode = Console.ReadLine();
                        DisplayFlightDetails(fcode);
                        break;

                    case 5: // Search Bookings by Destination
                        Console.Write("Enter destination city: ");
                        string city = Console.ReadLine();
                        SearchBookingsByDestination(city);
                        break;

                    case 6: // Add Flight
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

                    case 7: // Update Flight Departure
                        Console.Write("Enter flight code to update: ");
                        string updateCode = Console.ReadLine();
                        Console.Write("Enter new departure time (yyyy-mm-dd hh:mm): ");
                        DateTime newDepartureTime = DateTime.Parse(Console.ReadLine());
                        UpdateFlightDeparture(ref newDepartureTime);
                        break;

                    case 8: // Exit
                        ExitApplication();
                        return;

                    default: // Invalid Option
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
            canceled = null; // Initialize the canceled variable
            Console.Write("Enter passenger name to cancel booking: "); 
            canceled = Console.ReadLine();
            bool found = false; 

            for (int i = 0; i < passengerNames.Count; i++)
            {
                if (passengerNames[i] == canceled) // check if the name matches
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
            string bookingID = GenerateBookingID(passengerName); // Generate a unique booking ID by calling the function
            bookingIDs.Add(bookingID);
            passengerNames.Add(passengerName);
            bookedFlightCodes.Add(flightCode);
            return bookingID;
        }

        //================= Flight Code Validation ===========
        static bool ValidateFlightCode(string flightCode)
        {
            return flightCodes.Contains(flightCode); // Check if the flight code exists in the list
        }

        //================ Booking ID Generation ===========
        static string GenerateBookingID(string passengerName)
        {
            return "BID" + (bookingIDs.Count + 1).ToString("D2"); // Generate a unique booking ID , ("D2") to show only two digits number 00
        }

        //=============== Flight Details ==================
        static void DisplayFlightDetails(string code)
        {
            int index = flightCodes.IndexOf(code); // Find the index of the flight code
            if (index != -1)
            {
                Console.WriteLine("Flight Code: " + flightCodes[index]);
                Console.WriteLine("From: " + fromCities[index]);
                Console.WriteLine("To: " + toCities[index]);
                Console.WriteLine("Departure: " + departureTimes[index]);
                Console.WriteLine("Duration: " + durations[index] + " hours");
                Console.WriteLine("Fare: " + fare[index]);

                Console.WriteLine("Booking Details: ");
                for (int i = 0; i < bookedFlightCodes.Count; i++) 
                {
                    if (bookedFlightCodes[i] == code)
                    {
                        Console.WriteLine("Booking ID: " + bookingIDs[i]); 
                        Console.WriteLine("Passenger Name: " + passengerNames[i]);
                    }
                }
            }
        }

        // ============== Search Bookings destination =================
        static void SearchBookingsByDestination(string toCity)
        {
            for (int i = 0; i < flightCodes.Count; i++) 
            {
                if (toCities[i] == toCity) //check the city name
                {
                    Console.WriteLine("Flight found: " + flightCodes[i]);
                    Console.WriteLine("From: " + fromCities[i]);
                    Console.WriteLine("To: " + toCities[i]);
                    Console.WriteLine("Departure: " + departureTimes[i]);
                    Console.WriteLine("Duration: " + durations[i] + " hours");
                    Console.WriteLine("Price: $" + fare[i]);

                    Console.WriteLine("Bookings for this flight:");
                    for (int j = 0; j < bookedFlightCodes.Count; j++)
                    {
                        if (bookedFlightCodes[j] == flightCodes[i])
                        {
                            Console.WriteLine($"- {passengerNames[j]} (Booking ID: {bookingIDs[j]})");
                        }
                    }
                    Console.WriteLine();
                }
            }
        }

        //============== Fare Calculation =================
        static int CalculateFare(int basePrice, int numTickets)
        {
            return basePrice * numTickets; // Calculate total fare if (int)
        }

        static double CalculateFare(double basePrice, int numTickets)
        {
            return basePrice * numTickets; // Calculate total fare if (double)
        }

        static int CalculateFare(int basePrice, int numTickets, int discount)
        {
            int total = basePrice * numTickets;
            return total - (total * discount / 100); // Calculate total fare with discount
        }

        //================ Confirm Action =================
        static bool ConfirmAction(string action)
        {
            Console.Write($"Are you sure you want to {action}? (y/n): "); 
            string input = Console.ReadLine().ToLower();
            while (input != "y" && input != "n") //check the input 
            {
                Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
                input = Console.ReadLine().ToLower(); 
            }
            return input == "y";
        }
        //================ Main Function =================
        static void Main(string[] args)
        {
            {
                StartSystem(); // Start the system
            }
        }



    }
}
