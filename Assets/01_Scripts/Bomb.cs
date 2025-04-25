using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Stats")]
    public int minDamage;
    public int maxDamage;
    public int damage;
    [Header("References")]
    public GameObject exploteParticle;
    [Header("Sounds")]
    public AudioClip explosionAudio;
    void Start()
    {
        damage = Random.Range(minDamage, maxDamage+1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Explote(Monkey player)
    {
        player.TakeDamage(damage);
        Destroy(gameObject);
        AudioManager.instance.PlaySound(explosionAudio);
        Instantiate(exploteParticle, transform.position, transform.rotation);
    }
}
