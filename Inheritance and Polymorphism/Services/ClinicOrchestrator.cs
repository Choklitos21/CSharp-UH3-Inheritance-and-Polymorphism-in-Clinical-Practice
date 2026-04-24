using Inheritance_and_Polymorphism.Models;
using Inheritance_and_Polymorphism.Utils;

namespace Inheritance_and_Polymorphism.Services;
// ═══════════════════════════════════════════════════════════════════════════════
// WHY ASYNC / AWAIT?
//
// Problem async solves:
//   In a synchronous (blocking) flow, the thread that calls an operation — like
//   reading from a database, sending an HTTP request, or writing a file — sits
//   idle until that operation finishes. In a UI or web server context this means
//   the application freezes or cannot serve other requests.
//
// Sync vs Async:
//   SYNC:  Thread A calls SaveToDatabase() → thread is BLOCKED for 500 ms → returns.
//          Nothing else can run on Thread A during those 500 ms.
//
//   ASYNC: Thread A calls SaveToDatabaseAsync() → hits 'await' → thread is RELEASED
//          back to the pool → other work runs → when the I/O completes the runtime
//          resumes on a free thread right after the await.
//
// When to use async/await in THIS system:
//   ✔ Saving a patient record to a database (simulated here with Task.Delay)
//   ✔ Sending an email/SMS notification over the network
//   ✔ Loading a patient's medical history from a remote service
//   ✔ Scheduling an appointment through an external calendar API
//   ✗ Pure CPU math (e.g., CalculateAverageVisits) — no I/O, no benefit
//   ✗ Simple in-memory list operations — overhead is not worth it
// ═══════════════════════════════════════════════════════════════════════════════

public class ClinicOrchestrator
{
    // ── Task 1 — Simple illustrative async example ────────────────────────────
    // This standalone demo shows the difference between blocking and non-blocking
    // execution before touching any real domain logic.
    public async Task DemoAsyncConceptAsync()
    {
        Logger.Info("[DEMO] Async concept illustration started.");

        // Without async: the thread would sleep here and block everything.
        // With async: the thread is released while Task.Delay counts down.
        Console.WriteLine("[DEMO] Step 1 — before the simulated I/O call.");
        await Task.Delay(800); // simulates a network/DB call taking 800 ms
        Console.WriteLine("[DEMO] Step 2 — resumed after the simulated I/O call.");

        Logger.Info("[DEMO] Async concept illustration complete.");
    }

    // ── Task 2 — RegisterPatientAsync ────────────────────────────────────────
    // Async version of patient registration.
    // Integrates with the existing Patient model and Logger from the previous sprint.
    //
    // Real-world analogy: calling an API to persist the patient record.
    // The await lets the calling thread do other work (e.g., keep the UI responsive)
    // while we "wait" for the database to confirm the write.
    public async Task<Patient> RegisterPatientAsync(string name, int age, string address, string phone)
    {
        Logger.Info($"[ASYNC] Registration started for '{name}'.");
        Console.WriteLine($"[ASYNC] Saving patient '{name}' to the system...");

        // Simulates the latency of a real database INSERT (~1 second).
        await Task.Delay(1000);

        var patient = new Patient(name, age, address, phone);

        Console.WriteLine($"[ASYNC] Patient '{name}' saved successfully.");
        Logger.Info($"[ASYNC] Registration complete for '{name}'.");

        return patient;
    }

    // ── Task 3 — Parallel clinic operations with Task.WhenAll ─────────────────
    // In a real clinic, loading history, scheduling, and sending notifications
    // are independent I/O operations. Running them in parallel cuts total wait
    // time from (sum of all delays) down to (slowest single delay).
    //
    // Task.WhenAll: waits for EVERY task to finish before continuing.
    // Use it when you need ALL results before the next step.
    public async Task RunParallelClinicOperationsAsync(Patient patient)
    {
        Logger.Info($"[PARALLEL] Starting parallel operations for patient '{patient.Name}'.");
        Console.WriteLine("\n[PARALLEL] Launching 3 concurrent clinic operations...");

        var loadHistory    = LoadMedicalHistoryAsync(patient.Name);
        var scheduleAppt   = ScheduleAppointmentAsync(patient.Name);
        var sendReminder   = SendNotificationAsync(patient);

        // DEBUGGING: set a breakpoint here and observe that all three tasks are
        // already running (their status will be WaitingForActivation or Running)
        // before WhenAll is even called — they start the moment they are created.
        await Task.WhenAll(loadHistory, scheduleAppt, sendReminder);

        Logger.Info($"[PARALLEL] All operations completed for '{patient.Name}'.");
        Console.WriteLine("[PARALLEL] All 3 operations finished.\n");
    }

    // Task.WhenAny: returns as soon as the FIRST task completes.
    // Use it for timeout patterns or when you only need one result (e.g., fastest server).
    public async Task DemoWhenAnyAsync()
    {
        Console.WriteLine("\n[WHEN-ANY] Racing 3 operations — first to finish wins...");

        var fast   = SimulateOperationAsync("Fast service",   400);
        var medium = SimulateOperationAsync("Medium service", 900);
        var slow   = SimulateOperationAsync("Slow service",  1500);

        // EXECUTION NOTE: the other two tasks keep running in the background;
        // WhenAny just stops waiting once the first one is done.
        var winner = await Task.WhenAny(fast, medium, slow);
        Console.WriteLine($"[WHEN-ANY] First to complete: '{await winner}'");

        // Best practice: await the remaining tasks if you care about their completion
        // or need to observe exceptions. Ignoring them can hide errors.
        await Task.WhenAll(fast, medium, slow);
        Console.WriteLine("[WHEN-ANY] All remaining tasks also finished.\n");
    }

    // ── Task 4 — Concurrent patient attendance ────────────────────────────────
    // Simulates multiple patients being attended simultaneously, which is realistic
    // in a clinic with several exam rooms / vets working in parallel.
    //
    // CONCURRENCY NOTE: Task.Run offloads work to the thread pool.
    // Use it for CPU-bound or legacy synchronous code that can't be awaited directly.
    // For true I/O-bound work (HTTP, DB) prefer async methods without Task.Run.
    public async Task AttendMultiplePatientsAsync(List<Patient> patients)
    {
        Logger.Info($"[CONCURRENCY] Attending {patients.Count} patients concurrently.");
        Console.WriteLine($"\n[CONCURRENCY] Starting concurrent attendance for {patients.Count} patients...");

        // Each patient attendance is an independent background task.
        // DEBUGGING: watch the interleaved console output — the order is non-deterministic
        // because tasks run on the thread pool, not sequentially.
        var attendanceTasks = patients.Select(p => AttendSinglePatientAsync(p)).ToList();

        await Task.WhenAll(attendanceTasks);

        Console.WriteLine("[CONCURRENCY] All patients have been attended.\n");
        Logger.Info("[CONCURRENCY] Concurrent attendance session complete.");
    }

    // Simulates concurrent pet registration for a single patient.
    // Each pet registration (e.g., health check + vaccine lookup) runs in parallel.
    public async Task RegisterPetsConcurrentlyAsync(Patient patient, List<Pet> pets)
    {
        Logger.Info($"[CONCURRENCY] Registering {pets.Count} pets concurrently for '{patient.Name}'.");
        Console.WriteLine($"\n[CONCURRENCY] Registering {pets.Count} pets in parallel...");

        var registrationTasks = pets.Select(pet => RegisterSinglePetAsync(patient, pet)).ToList();

        await Task.WhenAll(registrationTasks);

        Console.WriteLine("[CONCURRENCY] All pets registered.\n");
        Logger.Info($"[CONCURRENCY] Concurrent pet registration complete for '{patient.Name}'.");
    }

    // ── Private helpers ───────────────────────────────────────────────────────

    private async Task LoadMedicalHistoryAsync(string patientName)
    {
        Console.WriteLine($"  [HISTORY]  Loading medical history for '{patientName}'...");
        await Task.Delay(700); // simulates DB read
        Console.WriteLine($"  [HISTORY]  History loaded for '{patientName}'.");
        Logger.Info($"Medical history loaded for '{patientName}'.");
    }

    private async Task ScheduleAppointmentAsync(string patientName)
    {
        Console.WriteLine($"  [SCHEDULE] Scheduling appointment for '{patientName}'...");
        await Task.Delay(500); // simulates calendar API call
        Console.WriteLine($"  [SCHEDULE] Appointment scheduled for '{patientName}'.");
        Logger.Info($"Appointment scheduled for '{patientName}'.");
    }

    // Bridges the existing synchronous INotificable.SendNotification() into async context.
    // This is the correct way to integrate legacy sync code: wrap it, don't break it.
    private async Task SendNotificationAsync(Patient patient)
    {
        Console.WriteLine($"  [NOTIFY]   Sending notification to '{patient.Name}'...");
        await Task.Delay(300); // simulates SMTP/SMS gateway latency
        patient.SendNotification(); // calls existing INotificable implementation
        Logger.Info($"Async notification dispatched for '{patient.Name}'.");
    }

    private async Task<string> SimulateOperationAsync(string label, int delayMs)
    {
        await Task.Delay(delayMs);
        Console.WriteLine($"  [SIM] '{label}' completed after {delayMs} ms.");
        return label;
    }

    private async Task AttendSinglePatientAsync(Patient patient)
    {
        // Task.Run is used here to simulate a CPU-bound or legacy sync check-in process.
        await Task.Run(() =>
        {
            Logger.Info($"[ATTEND] Starting attendance for '{patient.Name}'.");
            Console.WriteLine($"  [ATTEND] Attending '{patient.Name}'...");
        });

        await Task.Delay(new Random().Next(300, 900)); // variable clinic time per patient
        Console.WriteLine($"  [ATTEND] '{patient.Name}' attendance complete.");
        Logger.Info($"[ATTEND] '{patient.Name}' finished.");
    }

    private async Task RegisterSinglePetAsync(Patient patient, Pet pet)
    {
        Console.WriteLine($"  [PET REG] Processing '{pet.Name}' ({pet.Species})...");
        await Task.Delay(400); // simulates vaccine record lookup
        patient.Pets.Add(pet);
        Console.WriteLine($"  [PET REG] '{pet.Name}' added to '{patient.Name}'.");
        Logger.Info($"Pet '{pet.Name}' registered asynchronously under '{patient.Name}'.");
    }
}