using UnityEngine;

public class BombMovement : MonoBehaviour
{
    public float speed = 20;
    public float timeToDestroy = 4;
    public float damage;
    public float moveDuration = 0.1f;  // Duración del movimiento en segundos
    private float timer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer < moveDuration)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.zero);
        }
    }
}
