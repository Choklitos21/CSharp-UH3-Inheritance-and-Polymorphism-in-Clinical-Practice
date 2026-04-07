using System.Threading.Channels;

namespace Inheritance_and_Polymorphism.Models;
using Interfaces;

public class Patient: IRegisterable
{
    public string Name { get; set; }
    public int Age { get; set; }
    private string _address { get; set; }
    protected string _phone { get; set; }
    public List<Pet>? Pets { get; set; }

    public Patient(string name, int age, string address, string phone)
    {
        Name = name;
        Age = age;
        _address = address;
        _phone = phone;
        Pets = new List<Pet>();
    }

    public void ShowPatientInfo()
    {
        Console.WriteLine($@"Patient {Name} with {Age} years old, lives at {_address} and can be contacted on {_phone}");
        if (Pets == null )
        {
            Console.WriteLine("Has no pets registered at this time");
            return;
        }
        Console.WriteLine($"Which has {Pets.Count} pets");
        foreach (var pet in Pets)
        {
            pet.ShowPetInfo();
        }
    }

    public void Register()
    {
        Console.WriteLine("Register a Pet");
        Console.WriteLine("What's your pet name?: ");
        string? petName = Console.ReadLine();
        Console.WriteLine("What is your pets species?: ");
        string? species = Console.ReadLine();
        Console.WriteLine("What is your pets race?: ");
        string? race = Console.ReadLine();
        Console.WriteLine("What is your pets species?: ");
        int.TryParse(Console.ReadLine(), out int age);
        Console.WriteLine("What is your pets species?: ");
        Pets.Add(new Pet(petName ?? "None", species ?? "None", race ?? "None", age, Name));
    }
}