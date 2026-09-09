using System;
using UnityEngine;

public class Slicable : BaseObject
{
    public static event Action OnSlicableMissed;

    [Header("Object Referances")]
    [SerializeField] private GameObject _unslicedObject;
    [SerializeField] private GameObject _horizontalySliced;
    [SerializeField] private GameObject _verticalySliced;

    [Header("Physics Referances (Rigidbody2D)")]
    [SerializeField] private Rigidbody2D _topHalfRb;
    [SerializeField] private Rigidbody2D _bottomHalfRb;
    [SerializeField] private Rigidbody2D _leftHalfRb;
    [SerializeField] private Rigidbody2D _rightHalfRb;

    [Header("Settings")]
    [SerializeField] private float _seperationForce = 3f;

    private Rigidbody2D _mainRb;

    private float _missingThresholdPositionY = -.5f;
    private bool _isCutted = false;

    private void Awake()
    {
        _mainRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (transform.position.y < _missingThresholdPositionY && !_isCutted)
        { 
            OnSlicableMissed?.Invoke(); 
            Destroy(gameObject);
        }
    }

    public void Cut(Vector2 swipeDirection)
    {
        _unslicedObject.SetActive(false);
        _isCutted = true;
        Vector2 currentVelocity = _mainRb != null ? _mainRb.linearVelocity : Vector2.zero;

        if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
        {
            _horizontalySliced.SetActive(true);

            _topHalfRb.linearVelocity = currentVelocity;
            _bottomHalfRb.linearVelocity = currentVelocity;

            _topHalfRb.AddForce(Vector2.up * _seperationForce, ForceMode2D.Impulse);
            _bottomHalfRb.AddForce(Vector2.down * _seperationForce, ForceMode2D.Impulse);
        }
        else
        {
            _verticalySliced.SetActive(true);

            _leftHalfRb.linearVelocity = currentVelocity;
            _rightHalfRb.linearVelocity = currentVelocity;

            _leftHalfRb.AddForce(Vector2.left *_seperationForce, ForceMode2D.Impulse);
            _rightHalfRb.AddForce(Vector2.right * _seperationForce , ForceMode2D.Impulse);
        }

        GetComponent<Collider2D>().enabled = false;
    }


}