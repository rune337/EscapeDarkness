using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player;
    public float followSpeed = 5.0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
    }

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 nextPos = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        Vector3 nowPos = transform.position;

        transform.position = Vector3.Lerp(nowPos, nextPos, followSpeed * Time.deltaTime);
    }

}