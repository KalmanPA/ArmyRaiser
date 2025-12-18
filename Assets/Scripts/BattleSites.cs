using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleSites : MonoBehaviour
{
    private List<UnitBase> _playerUnits = new List<UnitBase>();

    private List<UnitBase> _enemyUnits = new List<UnitBase>();

    [SerializeField] private int _maxRankNumber = 5;
    [SerializeField] private float _distanceBetweenUnits = 1f;

    [SerializeField] private float _unitHight = 0.715f;

    [SerializeField] private GameObject _cardHoverVisual;

    internal void SpawnUnit(GameObject unit)
    {
        GameObject spawnedUnit = Instantiate(unit, GetNextPlayerRankPosition(), Quaternion.identity);
    }

    private void CardHoverVisualIndication(bool isVisualOn)
    {
        _cardHoverVisual.SetActive(isVisualOn);
    }

    private Vector3 GetNextPlayerRankPosition()
    {
        float yPos = transform.position.y - _distanceBetweenUnits - (_playerUnits.Count * _unitHight) - (_playerUnits.Count * _distanceBetweenUnits);

        return new Vector3(transform.position.x, yPos, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Col detected");

        if (collision.TryGetComponent<CardMovement>(out var cardMovement))
        {
            cardMovement.AssignPositionForCardToGo(GetNextPlayerRankPosition());
            cardMovement.BattleSiteToGoTo = this;
            CardHoverVisualIndication(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Col exited");

        if (collision.TryGetComponent<CardMovement>(out var cardMovement))
        {
            cardMovement.AssignPositionForCardToGo(Vector3.zero);
            cardMovement.BattleSiteToGoTo = null;
            CardHoverVisualIndication(false);
        }
    }

    
}
