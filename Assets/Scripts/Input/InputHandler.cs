using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public static event EventHandler OnCuttingStarted;
    public static event EventHandler OnCuttingCanceled;

    private PlayerInput _playerInput;
    private Camera _mainCamera;

    [Header("Cut Data")]
    public bool IsCutting;
    public Vector3 CurrentWorldPosition;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _mainCamera = Camera.main;

        _playerInput.Player.Click.started += Click_started;

        _playerInput.Player.Click.canceled += Click_canceled;
    }

    private void OnEnable() => _playerInput.Enable();
    private void OnDisable() => _playerInput.Disable();

    private void Update()
    {
        if(IsCutting)
        {
            UpdateCutPosition();
        }
    }

    private void Click_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        IsCutting = true;
        OnCuttingStarted?.Invoke(this, EventArgs.Empty);
        UpdateCutPosition();
    }

    private void Click_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        IsCutting = false;
        OnCuttingCanceled?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateCutPosition()
    {
        Vector2 screenPosition = _playerInput.Player.Position.ReadValue<Vector2>();

        Vector3 rawWorldPosition = _mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 10f));

        CurrentWorldPosition = new Vector3(rawWorldPosition.x, rawWorldPosition.y, 0f);
    }
}
