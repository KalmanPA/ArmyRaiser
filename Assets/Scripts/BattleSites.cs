using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleSites : MonoBehaviour
{
    private List<UnitBase> _playerUnits = new List<UnitBase>();

    private List<UnitBase> _enemyUnits = new List<UnitBase>();

    [Header("General")]
    [SerializeField] private int _maxRankNumber = 5;
    [SerializeField] private float _distanceBetweenUnits = 0.1f;

    [SerializeField] private float _unitHight = 0.715f;

    [SerializeField] private GameObject _cardHoverVisual;

    [Header("Enemy Side")]
    [SerializeField] GameObject _protEnemyCard;

    private void Start()
    {
        SpawnEnemyUnit(_protEnemyCard);
    }

    public void RunBattle()
    {
        int powerDiffernence = IsPlayerAttacking();

        if (powerDiffernence < 0)
        {
            //Run player Defence logic
            foreach (UnitBase unit in _playerUnits)
            {
                unit.DefendBehaviour();
            }

            //Run enemy attack logic
            foreach (UnitBase unit in _enemyUnits)
            {
                unit.AttackBehaviour();
            }

            //Deal damage
            DealCombatDamage(false, Mathf.Abs(powerDiffernence));
        }
        else if(powerDiffernence > 0)
        {
            //Run enemy Defence logic
            foreach (UnitBase unit in _enemyUnits)
            {
                unit.DefendBehaviour();
            }
            //Run player attack logic
            foreach (UnitBase unit in _playerUnits)
            {
                unit.AttackBehaviour();
            }
            //deal damage
            DealCombatDamage(true, Mathf.Abs(powerDiffernence));
        }





        SpawnEnemyUnit(_protEnemyCard);
    }

    private void DealCombatDamage(bool isPlayerAttacking, int damage)
    {
        if (isPlayerAttacking)
        {
            foreach (UnitBase unit in _enemyUnits)
            {
                if (!unit.IsDead)
                {
                    unit.DamageUnit(damage, out int overflowDamage);

                    damage = overflowDamage;

                    if (damage <= 0) return;
                }
            }


            //while (damage > 0 || i < _enemyUnits.Count)
            //{
            //    if (!_enemyUnits[i].IsDead)
            //    {
            //        _enemyUnits[i].DamageUnit(damage, out int overflowDamage);

            //        damage = overflowDamage;
            //    }

            //    i++;
            //}
        }
        else
        {
            foreach (UnitBase unit in _playerUnits)
            {
                if (!unit.IsDead)
                {
                    unit.DamageUnit(damage, out int overflowDamage);

                    damage = overflowDamage;

                    if (damage <= 0) return;
                }
            }
            //int i = 0;

            //while (damage > 0 || i < _playerUnits.Count)
            //{
            //    if (!_playerUnits[i].IsDead)
            //    {
            //        _playerUnits[i].DamageUnit(damage, out int overflowDamage);

            //        damage = overflowDamage;
            //    }

            //    i++;
            //}
        }
    }

    private int IsPlayerAttacking()
    {
        int playerPower = 0;
        int enemyPower = 0;

        foreach (UnitBase unit in _playerUnits)
        {
            if (!unit.IsDead)
            {
                playerPower += unit.Power;
            }
            
        }

        foreach (UnitBase unit in _enemyUnits)
        {
            if (!unit.IsDead)
            {
                enemyPower += unit.Power;
            }
        }

        return playerPower - enemyPower;
    }

    private void SpawnEnemyUnit(GameObject card)
    {

        GameObject spawnedUnit = Instantiate(card.GetComponent<CardMovement>().Unit, GetNextEnemyRankPosition(), Quaternion.identity);

        UnitBase unitScript = spawnedUnit.GetComponent<UnitBase>();

        unitScript.IntitailiseDisplayCard(card);

        _enemyUnits.Add(unitScript);

        unitScript.Rank = _enemyUnits.Count;
    }

    public void SpawnUnit(GameObject unit, GameObject card)
    {

        GameObject spawnedUnit = Instantiate(unit, GetNextPlayerRankPosition(), Quaternion.identity);

        UnitBase unitScript = spawnedUnit.GetComponent<UnitBase>();

        unitScript.IntitailiseDisplayCard(card);



        _playerUnits.Add(unitScript);

        unitScript.Rank = _playerUnits.Count;

        unitScript.DoEnterBattleSiteAbility();
    }

    private void CardHoverVisualIndication(bool isVisualOn)
    {
        _cardHoverVisual.SetActive(isVisualOn);
    }

    private Vector3 GetNextPlayerRankPosition()
    {
        float yPos = transform.position.y - 0.59f - (_playerUnits.Count * _unitHight) - (_playerUnits.Count * _distanceBetweenUnits);

        return new Vector3(transform.position.x, yPos, transform.position.z);
    }

    private Vector3 GetNextEnemyRankPosition()
    {
        float yPos = transform.position.y + 0.59f + (_enemyUnits.Count * _unitHight) + (_enemyUnits.Count * _distanceBetweenUnits);

        return new Vector3(transform.position.x, yPos, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Col detected");

        if (collision.TryGetComponent<CardMovement>(out var cardMovement))
        {
            if (!cardMovement.IsBeingDragged()) return;
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
            //if (!cardMovement.IsBeingDragged()) return;
            cardMovement.AssignPositionForCardToGo(Vector3.zero);
            cardMovement.BattleSiteToGoTo = null;
            CardHoverVisualIndication(false);
        }
    }

    
}
