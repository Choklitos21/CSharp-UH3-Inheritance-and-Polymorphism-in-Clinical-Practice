namespace Inheritance_and_Polymorphism.Models;

// DESIGN DECISION — Abstract class vs Interface for Animal:
// Animal is an abstract class (not an interface) because:
//   1. It holds shared STATE: Name, Age, Species — interfaces cannot hold fields.
//   2. It expresses an IS-A relationship: Pet IS-AN Animal.
//   3. MakeNoise() has a meaningful default behavior that subclasses can override.
// Rule of thumb: use abstract classes when sharing state/default behavior;
// use interfaces when defining a capability contract across unrelated types.
public abstract class Animal
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Species { get; set; }

    protected Animal(string name, int age, string species)
    {
        Name = name;
        Age = age;
        Species = species;
    }

    // virtual (not abstract): provides a sensible default.
    // Subclasses override only when they need species-specific behavior.
    public virtual void MakeNoise()
    {
        Console.WriteLine($"{Name} the {Species} makes a sound.");
    }
}