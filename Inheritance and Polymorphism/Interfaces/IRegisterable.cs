namespace Inheritance_and_Polymorphism.Interfaces;

// Use an interface here instead of an abstract class because:
// - "Registrable" is a capability/contract, not a shared identity.
// - Patient and Pet are completely different entity types; they share no state
//   or default behavior — only the guarantee that they can be registered.
// - Interfaces allow multiple contracts on a single class (e.g., Patient is
//   both IRegistrable and INotificable), which abstract classes cannot do.
public interface IRegistrable
{
    void Register();
}