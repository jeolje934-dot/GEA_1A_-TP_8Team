
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpPower = 5f;
    public float gravity = -20f;
    public float mouseSensitivity = 0.2f;

    public Transform cameraPivot;

    // UI가 열려 있으면 플레이어 조작을 중지
    public static bool inputLocked = false;

    private Vector2 lookInput;
    private float pitch = 20f;
    private Vector2 moveInput;
    private float verticalVelocity;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public void OnMove(InputValue value)
    {
        if (inputLocked) return;
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (inputLocked) return;

        if (value.isPressed && controller.isGrounded)
        {
            verticalVelocity = jumpPower;
        }
    }

    public void OnLook(InputValue value)
    {
        if (inputLocked)
        {
            lookInput = Vector2.zero;
            return;
        }

        lookInput = value.Get<Vector2>();
    }

    void Update()
    {
        if (inputLocked)
        {
            lookInput = Vector2.zero;
            moveInput = Vector2.zero;
            return;
        }

        // 좌우 회전
        transform.Rotate(
            0f,
            lookInput.x * mouseSensitivity,
            0f
        );

        // 중력 및 점프
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 move =
            transform.forward * moveInput.y +
            transform.right * moveInput.x;

        move *= moveSpeed;
        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);
    }
}
