using Inheritance_and_Polymorphism.Interfaces;

namespace Inheritance_and_Polymorphism.Models;

public class Pet: Animal, IRegistrable
{
    public string Race { get; set; }
    private string Owner { get; set; }

    public Pet(string petName, string species, string race, int age, string owner): base(petName, age, species)
    {
        Race = race;
        Owner = owner;
    }

    public void ShowPetInfo()
    {
        Console.WriteLine($@"
        Pet Name: {Name}
        Species: {Species}
        Race: {Race}
        Age: {Age}");
    }

    public override void MakeNoise()
    {
        if (Species == "dog")
        {
            Console.WriteLine("The dog Barks!");
        } else if (Species == "cat")
        {
            Console.WriteLine("The cat Meows!");
        }
        else
        {
            Console.WriteLine($"This {Species} makes a weird noise...");
        }
    }
    
    public void Register()
    {
        Console.WriteLine("Pets cannot register by themselves...");
    }
}