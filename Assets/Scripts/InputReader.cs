using System;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, DragDropMouse.IDragDropActions
{
    public event Action ClickEvent;

    public void OnClicking(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        ClickEvent?.Invoke();
    }

    public void OnTracking(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }
}
