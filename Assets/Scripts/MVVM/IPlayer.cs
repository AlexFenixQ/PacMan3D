using UnityEngine;

public interface IPlayer
{
    void Move(Vector3 direction);
    void SetEnabled(bool enabled);
    Vector3 GetPosition();
}