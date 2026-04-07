using Inheritance_and_Polymorphism.Models;

Patient? patient = null;
VeterinaryService generalInquiry = new GeneralInquiry();
VeterinaryService vaccination = new Vaccination();

bool flag = true;
while (flag)
{
    Console.WriteLine(@"
    ----------- MENU -----------
    1. Register a Patient automatically
    2. Register 3 Pets into that Patient
    3. Show Patient and his Pets info
    4. Show VeterinaryService for abstract methods
    5. Show how each Pet make noise according to their species
    6. EXIT
    Choose an option: 
    ");
    string? option = Console.ReadLine();
    switch (option)
    {
        case "1":
            PatientRegister();
            break;
        case "2":
            RegisterPets();
            break;
        case "3":
            if (patient == null) {
                Console.WriteLine("Try to register the patient first");
                break;
            }
            patient.ShowPatientInfo();
            break;
        case "4":
            generalInquiry.Attend();
            vaccination.Attend();
            break;
        case "5":
            if (patient == null || patient.Pets == null) {
                Console.WriteLine("Try to register the patient and his pets first");
                break;
            }
            patient.Pets[0].MakeNoise();
            patient.Pets[1].MakeNoise();
            patient.Pets[2].MakeNoise();
            break;
        case "6":
            Console.WriteLine("Bye bye...");
            flag = false;
            break;
        default:
            Console.WriteLine("Option not valid...");
            break;
    }
}

void PatientRegister()
{
    patient = new Patient("Pepe", 25, "1245 Pasadena, LA 05524", "+57 3003104567");
    Console.WriteLine("Patient registered");
}

void RegisterPets()
{
    if (patient == null)
    {
        Console.WriteLine("Try to register the patient first");
        return;
    };
    patient.Pets?.Add(new Pet("Goofy", "dog", "Border Collie", 4, patient.Name));
    patient.Pets?.Add(new Pet("Sarita", "cat", "Maine Coon", 10, patient.Name));
    patient.Pets?.Add(new Pet("Masacre", "Hámster", "Syrian hamster", 1, patient.Name));
    Console.WriteLine("Pets registered");
}