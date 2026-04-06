namespace Inheritance_and_Polymorphism.Models;

public class Animal
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Species { get; set; }

    public Animal(string petName, int age, string species)
    {
        Name = petName;
        Age = age;
        Species = species;
    }

    public virtual void MakeNoise()
    {
        Console.WriteLine("Animals make noise");
    }
}