using UnityEngine;

public class BaseObject : MonoBehaviour
{

    private const float ROTATION_SPEED = 100f;

    protected Vector3 _rotation = new(0f, 0f, ROTATION_SPEED);

    private void FixedUpdate()
    {
        transform.Rotate(_rotation * Time.fixedDeltaTime);
    }
}