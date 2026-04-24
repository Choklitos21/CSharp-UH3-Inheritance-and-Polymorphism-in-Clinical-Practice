namespace Inheritance_and_Polymorphism.Exceptions;

// Domain-specific exception: thrown when a pet lookup fails.
// Inheriting from Exception (not ApplicationException) follows modern .NET guidelines.
// Always provide: a default message, a parameterized message, and the inner-exception
// constructor chain — this is the standard "exception trilogy" pattern.
public class PetNotFoundException : Exception
{
    public PetNotFoundException()
        : base("Pet was not found in the system.") { }

    public PetNotFoundException(string petName)
        : base($"Pet '{petName}' was not found in the patient's records.") { }

    public PetNotFoundException(string petName, Exception innerException)
        : base($"Pet '{petName}' was not found in the patient's records.", innerException) { }
}