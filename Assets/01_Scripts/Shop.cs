using System.Collections;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject simbolomision;
    public Monkey monkey;
    public bool playernear;
    public bool acept;
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panelv21;
    public GameObject panelv22;
    public GameObject panelv23;
    public GameObject panelMessage;
    int number;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        monkey = GameObject.FindGameObjectWithTag("Player").GetComponent<Monkey>();
        simbolomision.SetActive(true);
        panel1.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && acept == false)
        {
            Vector3 pos = new Vector3(transform.position.x, monkey.gameObject.transform.position.y, transform.position.z);
            monkey.gameObject.transform.LookAt(pos);
            panel1.SetActive(false);
            number = Random.Range(0, 4);
            if (number == 0)
            {
                panel2.SetActive(true);
                panelv21.SetActive(false);
                panelv22.SetActive(false);
                panelv23.SetActive(false);

            }
            else if (number == 1)
            {
                panelv21.SetActive(true);
                panel2.SetActive(false);
                panelv22.SetActive(false);
                panelv23.SetActive(false);
            }
            else if (number == 2)
            {
                panelv21.SetActive(false);
                panel2.SetActive(false);
                panelv22.SetActive(true);
                panelv23.SetActive(false);
            }
            else if (number == 3)
            {
                panelv21.SetActive(false);
                panel2.SetActive(false);
                panelv22.SetActive(false);
                panelv23.SetActive(true);
            }


        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
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
        if (other.tag == "Player")
        {
            playernear = false;
            panel1.SetActive(false);
            panelv21.SetActive(false);
            panel2.SetActive(false);
            panelv22.SetActive(false);
            panelv23.SetActive(false);
        }
    }

    public void Si()
    {
        panel1.SetActive(false);
        acept = true;
        if (monkey.coins > 0)
        {
            if (panel2.activeSelf)
            {
                monkey.bulletFire += 1;
                monkey.coins -= 1;
            }
            else if (panelv21.activeSelf)
            {
                monkey.bulletIce += 1;
                monkey.coins -= 1;
            }
            else if (panelv22.activeSelf)
            {
                monkey.speed += 1;
                monkey.coins -= 1;
            }
            else if (panelv23.activeSelf)
            {
                monkey.damage += 1;
                monkey.coins -= 1;
            }

        }
        else
        {
            panelMessage.SetActive(true);
            StartCoroutine(HideMessageAfterDelay());
        }

        simbolomision.SetActive(false);
        panel2.SetActive(false);
        panel1.SetActive(true);
        panelv21.SetActive(false);
        panelv22.SetActive(false);
        panelv23.SetActive(false);
        acept = false;
    }
    public void No()
    {
        panel2.SetActive(false);
        panel1.SetActive(true);
        panelv21.SetActive(false);
        panelv22.SetActive(false);
        panelv23.SetActive(false);
    }
    private IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(3);
        panelMessage.SetActive(false);
    }
}
