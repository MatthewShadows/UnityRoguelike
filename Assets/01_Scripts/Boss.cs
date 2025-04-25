using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public float life = 1f;
    public float damage = 1f;
    public float speed = 10f;
    public float ShootDelay = 0.01f;
    public float delaySpeed = 1f;
    public float detectionRange = 150f;




    [Header("References")]
    public GameObject BoombPrefab;
    public GameObject BulletPrefab;
    public Transform FirePoint;
    public Slider BarraDeVida;
    private Rigidbody rb;
    private bool canShoot = false;
    private float delayCounter = 0f;
    private float delayBoomb = 0.2f;
    private GameObject playerRef;

    public AudioClip shootSound;
    public AudioClip BoombSound;
    public AudioClip Die;


    public Animator animatie;
    private float timer = 0f;
    public float DieDuration = 0.3f;
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        rb = this.GetComponent<Rigidbody>();
        canShoot = Random.Range(0, 2) == 1;
    }

    // Update is called once per frame
    void Update()
    {
        BarraDeVida.value = life;

        float distance = Vector3.Distance(this.transform.position, playerRef.transform.position);


        if (life <= 5)
        {
            animatie.SetBool("isAttack", false);
            if (distance <= detectionRange)
            {
                if (life > 0)
                {
                    Track();
                    BoombShoot();
                    animatie.SetBool("isThrowing", true);
                }



            }
            else
            {
                animatie.SetBool("isThrowing", false);
            }
        }
        else if (life > 5f)
        {
            if (distance <= detectionRange)
            {
                Track();
                StopAndShoot();
                animatie.SetBool("isAttack", true);


            }
            else
            {
                animatie.SetBool("isAttack", false);
            }
        }
    }



    void Track()
    {
        Vector3 direction = (playerRef.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 1f);


    }

    void StopAndShoot()
    {
        //rb.linearVelocity = Vector3.zero;

        if (delayCounter >= ShootDelay)
        {
            Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation);
            delayCounter = 0;
            AudioManager.instance.PlaySound(shootSound);
        }
        else
        {
            delayCounter += delaySpeed * Time.deltaTime;
        }
    }
    void BoombShoot()
    {
        //rb.linearVelocity = Vector3.zero;

        if (delayCounter >= delayBoomb)
        {
            Instantiate(BoombPrefab, FirePoint.position, FirePoint.rotation);
            delayCounter = 0;
            AudioManager.instance.PlaySound(BoombSound);
        }
        else
        {
            delayCounter += delaySpeed * Time.deltaTime;
        }
    }
    public void TakeDamage(float damage)
    {
        life -= damage;
        if (life <= 0)
        {
            animatie.SetBool("isThrowing", false);
            timer += Time.deltaTime;
            if (timer < DieDuration)
            {
                AudioManager.instance.PlaySound(Die);
                animatie.SetTrigger("isDead");
            }
            else
            {
                Destroy(gameObject);
            }



        }
    }
}
