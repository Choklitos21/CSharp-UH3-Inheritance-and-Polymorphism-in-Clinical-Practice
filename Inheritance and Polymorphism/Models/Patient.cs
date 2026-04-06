namespace Inheritance_and_Polymorphism.Models;
using Interfaces;

public class Patient: IRegisterable
{
    public string Name { get; set; }
    public int Age { get; set; }
    private string _address { get; set; }
    protected int _phone { get; set; }
    public List<Pet>? Pets { get; set; }

    public Patient(string name, int age, string address, int phone)
    {
        Name = name;
        Age = age;
        _address = address;
        _phone = phone;
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
        throw new NotImplementedException();
    }
}