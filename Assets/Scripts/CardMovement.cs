using UnityEngine;
using UnityEngine.InputSystem;

public class CardMovement : MonoBehaviour,DragDropMouse.IDragDropActions
{
    DragDropMouse _input;

    Camera _cam;
    bool _dragging;
    Vector3 _offset;

    [SerializeField] float _moveSpeed = 10f;
    Vector3 _positionForCardToGoTo;
    Vector3 _posInHand;

    public int HandIndex;
    public int TemporaryHandIndex;

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

    void StartDrag(InputAction.CallbackContext ctx)
    {
        Vector2 screenPos = _input.DragDrop.Tracking.ReadValue<Vector2>();
        Vector3 worldPos = ScreenToWorld(screenPos);

        Collider2D col = GetComponent<Collider2D>();
        if (col != null && col.OverlapPoint(worldPos))
        {
            _dragging = true;
            _offset = transform.position - worldPos;
        }
    }

    void StopDrag(InputAction.CallbackContext ctx)
    {
        _dragging = false;
    }

    void Update()
    {
        if (!_dragging)
        {
            if (_positionForCardToGoTo == Vector3.zero)
                _positionForCardToGoTo = _posInHand;

            transform.position = Vector3.Lerp(
                transform.position,
                _positionForCardToGoTo,
                Time.deltaTime * _moveSpeed
            );
        }
        else
        {
            Vector2 screenPos = _input.DragDrop.Tracking.ReadValue<Vector2>();
            Vector3 worldPos = ScreenToWorld(screenPos);
            transform.position = worldPos + _offset;
        }
    }

    Vector3 ScreenToWorld(Vector2 screenPos)
    {
        Vector3 pos = screenPos;
        pos.z = -_cam.transform.position.z;
        return _cam.ScreenToWorldPoint(pos);
    }

    private void OnTriggerEnter2D(Collider2D collision) { if (collision.TryGetComponent<ICardPlace>(out var cardPlace)) { Vector3 targetPos = cardPlace.CardEntered(); if (targetPos != null) { _positionForCardToGoTo = targetPos; } } }
    private void OnTriggerExit2D(Collider2D collision) { if (collision.CompareTag("CardPlace")) { } }
}

