using Inheritance_and_Polymorphism.Interfaces;
using Inheritance_and_Polymorphism.Utils;

namespace Inheritance_and_Polymorphism.Models;

// DESIGN DECISION — Abstract class for VeterinaryService:
// VeterinaryService stays as an abstract class (not an interface) because:
//   - It models a shared IDENTITY: all services are VeterinaryServices.
//   - It can later hold shared state (e.g., Duration, Cost, ServiceCode).
//   - Attend() must be overridden by each concrete service — abstract enforces this.
//
// We also add IAttendible so external code can reference any attendible thing
// (including future non-service types) via the interface, without depending on
// the concrete class hierarchy. This follows the Dependency Inversion Principle.
public abstract class VeterinaryService : IAttendible
{
    // Shared property — concrete services inherit this automatically.
    public string ServiceName { get; protected set; } = "General Service";

    // abstract forces every subclass to provide its own implementation.
    public abstract void Attend();
}

public class GeneralConsultation : VeterinaryService
{
    public GeneralConsultation()
    {
        ServiceName = "General Consultation";
    }

    public override void Attend()
    {
        Logger.Info($"Service started: {ServiceName}");
        Console.WriteLine("Welcome! The vet is ready for your consultation.");
    }
}

public class Vaccination : VeterinaryService
{
    public string VaccineType { get; set; }

    public Vaccination(string vaccineType = "Standard")
    {
        ServiceName = "Vaccination";
        VaccineType = vaccineType;
    }

    public override void Attend()
    {
        Logger.Info($"Service started: {ServiceName} ({VaccineType})");
        Console.WriteLine($"Preparing {VaccineType} vaccine. Please keep your pet calm.");
    }
}