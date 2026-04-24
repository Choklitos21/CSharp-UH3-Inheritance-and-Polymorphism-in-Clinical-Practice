using Inheritance_and_Polymorphism.Interfaces;
using Inheritance_and_Polymorphism.Exceptions;
using Inheritance_and_Polymorphism.Utils;

namespace Inheritance_and_Polymorphism.Models;

// MULTIPLE INTERFACE IMPLEMENTATION:
// Patient implements both IRegistrable and INotificable.
// This is valid in C# because interfaces are contracts, not classes.
// A Patient CAN register a pet (IRegistrable) AND can receive notifications (INotificable).
// Neither capability is inherited from a base class — they are independently applied contracts.
//
// DESIGN NOTE — Why Patient is NOT abstract:
// We create direct Patient instances. If we needed base human types
// (e.g., PediatricPatient, SeniorPatient) with shared behavior, Patient could
// become abstract. For now, a concrete class is appropriate.
public class Patient : IRegistrable, INotificable
{
    public string Name { get; set; }
    public int Age { get; set; }
    private string Address { get; set; }

    // Protected so subclasses (if added later) can access phone without a public setter.
    protected string Phone { get; set; }

    public List<Pet> Pets { get; set; }

    public Patient(string name, int age, string address, string phone)
    {
        Name = name;
        Age = age;
        Address = address;
        Phone = phone;
        Pets = new List<Pet>();
    }

    public void ShowPatientInfo()
    {
        Console.WriteLine($"Patient  : {Name}");
        Console.WriteLine($"Age      : {Age}");
        Console.WriteLine($"Address  : {Address}");
        Console.WriteLine($"Phone    : {Phone}");

        if (Pets.Count == 0)
        {
            Console.WriteLine("Pets     : None registered.");
            return;
        }

        Console.WriteLine($"Pets ({Pets.Count}):");
        foreach (var pet in Pets)
            pet.ShowPetInfo();
    }

    // IRegistrable — synchronous interactive pet registration (unchanged from v1).
    // BREAKPOINT SUGGESTION: Place a breakpoint on the Pets.Add() line.
    // Inspect: petName (null-coalescing applied?), age (did TryParse succeed?),
    // Pets.Count before and after (did the list grow?).
    public void Register()
    {
        Logger.Info($"Starting pet registration for patient '{Name}'.");
        try
        {
            Console.Write("Pet name: ");
            string? petName = Console.ReadLine();

            Console.Write("Species : ");
            string? species = Console.ReadLine();

            Console.Write("Race    : ");
            string? race = Console.ReadLine();

            Console.Write("Age     : ");
            // DEBUGGING NOTE: int.TryParse returns false if input is non-numeric.
            // The out variable 'age' will be 0 on failure — inspect it after this line.
            bool parsed = int.TryParse(Console.ReadLine(), out int age);
            if (!parsed)
                Logger.Warning("Age input was not a valid integer — defaulting to 0.");

            var newPet = new Pet(
                petName ?? "Unknown",
                species ?? "Unknown",
                race    ?? "Unknown",
                age,
                Name
            );

            Pets.Add(newPet);
            Logger.Info($"Pet '{newPet.Name}' registered under patient '{Name}'.");
        }
        catch (Exception ex)
        {
            Logger.Error("Unexpected error during pet registration.", ex);
            throw; // Re-throw so the caller can decide how to handle it.
        }
    }

    // ASYNC OVERLOAD — RegisterAsync
    // WHY async here?
    //   The synchronous Register() works for a simple console flow.
    //   In a real system, adding a pet triggers I/O: validating species against an
    //   external veterinary database, writing the pet record via an HTTP API, etc.
    //   Making this async keeps the calling thread free during that I/O wait.
    //
    // NAMING CONVENTION: async methods ALWAYS end with "Async" (C# standard).
    // BEST PRACTICE: returns Task, never void — Task lets the caller await and observe exceptions.
    //
    // This method does NOT replace Register() — it extends the class with an
    // async-friendly path for scenarios that involve real I/O.
    public async Task RegisterAsync(string petName, string species, string race, int age)
    {
        Logger.Info($"[ASYNC] Starting async pet registration for patient '{Name}'.");
        Console.WriteLine($"[ASYNC] Validating and saving pet '{petName}'...");

        // Simulates: external species validation API + DB write latency.
        await Task.Delay(600);

        var newPet = new Pet(petName, species, race, age, Name);
        Pets.Add(newPet);

        Console.WriteLine($"[ASYNC] Pet '{newPet.Name}' registered under '{Name}'.");
        Logger.Info($"[ASYNC] Pet '{newPet.Name}' registered asynchronously under '{Name}'.");
    }

    // INotificable — synchronous notification (unchanged from v1).
    public void SendNotification()
    {
        Logger.Info($"Sending notification to patient '{Name}' at {Phone}.");
        Console.WriteLine($"[NOTIFICATION] Reminder sent to {Name} ({Phone}): You have an upcoming appointment.");
    }

    // Async variant of SendNotification for use in concurrent pipelines.
    // Follows the same INotificable contract intent but adds real I/O simulation
    // (e.g., SMTP call, push notification service).
    public async Task SendNotificationAsync()
    {
        Logger.Info($"[ASYNC] Dispatching async notification for '{Name}'.");
        await Task.Delay(300); // simulates notification gateway latency
        SendNotification();    // reuses existing sync logic — no duplication
    }

    // Finds a pet by name or throws a domain-specific exception if not found.
    // BREAKPOINT SUGGESTION: Step through the FirstOrDefault lambda.
    // Inspect: 'searchName' (correct casing?), 'found' (null or Pet object?).
    public Pet FindPetByName(string searchName)
    {
        var found = Pets.FirstOrDefault(
            p => p.Name.Equals(searchName, StringComparison.OrdinalIgnoreCase)
        );

        if (found == null)
            throw new PetNotFoundException(searchName);

        return found;
    }

    // Demonstrates a controlled division-by-zero error for debugging practice.
    // TASK 4: Place a breakpoint on the division line.
    // Watch variables: totalVisits (0?), divisor (also 0?), result (never reached).
    // In Visual Studio: Debug > Exceptions > CLR Exceptions — check DivideByZeroException.
    public double CalculateAverageVisits(int totalVisits)
    {
        int divisor = Pets.Count;

        // INTENTIONAL BUG for debugging exercise:
        // If no pets are registered, divisor is 0 — this throws DivideByZeroException.
        // Fix: add a guard like `if (divisor == 0) return 0;` before the division.
        int result = totalVisits / divisor;
        return result;
    }
}