namespace Inheritance_and_Polymorphism.Interfaces;

// IAttendible decouples the "can be attended" contract from the abstract class hierarchy.
// VeterinaryService stays as an abstract class because it represents a shared domain
// concept with potential shared state (e.g., duration, cost) in the future.
// Adding IAttendible allows non-VeterinaryService classes to also be "attendible"
// without forcing them into the inheritance chain — open for extension.
public interface IAttendible
{
    void Attend();
}