using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardMovement : MonoBehaviour,DragDropMouse.IDragDropActions
{
    public event Action<Vector3> OnHovered;

    public event Action<bool> OnDrag;

    [HideInInspector] public bool IsSelected = false;

    [HideInInspector] public BattleSites BattleSiteToGoTo;

    [SerializeField] private GameObject _unit;

    //public bool IsAnotherCardSelected = false;

    DragDropMouse _input;

    Camera _cam;
    bool _isBeingDragged;
    Vector3 _offset;

    [SerializeField] float _moveSpeed = 10f;
    Vector3 _positionForCardToGoTo;
    Vector3 _posInHand;

    public int HandIndex;
    public int TemporaryHandIndex;

    private GameObject _displayCopy;

    void Awake()
    {
        _cam = Camera.main;

        _input = new DragDropMouse();
        _input.DragDrop.SetCallbacks(this);   // THIS LINKS THE ACTIONS
    }

    void OnEnable()
    {
        _input.Enable();
    }

    void OnDisable()
    {
        _input.Disable();
    }

    public void AssignHandPosititon(Vector3 posInHand) { _posInHand = posInHand; }

    public void AssignPositionForCardToGo(Vector3 posToGoTo) { _positionForCardToGoTo = posToGoTo; }

    public void ToggleCardCollider(bool enabled)
    {
        if (_isBeingDragged) return;
        GetComponent<Collider2D>().enabled = enabled;
        //IsAnotherCardSelected = !enabled;
    }

    public void OnClicking(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
            StartDrag(ctx);
        if (ctx.canceled)
            StopDrag(ctx);
    }

    public void OnTracking(InputAction.CallbackContext ctx)
    {
        // This event fires constantly while moving the mouse
    }

    public void SelectCard()
    {
        IsSelected = true;

        if (_displayCopy == null)
        {
            _displayCopy = Instantiate(gameObject, transform.position, Quaternion.identity);

            _displayCopy.GetComponent<CardMovement>().enabled = false;

            _displayCopy.transform.position = new Vector3(transform.position.x, -3.2f, -3f);
            _displayCopy.transform.localScale = Vector3.one;
        }
        //transform.localScale = Vector3.one;
        //transform.position = new Vector3(transform.position.x, transform.position.y, -3f);
    }

    public void DeSelectCard()
    {
        IsSelected = false;

        if (_displayCopy != null)
        {
            Destroy(_displayCopy.gameObject);
        }
        
        //_isHoveredOver = false;
        //OnHovered?.Invoke(false);

        //transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        //transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    void StartDrag(InputAction.CallbackContext ctx)
    {
        Vector2 screenPos = _input.DragDrop.Tracking.ReadValue<Vector2>();
        Vector3 worldPos = ScreenToWorld(screenPos);

        Collider2D col = GetComponent<Collider2D>();
        if (col != null && col.OverlapPoint(worldPos))
        {
            _isBeingDragged = true;
            OnDrag?.Invoke(true);
            _offset = worldPos;
        }

        DeSelectCard();
    }

    void StopDrag(InputAction.CallbackContext ctx)
    {
        OnDrag?.Invoke(false);
        _isBeingDragged = false;
    }

    void Update()
    {
        MoveCard();

        CheckIfSelected();
    }

    private void MoveCard()
    {
        //if (IsAnotherCardSelected) return;

        if (!_isBeingDragged)
        {
            MoveCardToFixedPosition();
        }
        else
        {
            DragCard();
        }
    }

    private void CheckIfSelected()
    {
        if (Vector3.Distance(transform.position, _positionForCardToGoTo) < 0.1)
        {
            Vector2 screenPos = _input.DragDrop.Tracking.ReadValue<Vector2>();
            Vector3 worldPos = ScreenToWorld(screenPos);

            Collider2D col = GetComponent<Collider2D>();
            if (col != null && col.OverlapPoint(worldPos))
            {
                //_isHoveredOver = true;
                OnHovered?.Invoke(worldPos);
            }
            else
            {
                DeSelectCard();
            }
        }
    }

    private void MoveCardToFixedPosition()
    {
        if (_positionForCardToGoTo == Vector3.zero)
            _positionForCardToGoTo = _posInHand;

        transform.position = Vector3.Lerp(
            transform.position,
            _positionForCardToGoTo,
            Time.deltaTime * _moveSpeed
        );

        if (BattleSiteToGoTo != null)
        {
            if (Vector3.Distance(transform.position, _positionForCardToGoTo) < 0.1f)
            {
                BattleSiteToGoTo.SpawnUnit(_unit);
                DeSelectCard();
                Destroy(gameObject);
            }
        }
    }

    private void DragCard()
    {
        if (!IsSelected) return;

        //transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

        Vector2 screenPos = _input.DragDrop.Tracking.ReadValue<Vector2>();
        Vector3 worldPos = ScreenToWorld(screenPos);
        transform.position = worldPos/* + _offset*/;
    }

    Vector3 ScreenToWorld(Vector2 screenPos)
    {
        Vector3 pos = screenPos;
        pos.z = -_cam.transform.position.z;
        return _cam.ScreenToWorldPoint(pos);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{ 
    //    if (collision.TryGetComponent<ICardPlace>(out var cardPlace)) { Vector3 targetPos = cardPlace.CardEntered();
    //        if (targetPos != null) 
    //        { 
    //            _positionForCardToGoTo = targetPos;
    //        }
    //    }
    //}
    //private void OnTriggerExit2D(Collider2D collision) { if (collision.CompareTag("CardPlace")) { } }
}

