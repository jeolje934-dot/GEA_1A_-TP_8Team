using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitch : MonoBehaviour
{
    [Header("카메라")]
    public GameObject firstPersonCamera;
    public GameObject thirdPersonCamera;

    private bool isThirdPerson = false;

    void Start()
    {
        // 처음에는 1인칭
        firstPersonCamera.SetActive(true);
        thirdPersonCamera.SetActive(false);
    }

    void Update()
    {
        // V키를 눌렀는지 확인
        if (Keyboard.current != null && Keyboard.current.vKey.wasPressedThisFrame)
        {
            SwitchCamera();
        }
    }

    void SwitchCamera()
    {
        isThirdPerson = !isThirdPerson;

        if (isThirdPerson)
        {
            // 1인칭
            firstPersonCamera.SetActive(false);
            thirdPersonCamera.SetActive(true);

            Debug.Log("1인칭 카메라");
        }
        else
        {
            // 3인칭
            firstPersonCamera.SetActive(true);
            thirdPersonCamera.SetActive(false);

            Debug.Log("3인칭 카메라");
        }
    }
}