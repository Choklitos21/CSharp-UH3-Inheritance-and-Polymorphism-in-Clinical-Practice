namespace Inheritance_and_Polymorphism.Interfaces;

// INotificable is a separate, single-responsibility interface.
// Not every IRegistrable entity needs notifications (e.g., Pet doesn't need them),
// so splitting this out avoids the "fat interface" anti-pattern.
// Patient implements both IRegistrable and INotificable — demonstrating
// that multiple interface implementation is clean and composable in C#.
public interface INotificable
{
    void SendNotification();
}