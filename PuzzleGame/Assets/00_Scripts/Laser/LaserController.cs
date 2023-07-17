using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class LaserController : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 100f;

    private Rigidbody2D laserRigidbody;

    private void Awake()
    {
        laserRigidbody = GetComponent<Rigidbody2D>();
    }

    public void RotateLaser(float rotation)
    {
        float newRotation = rotation * rotateSpeed * Time.fixedDeltaTime;
        laserRigidbody.MoveRotation(laserRigidbody.rotation + newRotation);
    }
}
