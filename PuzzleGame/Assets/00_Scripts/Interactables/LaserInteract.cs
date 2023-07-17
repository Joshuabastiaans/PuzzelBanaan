using UnityEngine;
using UnityEngine.InputSystem;

public class LaserInteract : MonoBehaviour
{
    private LaserController laserController;
    [SerializeField] private float inputRotationSpeed = 100f;

    private PlayerInputSystem playerControls;
    private InputAction rotateLaser;

    private void Awake()
    {
        playerControls = new PlayerInputSystem();
    }

    private void OnEnable()
    {
        rotateLaser = playerControls.Player.RotateLaser;
        rotateLaser.Enable();
    }

    private void OnDisable()
    {
        rotateLaser.Disable();
    }

    private void FixedUpdate()
    {
        float inputRotation = GetInputRotation();

        if (laserController != null)
        {
            laserController.RotateLaser(inputRotation);
        }
    }

    private float GetInputRotation()
    {
        float rotation = 0f;
        float inputValue = rotateLaser.ReadValue<float>();

        if (inputValue > 0f)
        {
            rotation = inputRotationSpeed * Time.deltaTime;
        }
        else if (inputValue < 0f)
        {
            rotation = -inputRotationSpeed * Time.deltaTime;
        }

        return rotation;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser"))
        {
            laserController = collision.GetComponent<LaserController>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser"))
        {
            laserController = null;
        }
    }
}
