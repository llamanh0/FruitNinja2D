using UnityEngine;

[RequireComponent (typeof(TrailRenderer))]
public class TrailController : MonoBehaviour
{
    [Header("Referances")]
    [SerializeField] private InputHandler _inputHandler;

    private TrailRenderer _trailRenderer;

    private void Awake()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
        _trailRenderer.emitting = false;
    }

    private void Update()
    {
        if (!_inputHandler) return;

        if (_inputHandler.IsCutting)
        {
            transform.position = _inputHandler.CurrentWorldPosition;

            if(!_trailRenderer.emitting)
            {
                _trailRenderer.Clear();
                _trailRenderer.emitting = true;
            }
        }
        else
        {
            _trailRenderer.emitting = false;
        }
    }
}
