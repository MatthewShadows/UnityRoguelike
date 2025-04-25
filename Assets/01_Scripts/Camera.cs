using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform player;
    public float height = 10f;

    void LateUpdate()
    {
        Vector3 desiredPosition = new Vector3(player.position.x, player.position.y + height, player.position.z);
        transform.position = desiredPosition;
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
}
