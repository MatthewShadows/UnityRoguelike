using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Monkey : MonoBehaviour
{

    [Header("Stats")]
    public float speed = 4f;
    public float life = 10f;
    public float mouseSensitivity = 100f;
    public float MaxL = 10f;
    public float timeBtwShoot = 3f;
    public float damage = 2f;
    bool canShoot = true;
    float timer = 0;
    public int coins = 0;
    public int bulletFire = 0;
    public int bulletIce = 0;
    public float moveDistance = 1.0f;
    public int level = 0;


    [Header("UI")]
    public TextMeshProUGUI DamageText;
    public TextMeshProUGUI SpeedText;
    public TextMeshProUGUI CoinsText;
    public TextMeshProUGUI FireText;
    public TextMeshProUGUI IceText;
    public TextMeshProUGUI levelText;

    [Header("Referencias")]
    public Rigidbody rb;
    public GameObject bulletPrefab;
    public GameObject bulletPrefabIce;
    public GameObject bulletPrefabFire;
    public Transform firePoint;
    public Animator anim;
    public Image lifebar;

    [Header("Sonidos")]
    public AudioClip attack;
    public AudioClip dodge;
    [Header("Shop")]
    public bool playernear;
    public bool acept;
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panelv21;
    public GameObject panelv22;
    public GameObject panelv23;
    public GameObject panelMessage;
    int number;
    int coinsTrade;
    int powerTrade;
    public TextMeshProUGUI trade;
    public GameObject menu;
    public GameObject ShopCanvas;

    private float yRotation = 0f;
    Vector2 dir;
    InputAction inputAction;
    NewImputSystem input;
    Vector2 lookDirection = Vector2.zero;
    void Awake()
    {
        input = new NewImputSystem();
        input.Player.Movement.performed += ctx => dir = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => dir = Vector2.zero;
        input.Player.Shoot.performed += ctx => normalShoot();
        input.Player.ShootFire.performed += ctx => ShootFire();
        input.Player.ShootIce.performed += ctx => ShootIce();
        input.Player.Dash.performed += ctx => Dash();
        input.Player.Shop.performed += ctx => Shop();
        input.Player.Rotation.performed += ctx => lookDirection = ctx.ReadValue<Vector2>();
        input.Player.Rotation.canceled += ctx => lookDirection = Vector2.zero;
    }
    void OnEnable()
    {
        input.Enable();
        

    }
    void OnDisable()
    {
        input.Disable();
    }
    void Start()
    {
        lifebar.fillAmount = MaxL;
        DamageText.text = "" + damage;
        SpeedText.text = "" + speed;
        CoinsText.text = "" + coins;
        FireText.text = "" + bulletFire;
        IceText.text = "" + bulletIce;
        //Cursor.lockState = CursorLockMode.Locked;
        UpdateLevel();
    }
    void UpdateLevel()
    {
        levelText.text = "Nivel : " + ++level;
    }
    void Update()
    {
        lifebar.fillAmount = (life / MaxL);
        DamageText.text = "" + damage;
        SpeedText.text = "" + speed;
        CoinsText.text = "" + coins;
        FireText.text = "" + bulletFire;
        IceText.text = "" + bulletIce;
        RotateWithMouse();
        //normalShoot();
        CheckIfCanShoot();
        Checklife();
    }
    void Checklife()
    {
        if (life <= 0)
        {
            anim.SetTrigger("Dead");
            menu.SetActive(true);

        }
    }
    void CheckIfCanShoot()
    {
        if (!canShoot)
        {
            if (timer < timeBtwShoot)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
                canShoot = true;
            }
        }
    }
    void FixedUpdate()
    {
        Movement(); 
    }
    void Movement()
    {
        // Si no hay input del jugador, frena el movimiento
        if (dir == Vector2.zero)
        {
            rb.linearVelocity = Vector3.zero;
            anim.SetFloat("Speed", 0);
            return;
        }

        // Calcular el movimiento basado en el input actual
        Vector3 move = new Vector3(dir.x, 0, dir.y) * speed * Time.fixedDeltaTime;
        Vector3 newPosition = rb.position + move;

        // Mover el Rigidbody
        rb.MovePosition(newPosition);

        // Actualizar las animaciones
        anim.SetFloat("X", dir.x);
        anim.SetFloat("Speed", Mathf.Abs(dir.x) + Mathf.Abs(dir.y));

    }

    void RotateWithMouse()
    {
        float joystickX = lookDirection.x * mouseSensitivity * Time.deltaTime;

        yRotation += joystickX;
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
    void normalShoot()
    {
        if (canShoot)
        {
            AudioManager.instance.PlaySound(attack);
            anim.SetTrigger("Attack");
            Bullet b = Instantiate(bulletPrefab, firePoint.position, transform.rotation).GetComponent<Bullet>();
            b.damage = damage;
            canShoot = false;
        }
    }
    void ShootFire()
    {
        if (canShoot)
        {
            if (bulletFire > 0)
            {
                AudioManager.instance.PlaySound(attack);
                anim.SetTrigger("Attack");
                Bullet b = Instantiate(bulletPrefabFire, firePoint.position, transform.rotation).GetComponent<Bullet>();
                b.bulletType = Bullet.BulletType.Fire;
                canShoot = false;
                bulletFire -= 1;
            }
        }
        
    }
    void ShootIce()
    {
        if (canShoot)
        {
            if (bulletIce > 0)
            {
                AudioManager.instance.PlaySound(attack);
                anim.SetTrigger("Attack");
                Bullet b = Instantiate(bulletPrefabIce, firePoint.position, transform.rotation).GetComponent<Bullet>();
                b.bulletType = Bullet.BulletType.Ice;
                canShoot = false;
                bulletIce -= 1;
            }
        }
        
    }
    void Dash()
    {
        if (canShoot)
        {
            AudioManager.instance.PlaySound(dodge);
            anim.SetTrigger("Dodge");
            transform.position += Vector3.right * moveDistance;
            canShoot = false;
        }
        
    }
    //void Shoot()
    //{
    //    if (canShoot)
    //    {
    //        if (Input.GetKeyDown(KeyCode.Space))
    //        {
    //            AudioManager.instance.PlaySound(attack);
    //            anim.SetTrigger("Attack");
    //            Bullet b = Instantiate(bulletPrefab, firePoint.position, transform.rotation).GetComponent<Bullet>();
    //            b.damage = damage;
    //            canShoot = false;
    //        }
    //        else if (Input.GetKeyDown(KeyCode.R))
    //        {
    //            AudioManager.instance.PlaySound(dodge);
    //            anim.SetTrigger("Dodge");
    //            transform.position += Vector3.right * moveDistance;
    //            canShoot = false;

    //        }
    //        else if (Input.GetKeyDown(KeyCode.F) && bulletFire > 0)
    //        {
    //            AudioManager.instance.PlaySound(attack);
    //            anim.SetTrigger("Attack");
    //            Bullet b = Instantiate(bulletPrefabFire, firePoint.position, transform.rotation).GetComponent<Bullet>();
    //b.bulletType = Bullet.BulletType.Fire; 
    //            canShoot = false;
    //            bulletFire -= 1;
    //        }
    //        else if (Input.GetKeyDown(KeyCode.I) && bulletIce > 0)
    //        {
    //            AudioManager.instance.PlaySound(attack);
    //            anim.SetTrigger("Attack");
    //            Bullet b = Instantiate(bulletPrefabIce, firePoint.position, transform.rotation).GetComponent<Bullet>();
    //            b.bulletType = Bullet.BulletType.Ice; 
    //            canShoot = false;
    //            bulletIce -= 1;
    //        }
    //    }
    //}

    public void TakeDamage(float damage)
    {

        if (life > 0)
        {
            life -= damage;
            lifebar.fillAmount = life / MaxL;

        }
        else
        {
            anim.SetTrigger("Dead");
            menu.SetActive(true);

        }
    }
    public void TakeSpeed(float amount)
    {
        speed += amount;

    }
    public void TakeForce(float amount)
    {
        damage += amount;

    }
    public void TakeCoins(int amount)
    {
        coins += amount;

    }
    public void TakeFire(int amount)
    {
        bulletFire += amount;

    }
    public void TakeIce(int amount)
    {
        bulletIce += amount;

    }
    public void IncrementLife(float amount)
    {
        life += amount;
        if (life > MaxL)
        {
            life = MaxL;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        
    }
    void Shop()
    {
        coinsTrade = UnityEngine.Random.Range(1, 4);
        powerTrade = UnityEngine.Random.Range(1, 7);


        trade.text = "" + coinsTrade + "   =   " + powerTrade;
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        this.gameObject.transform.LookAt(pos);
        panel1.SetActive(false);
        number = UnityEngine.Random.Range(0, 4);
        if (number == 0)
        {
            panel2.SetActive(true);
            panelv21.SetActive(false);
            panelv22.SetActive(false);
            panelv23.SetActive(false);
            trade.gameObject.SetActive(true);

        }
        else if (number == 1)
        {
            panelv21.SetActive(true);
            panel2.SetActive(false);
            panelv22.SetActive(false);
            panelv23.SetActive(false);
            trade.gameObject.SetActive(true);
        }
        else if (number == 2)
        {
            panelv21.SetActive(false);
            panel2.SetActive(false);
            panelv22.SetActive(true);
            panelv23.SetActive(false);
            trade.gameObject.SetActive(true);
        }
        else if (number == 3)
        {
            panelv21.SetActive(false);
            panel2.SetActive(false);
            panelv22.SetActive(false);
            panelv23.SetActive(true);
            trade.gameObject.SetActive(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Shop")
        {
            ShopCanvas.SetActive(true);
            playernear = true;
            if (acept == false)
            {
                panel1.SetActive(true);
                /*panel1.SetActive(true);*/
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Shop")
        {
            ShopCanvas.SetActive(false);
            playernear = false;
            panel1.SetActive(false);
            panelv21.SetActive(false);
            panel2.SetActive(false);
            panelv22.SetActive(false);
            panelv23.SetActive(false);
            trade.gameObject.SetActive(false);
        }
    }
    public void Si()
    {
        panel1.SetActive(false);
        acept = true;
        if (coins >= coinsTrade)
        {
            if (panel2.activeSelf)
            {
                bulletFire += powerTrade;
                coins -= coinsTrade;
            }
            else if (panelv21.activeSelf)
            {
                bulletIce += powerTrade;
                coins -= coinsTrade;
            }
            else if (panelv22.activeSelf)
            {
                speed += powerTrade;
                coins -= coinsTrade;
            }
            else if (panelv23.activeSelf)
            {
                damage += powerTrade;
                coins -= coinsTrade;
            }

        }
        else
        {
            panelMessage.SetActive(true);
            StartCoroutine(HideMessageAfterDelay());
        }
        panel2.SetActive(false);
        panel1.SetActive(true);
        panelv21.SetActive(false);
        panelv22.SetActive(false);
        panelv23.SetActive(false);
        trade.gameObject.SetActive(false);
        acept = false;
    }
    public void No()
    {
        panel2.SetActive(false);
        panel1.SetActive(true);
        panelv21.SetActive(false);
        panelv22.SetActive(false);
        panelv23.SetActive(false);
        trade.gameObject.SetActive(false);
    }
    private IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(3);
        panelMessage.SetActive(false);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void End()
    {
        Application.Quit();
    }
}
