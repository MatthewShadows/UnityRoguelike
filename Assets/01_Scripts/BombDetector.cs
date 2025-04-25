using UnityEngine;

public class BombDetector : MonoBehaviour
{
    public Bomb bomb;
    private void Start()
    {
        bomb = gameObject.GetComponentInParent<Bomb>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bomb.Explote(other.gameObject.GetComponent<Monkey>());
        }
    }
}
