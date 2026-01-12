using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleButton : MonoBehaviour,DragDropMouse.IDragDropActions
{
    DragDropMouse _input;

    Camera _cam;

    public event Action OnBattleTriggered;

    void Awake()
    {
        _cam = Camera.main;

        _input = new DragDropMouse();
        _input.DragDrop.SetCallbacks(this);   // THIS LINKS THE ACTIONS
    }

    private void Update()
    {
        //CheckForPointerHoveringOnButton();
    }

    public void OnClicking(InputAction.CallbackContext context)
    {
        if (context.started)
            ButtonPressed(context);
    }

    private void ButtonPressed(InputAction.CallbackContext context)
    {
        Vector2 screenPos = _input.DragDrop.Tracking.ReadValue<Vector2>();
        Vector3 worldPos = ScreenToWorld(screenPos);

        Collider2D col = GetComponent<Collider2D>();
        if (col != null && col.OverlapPoint(worldPos))
        {
            OnBattleTriggered?.Invoke();
        }
    }

    Vector3 ScreenToWorld(Vector2 screenPos)
    {
        Vector3 pos = screenPos;
        pos.z = -_cam.transform.position.z;
        return _cam.ScreenToWorldPoint(pos);
    }

    public void OnTracking(InputAction.CallbackContext context)
    {
        
    }

    private void CheckForPointerHoveringOnButton()
    {
        Vector2 screenPos = _input.DragDrop.Tracking.ReadValue<Vector2>();
        Vector3 worldPos = ScreenToWorld(screenPos);

        Collider2D col = GetComponent<Collider2D>();
        if (col != null && col.OverlapPoint(worldPos))
        {
            Debug.Log("BattlebuttonHovered");
        }
        else
        {
            ///ToggleCardToDisplay(false);
        }
    }

    void OnEnable()
    {
        _input.Enable();
    }

    void OnDisable()
    {
        _input.Disable();
    }
}
