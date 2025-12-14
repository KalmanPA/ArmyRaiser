using System;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private List<GameObject> _cardsInHand = new List<GameObject>();

    [SerializeField] private GameObject _testCard;

    [SerializeField] private Transform _cardDrawSpawnPosition;

    [SerializeField] private float _handWidth = 20;
    [SerializeField] private float _distanceBetweenCards = 2.6f;

    private void Start()
    {
        Debug.Log("HandStart");

        DrawCard();
        DrawCard();
        DrawCard();
        DrawCard();
    }

    public void DrawCard()
    {
        GameObject card = Instantiate(_testCard, _cardDrawSpawnPosition.position, Quaternion.identity);

        AddCardToHand(card);
    }

    public void AddCardToHand(GameObject card)
    {
        _cardsInHand.Add(card);

        AdjustCardIndexesAndPositions();
    }

    private void AdjustCardIndexesAndPositions()
    {
        int count = _cardsInHand.Count;
        if (count == 0) return;

        // Center offset (this makes the middle card stay in the middle)
        float centerIndex = (count - 1) / 2f;

        for (int i = 0; i < count; i++)
        {
            // Horizontal position relative to center
            float offset = (i - centerIndex) * _distanceBetweenCards;

            // Optional clamp to hand width
            offset = Mathf.Clamp(offset, -_handWidth * 0.5f, _handWidth * 0.5f);

            Vector3 position = new Vector3(offset + gameObject.transform.position.x, gameObject.transform.position.y, 0f);

            // Assign to card
            _cardsInHand[i].GetComponent<CardMovement>().AssignHandPosititon(position);
        }
    }

    public void CardPlayed()
    {
        
    }
}
