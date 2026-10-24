using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public Vector3 offset = new Vector3(0, 5, -7);

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = new Vector3(
            target.position.x,
            0,
            target.position.z
        );

        transform.position = targetPosition + offset;
        transform.rotation = Quaternion.Euler(25, 0, 0);
    }
}