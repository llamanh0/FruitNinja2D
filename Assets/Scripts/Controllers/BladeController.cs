using System;
using UnityEngine;

public class BladeController : MonoBehaviour
{
    public static event EventHandler OnFruitSliced;
    public static event EventHandler OnBombSliced;

    private Collider2D _collider;

    private Vector3 _previousPosition;
    private Vector3 _currentPosition;
    private Vector3 _swipeDirection;

    private void Awake()
    {
        InputHandler.OnCuttingStarted += InputHandler_OnCuttingStarted;
        InputHandler.OnCuttingCanceled += InputHandler_OnCuttingCanceled;
        
        _collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        _previousPosition = _currentPosition;
        _currentPosition = transform.position;

        _swipeDirection = _currentPosition - _previousPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Slicable fruit = collision.GetComponent<Slicable>();
        Explosive bomb = collision.GetComponent<Explosive>();

        // Disable Collider
        collision.enabled = false;

        if (fruit != null)
        {
            OnFruitSliced?.Invoke(this, EventArgs.Empty);
            fruit.Cut(_swipeDirection);
        }

        if (bomb != null)
        {
            OnBombSliced?.Invoke(this, EventArgs.Empty);
            bomb.Explode();
        }
    }

    private void InputHandler_OnCuttingCanceled(object sender, System.EventArgs e)
    {
        _collider.enabled = false;
    }

    private void InputHandler_OnCuttingStarted(object sender, System.EventArgs e)
    {
        _collider.enabled = true;
    }

    private void OnDestroy()
    {
        InputHandler.OnCuttingCanceled -= InputHandler_OnCuttingCanceled;
        InputHandler.OnCuttingStarted -= InputHandler_OnCuttingStarted;
    }
}
