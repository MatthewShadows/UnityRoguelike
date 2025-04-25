using UnityEngine;
using System.Threading.Tasks;
using System.Collections;

public class CrazyEnemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public float life = 1f;
    public float damage = 1f;
    public float speed = 10f;
    public float ShootDelay = 2f;
    public float delaySpeed = 1f;
    public float detectionRange = 150f;
    private bool isFrozen = false;
    private bool isBurning = false;







    [Header("References")]

    private Rigidbody rb;
    private bool canShoot = false;
    //private float delayCounter = 0f;
    private GameObject playerRef;
    public Animator animatie;
    public GameObject[] powerUps;
    public GameObject freezeParticlesPrefab;
    private GameObject freezeEffectInstance;
    public GameObject fireParticlesPrefab;
    private GameObject fireEffectInstance;

    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        rb = this.GetComponent<Rigidbody>();
        canShoot = Random.Range(0, 2) == 1;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(this.transform.position, playerRef.transform.position);
        


        if (distance <= detectionRange )
        {
            Track();
            Move();
            animatie.SetBool("IsRunning", true);
            

        }
        else
        {
            
            rb.linearVelocity = Vector3.zero;
            animatie.SetBool("IsRunning", false);
        }

        


    }

    void Move()
    {
        rb.linearVelocity = transform.forward * speed;
    }

    void Track()
    {
        Vector3 direction = (playerRef.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);


    }

    //void StopAndShoot()
    //{
    //    //rb.linearVelocity = Vector3.zero;

    //    if (delayCounter >= ShootDelay)
    //    {
    //        Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation);
    //        delayCounter = 0;
    //    }
    //    else
    //    {
    //        delayCounter += delaySpeed * Time.deltaTime;
    //    }
    //}

    public void TakeDamage(float damage)
    {
        life -= damage;
        if (life <= 0)
        {
            Destroy(gameObject);
            
            GameObject powerup = powerUps[Random.Range(0, powerUps.Length)];
            Instantiate(powerup, transform.position, transform.rotation);
        }
    }
    public void Freeze()
    {
        if (!isFrozen)
        {
            isFrozen = true;
            Debug.Log("Enemy frozen");
            StartCoroutine(FreezeEffect());
        }
    }
    public void Burn()
    {
        if (!isBurning)
        {
            isBurning = true;
            Debug.Log("Enemy burning");
            StartCoroutine(BurnEffect());
        }
    }

    private IEnumerator FreezeEffect()
    {
        // Obtener el componente Rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Congelar el movimiento del enemigo (detener la velocidad y rotación)
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;  // Congela completamente el Rigidbody

            // Instanciar las partículas de congelamiento
            freezeEffectInstance = Instantiate(freezeParticlesPrefab, transform.position, Quaternion.identity);

            // Hacer que las partículas sigan al enemigo
            freezeEffectInstance.transform.SetParent(transform);
        }

        // Esperar 3 segundos mientras el enemigo está congelado
        yield return new WaitForSeconds(3);

        if (rb != null)
        {
            // Restaurar el movimiento del Rigidbody
            rb.isKinematic = false;  // Reactivar el movimiento normal del Rigidbody
        }

        // Destruir las partículas de congelamiento después de 3 segundos
        if (freezeEffectInstance != null)
        {
            Destroy(freezeEffectInstance);
        }

        isFrozen = false;  // El enemigo ya no está congelado
    }

    private IEnumerator BurnEffect()
    {
        // Instanciar las partículas de fuego
        fireEffectInstance = Instantiate(fireParticlesPrefab, transform.position, Quaternion.identity);

        // Hacer que las partículas sigan al enemigo
        fireEffectInstance.transform.SetParent(transform);

        // Aplica daño progresivo por fuego durante 5 segundos
        for (int i = 0; i < 5; i++)
        {
            TakeDamage(5); // Aplica 5 puntos de daño por segundo
            yield return new WaitForSeconds(1);
        }

        // Destruir las partículas de fuego después de 5 segundos
        if (fireEffectInstance != null)
        {
            Destroy(fireEffectInstance);
        }

        isBurning = false; // El enemigo ya no está quemándose
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Monkey>().TakeDamage(damage);

            animatie.SetTrigger("IsClose");

            //Destroy(gameObject);
        }
        
    }

}
