using UnityEngine;

public interface ICardPlace
{
    Vector3 CardEntered();
    void CardExited(Transform card);
}
