using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float speed = 3;
    [SerializeField] private AudioClip _clip;
    [SerializeField] private int PowerupID;
    //0 = triple shot
    //1 = speed 
    //2 = shield
    //3 = health
    //4 = ammo
    private int bottomOfScreen = -8;

    private void Update() 
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        if (transform.position.y < bottomOfScreen)
        {
            DestroyPowerUp();
        }

    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position);
            if (player != null)
            {
                switch(PowerupID)
                {
                    case 0:
                        player.TripleFireSetActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    case 3:
                        player.HealthAdd();
                        break;
                    case 4:
                        //ammo powerup
                        break;
                    default:
                        Debug.LogError("PowerupID is invalid");
                        break;
                }
                
            }
            DestroyPowerUp();
        }
    }

    private void DestroyPowerUp()
    {
        Destroy(this.gameObject);
    }
}
