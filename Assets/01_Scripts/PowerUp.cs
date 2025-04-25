using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Stats")]
    public TypePowerUp TypePowerUp;
    public int minAmount;
    public int maxAmount;
    public int amount;
    void Start()
    {
        amount = Random.Range(minAmount, maxAmount+1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player");
            Monkey player = collision.gameObject.GetComponent<Monkey>();
            switch (TypePowerUp)
            {
                case TypePowerUp.Heal:
                    player.IncrementLife(amount);
                    break;
                case TypePowerUp.Force:
                    player.TakeForce(amount);
                    break;
                case TypePowerUp.Speed:
                    player.TakeSpeed(amount);
                    break;
                case TypePowerUp.Coin:
                    player.TakeCoins(amount);
                    break;
                case TypePowerUp.Fire:
                    player.TakeFire(amount);
                    break;
                case TypePowerUp.Ice:
                    player.TakeIce(amount);
                    break;
                default:
                    break;
            }
            Destroy(gameObject);
        }
    }
}
public enum TypePowerUp
{
    Heal,
    Force,
    Speed,
    Coin,
    Fire,
    Ice
}
