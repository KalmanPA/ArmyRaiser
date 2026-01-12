using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class UnitBase : MonoBehaviour, DragDropMouse.IDragDropActions
{
    [HideInInspector] public bool IsDead;

    //public bool IsLeavingCorpse;

    public event Action<int> OnPowerChanged;

    public event Action OnDisplayCardInitailised;

    [HideInInspector] public int Rank;
    //[HideInInspector] public GameObject UnitCard;
    [HideInInspector] public GameObject DisplayUnitCard;

    private const int MIN_POWER = 0;
    private const int MAX_POWER = 99;

    [SerializeField]
    private int _power;

    DragDropMouse _input;

    Camera _cam;

    public int Power
    {
        get => _power;
        protected set
        {
            int clampedValue = Mathf.Clamp(value, MIN_POWER, MAX_POWER);

            if (_power == clampedValue)
                return;

            _power = clampedValue;

            OnPowerChanged?.Invoke(_power);

            if (_power == MIN_POWER)
            {
                Death();
            }
        }
    }

    void Awake()
    {
        _cam = Camera.main;

        _input = new DragDropMouse();
        _input.DragDrop.SetCallbacks(this);   // THIS LINKS THE ACTIONS
    }

    private void Update()
    {
        CheckForPointerHoveringOnUnit();
    }

    

    void OnEnable()
    {
        _input.Enable();
    }

    void OnDisable()
    {
        _input.Disable();
    }

    private void CheckForPointerHoveringOnUnit()
    {
        Vector2 screenPos = _input.DragDrop.Tracking.ReadValue<Vector2>();
        Vector3 worldPos = ScreenToWorld(screenPos);

        Collider2D col = GetComponent<Collider2D>();
        if (col != null && col.OverlapPoint(worldPos))
        {
            ToggleCardToDisplay(true);
        }
        else
        {
            ToggleCardToDisplay(false);
        }
    }

    private void ToggleCardToDisplay(bool state)
    {
        if (DisplayUnitCard != null)
        {
            DisplayUnitCard.gameObject.SetActive(state);
        }
        else
        {
            Debug.LogWarning("Card to Display is uassigned");
        }
    }

    public void IntitailiseDisplayCard(GameObject unitCard)
    {
        DisplayUnitCard = Instantiate(unitCard, transform.position, Quaternion.identity);
        DisplayUnitCard.transform.localScale = Vector3.one;

        DisplayUnitCard.GetComponent<CardMovement>().enabled = false;
        DisplayUnitCard.gameObject.SetActive(false);

        OnDisplayCardInitailised?.Invoke();
    }

    Vector3 ScreenToWorld(Vector2 screenPos)
    {
        Vector3 pos = screenPos;
        pos.z = -_cam.transform.position.z;
        return _cam.ScreenToWorldPoint(pos);
    }

    protected virtual void Death()
    {
        // Can be overridden

        IsDead = true;
        transform.Rotate(0f, 0f, -90f);
    }

    public void DamageUnit(int damage, out int overflowDamage)
    {
        int appliedDamage = Mathf.Min(damage, Power);
        Power -= appliedDamage;
        overflowDamage = damage - appliedDamage;
    }

    public virtual void DoEnterBattleSiteAbility()
    {
        // Intentionally empty
    }

    public virtual void AttackBehaviour()
    {
        // Intentionally empty
    }

    public virtual void DefendBehaviour()
    {
        // Intentionally empty
    }

    public void OnTracking(InputAction.CallbackContext context)
    {
        
    }

    public void OnClicking(InputAction.CallbackContext context)
    {
        
    }
}
