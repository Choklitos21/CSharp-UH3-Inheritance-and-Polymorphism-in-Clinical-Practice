using Inheritance_and_Polymorphism.Models;
using Inheritance_and_Polymorphism.Interfaces;
using Inheritance_and_Polymorphism.Exceptions;
using Inheritance_and_Polymorphism.Utils;
using Inheritance_and_Polymorphism.Services;

// ── ASYNC ENTRY POINT ─────────────────────────────────────────────────────────
// We call RunAsync() to keep all awaitable logic inside a proper async method,
// avoiding the need for .Wait() or .Result anywhere (Task 5 best practice).
await RunAsync();

async Task RunAsync()
{
    Logger.Info("Veterinary System started.");

    // Auto-register patient and pets on startup — no manual selection needed.
    Patient patient = new Patient("Pepe Gomez", 35, "Calle 10 #45-20, Envigado", "+57 300 310 4567");
    patient.Pets.Add(new Pet("Goofy",   "dog",     "Border Collie",  4, patient.Name));
    patient.Pets.Add(new Pet("Sarita",  "cat",     "Maine Coon",    10, patient.Name));
    patient.Pets.Add(new Pet("Masacre", "hamster", "Syrian Hamster", 1, patient.Name));
    Logger.Info($"Patient '{patient.Name}' and 3 pets auto-registered.");
    Console.WriteLine($"✔ Patient '{patient.Name}' and 3 pets registered automatically.\n");

    IAttendible generalConsultation = new GeneralConsultation();
    IAttendible vaccination         = new Vaccination("Rabies");
    var orchestrator                = new ClinicOrchestrator();

    bool running = true;
    while (running)
    {
        Console.WriteLine(@"
    ─────────── VETERINARY SYSTEM v2 ───────────
    ── Features ──
    1.  Show patient and pets info
    2.  Run veterinary services (IAttendible)
    3.  Make pets produce noise (polymorphism)
    4.  Send appointment notification (INotificable)
    5.  Find a pet by name (exception demo)
    6.  Demo: division-by-zero debugging

    ── Async features ──
    7.  DEMO: async concept illustration (Task 1)
    8.  Register patient asynchronously (Task 2)
    9.  Run parallel clinic operations (Task 3 — WhenAll)
    10. WhenAny race demo (Task 3 — WhenAny)
    11. Attend multiple patients concurrently (Task 4)
    12. Register pets concurrently (Task 4)

    0.  EXIT
    ─────────────────────────────────────────────
    Option: ");

        switch (Console.ReadLine()?.Trim())
        {
            case "1":
                patient.ShowPatientInfo();
                break;

            case "2":
                generalConsultation.Attend();
                vaccination.Attend();
                break;

            case "3":
                foreach (var pet in patient.Pets)
                    pet.MakeNoise();
                break;

            case "4":
                INotificable notificable = patient;
                notificable.SendNotification();
                break;

            case "5":
                FindPetDemo(patient);
                break;

            case "6":
                DebugDivisionDemo(patient);
                break;

            // Task 1 — illustrates async concept with a simple delay
            case "7":
                await orchestrator.DemoAsyncConceptAsync();
                break;

            // Task 2 — async patient registration with simulated DB latency
            case "8":
                // BEST PRACTICE: await the result directly — no .Result or .Wait().
                var asyncPatient = await orchestrator.RegisterPatientAsync(
                    "Maria Lopez", 28, "Carrera 43A #1-50, Medellín", "+57 311 222 3344"
                );
                Console.WriteLine($"Async patient ready: {asyncPatient.Name}");
                break;

            // Task 3 — Task.WhenAll: all three operations run in parallel
            case "9":
                await orchestrator.RunParallelClinicOperationsAsync(patient);
                break;

            // Task 3 — Task.WhenAny: first operation to finish wins
            case "10":
                await orchestrator.DemoWhenAnyAsync();
                break;

            // Task 4 — concurrent attendance of multiple patients
            case "11":
                var concurrentPatients = new List<Patient>
                {
                    new Patient("Carlos Rios",  40, "Calle 80 #12-34, Bello",     "+57 312 000 1111"),
                    new Patient("Ana Gomez",    32, "Av. El Poblado #5-10",       "+57 314 000 2222"),
                    new Patient("Luis Torres",  55, "Transv. 39 #74-100, Itagüí", "+57 315 000 3333"),
                };
                await orchestrator.AttendMultiplePatientsAsync(concurrentPatients);
                break;

            // Task 4 — concurrent pet registration under the current patient
            case "12":
                var petsToRegister = new List<Pet>
                {
                    new Pet("Rocky",  "dog",  "German Shepherd", 3, patient.Name),
                    new Pet("Luna",   "cat",  "Persian",         5, patient.Name),
                    new Pet("Tweety", "bird", "Canary",          1, patient.Name),
                };
                await orchestrator.RegisterPetsConcurrentlyAsync(patient, petsToRegister);
                patient.ShowPatientInfo();
                break;

            case "0":
                Logger.Info("Veterinary System v2 shutting down.");
                Console.WriteLine("Goodbye!");
                running = false;
                break;

            default:
                Logger.Warning("Invalid menu option entered.");
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }
}

// ── Helper methods ────────────────────────────────────────────────────────────

void FindPetDemo(Patient patient)
{
    Console.Write("Enter pet name to search: ");
    string? input = Console.ReadLine();

    try
    {
        Pet found = patient.FindPetByName(input ?? "");
        Console.WriteLine($"Found: {found.Name} ({found.Species})");
        Logger.Info($"Pet '{found.Name}' found.");
    }
    catch (PetNotFoundException ex)
    {
        Logger.Error(ex.Message);
        Console.WriteLine($"[ERROR] {ex.Message}");
    }
    catch (Exception ex)
    {
        Logger.Error("Unexpected error during pet search.", ex);
        Console.WriteLine("[ERROR] Something unexpected went wrong. Check the log.");
    }
    finally
    {
        Console.WriteLine("[INFO] Pet search operation completed.");
    }
}

void DebugDivisionDemo(Patient patient)
{
    try
    {
        double avg = patient.CalculateAverageVisits(10);
        Console.WriteLine($"Average visits per pet: {avg:F2}");
    }
    catch (DivideByZeroException ex)
    {
        Logger.Error("Division by zero in CalculateAverageVisits — no pets registered.", ex);
        Console.WriteLine("[ERROR] Cannot calculate average: patient has no pets registered.");
    }
    finally
    {
        Console.WriteLine("[INFO] Calculation attempt finished.");
    }
}