namespace Inheritance_and_Polymorphism.Models;
using Interfaces;

public class Pet: Animal, IRegisterable
{
    public string Race { get; set; }
    private Patient Owner { get; set; }

    public Pet(string petName, string species, string race, int age, Patient owner): base(petName, age, species)
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
        if (Race == "dog")
        {
            Console.WriteLine("The dog Barks!");
        } else if (Race == "cat")
        {
            Console.WriteLine("The cat Meows!");
        }
        else
        {
            Console.WriteLine($"This {Race} makes a weird noise...");
        }
    }
    
    public void Register()
    {
        throw new NotImplementedException();
    }
}