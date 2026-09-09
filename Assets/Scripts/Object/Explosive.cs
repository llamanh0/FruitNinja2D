
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Explosive : BaseObject
{
    public event EventHandler OnBombExplode;

    [SerializeField] private VisualEffect _explosionVFX;
    [SerializeField] private GameObject _explosiveObjectSprite;

    private Rigidbody2D _rb;
    private Unity.Cinemachine.CinemachineImpulseSource _impulseSource;

    private float _explosionTime = 2f;
    private bool _isExploded = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _impulseSource = GetComponent<Unity.Cinemachine.CinemachineImpulseSource>();
    }

    public void Explode()
    { 
        if (_isExploded) return;
        _isExploded = true;
        _rotation = Vector3.zero;
        OnBombExplode?.Invoke(this, EventArgs.Empty);
        StartCoroutine(ExplosionCourotine());
    }

    private IEnumerator ExplosionCourotine()
    {
        _rb.linearVelocity = Vector2.zero;
        _rb.Sleep();

        if (_explosionVFX != null) _explosionVFX.Play();

        _impulseSource.GenerateImpulse();
        _impulseSource.enabled = false;

        _explosiveObjectSprite.SetActive(false);

        Time.timeScale = 0.1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(_explosionTime);

        float tempTime = 0f;
        float rescaleTime = 1f;
        while (tempTime < rescaleTime)
        {
            tempTime += Time.deltaTime;
            if ((Time.timeScale + (0.01f * tempTime)) > 1f) break;
            Time.timeScale += 0.01f * tempTime;
            yield return null;
        }

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        Destroy(gameObject,3f);
    }
}
