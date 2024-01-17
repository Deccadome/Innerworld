public interface IPickup
{
    void EnablePrompt();
    void DisablePrompt();
    void Pickup();
    void Drop();
    string GetLabel();
}