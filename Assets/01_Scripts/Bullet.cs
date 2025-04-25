using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 10;
    public float timeToDestroy = 4;
    public bool playerBullet = false;
    public float damage;
    public enum BulletType { Normal, Fire, Ice }
    public BulletType bulletType;
    public GameObject fireParticlesPrefab;
    public GameObject freezeParticlesPrefab;

    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Decoration"))
        {
            Destroy(gameObject);
        }
        if (playerBullet && other.gameObject.CompareTag("Enemy"))
        {
            var enemy = other.gameObject.GetComponent<Enemy>();
            var boss = other.gameObject.GetComponent<Boss>();
            if (enemy != null)
            {
                // Aplica daño inicial
                enemy.TakeDamage(damage);

                // Aplica el efecto de la bala (Fuego o Hielo)
                ApplyBulletEffects(enemy);
            }
            else if (boss != null)
            {
                boss.TakeDamage(damage);
            }
            Destroy(gameObject);
        }

        if (!playerBullet && other.gameObject.CompareTag("Player"))
        {
            Monkey mono = other.gameObject.GetComponent<Monkey>();
            mono.TakeDamage(damage);
        }
    }

    private void ApplyBulletEffects(Enemy enemy)
    {
        switch (bulletType)
        {
            case BulletType.Ice:
                enemy.Freeze();
                break;
            case BulletType.Fire:
                enemy.Burn();
                break;
        }
    }
}
