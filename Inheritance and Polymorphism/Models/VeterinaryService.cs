namespace Inheritance_and_Polymorphism.Models;

public abstract class VeterinaryService
{
    public abstract void Attend();
}

public class GeneralInquiry: VeterinaryService
{
    public override void Attend()
    {
        Console.WriteLine("You're going to be attended now, what's your inquiry?");
    }
}

public class Vaccination : VeterinaryService
{
    public override void Attend()
    {
        Console.WriteLine("You're going to get vaccinated now");
    }
}