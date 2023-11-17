using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    public enum PowerUpType
    {
        Shield1,
        Shield2,
        AmmoType,
        AmmoType2,
        AmmoType3,
        AmmoType4,
        SlowMo
    }
    public PowerUpType type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = Time.deltaTime * -Vector3.right * 150 * UniversalTime.timeScale;
        if (transform.position.x <= -11)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            if (type == PowerUpType.Shield1)
            {
                coll.GetComponent<Robot>().Shield1();
            }
            else if (type == PowerUpType.Shield2)
            {
                coll.GetComponent<Robot>().Shield2();
            }
            else if (type == PowerUpType.AmmoType)
            {
                coll.GetComponent<Robot>().StartCoroutine("ChangeAmmo");
            }
            else if (type == PowerUpType.AmmoType2)
            {
                coll.GetComponent<Robot>().ChangeAmmo2();
            }
            else if (type == PowerUpType.AmmoType3)
            {
                coll.GetComponent<Robot>().ChangeAmmo3();
            }
            else if (type == PowerUpType.AmmoType4)
            {
                coll.GetComponent<Robot>().ChangeAmmo4();
            }
            else if (type == PowerUpType.SlowMo)
            {
                coll.GetComponent<Robot>().StartCoroutine("SlowMo");
            }

            Destroy(gameObject);
        }
    }
}
