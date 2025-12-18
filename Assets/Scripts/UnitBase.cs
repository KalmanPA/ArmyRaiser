using UnityEngine;

public abstract class UnitBase : MonoBehaviour
{
    [HideInInspector] public int Rank;
    [HideInInspector] public GameObject UnitCard;

    private const int MIN_POWER = 0;
    private const int MAX_POWER = 99;

    [SerializeField]
    private int _power;

    public int Power
    {
        get => _power;
        protected set
        {
            int clampedValue = Mathf.Clamp(value, MIN_POWER, MAX_POWER);

            if (_power == clampedValue)
                return;

            _power = clampedValue;

            if (_power == MIN_POWER)
            {
                Death();
            }
        }
    }

    protected virtual void Death()
    {
        // Base death behavior (optional)
        // Can be overridden
    }

    public void DamageUnit(int amount)
    {
        if (amount <= 0)
            return;

        Power -= amount;
    }

    public virtual void OnEnterBattleSite()
    {
        // Intentionally empty
    }

    public virtual void OnAttack()
    {
        // Intentionally empty
    }

    public virtual void OnDefend()
    {
        // Intentionally empty
    }
}
