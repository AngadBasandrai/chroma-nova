using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float dmg;
    public bool dmgFalloff;
    public float minFallOff;
    public float fallOffSpeed;
    public bool opp = false;
    public bool boom = false;
    public GameObject explosion;
    public float speedMul = 1;
    public bool homing = false;
    public GameObject dmgPop;
    public bool des = true;
    public bool shock = false;
    public float timeS;
    public bool armorBreak;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!homing)
        {
            GetComponent<Rigidbody2D>().velocity = Time.deltaTime * transform.right * 400 * (opp ? -1 : 1) * UniversalTime.timeScale * speedMul;
        }

        if (dmgFalloff)
        {
            if (dmg > minFallOff + (fallOffSpeed * Time.deltaTime))
            {
                dmg -= fallOffSpeed * Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        if (!boom)
        {
            if (transform.position.x > 10 || transform.position.x < -10)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (transform.position.x < -5.3f)
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player" && opp)
        {
            if (shock)
            {
                coll.gameObject.GetComponent<Robot>().StartCoroutine(coll.GetComponent<Robot>().Shocked(timeS));
            }
            if (armorBreak)
            {
                coll.gameObject.GetComponent<Robot>().StopShield();
            }
            
            GameObject tmp = Instantiate(dmgPop, coll.transform.position + (Vector3)(Random.insideUnitCircle * 1.5f), Quaternion.identity);
            DamagePopup dmgp = tmp.GetComponent<DamagePopup>();
            dmgp.Setup(dmg);

            if (coll.gameObject.GetComponent<Robot>().shield > 0)
            {
                if (coll.gameObject.GetComponent<Robot>().shield > dmg)
                {
                    coll.gameObject.GetComponent<Robot>().shield -= dmg;
                }
                else
                {
                    float leftover = dmg - coll.gameObject.GetComponent<Robot>().shield;
                    coll.gameObject.GetComponent<Robot>().shield = 0;
                    coll.gameObject.GetComponent<Robot>().StopShield();
                    coll.gameObject.GetComponent<Robot>().health -= leftover;
                }
            }
            else
            {
                coll.gameObject.GetComponent<Robot>().health -= dmg;
            }
            if (boom)
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
        if (coll.gameObject.tag == "Enemy" && !opp)
        {
            if (shock)
            {
                coll.gameObject.GetComponent<Enemy>().StartCoroutine(coll.GetComponent<Enemy>().Shocked(timeS));
            }

            GameObject tmp = Instantiate(dmgPop, coll.transform.position + (Vector3)Random.insideUnitCircle, Quaternion.identity);
            DamagePopup dmgp = tmp.GetComponent<DamagePopup>();
            dmgp.Setup(dmg);

            if (!coll.gameObject.GetComponent<Enemy>().boss)
            {
                coll.gameObject.GetComponent<Enemy>().health -= dmg;
            }
            else
            {
                coll.gameObject.GetComponent<Enemy>().bossHealth -= dmg;
            }
            Destroy(gameObject);
            if (boom)
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
            }
            
            coll.gameObject.GetComponent<Enemy>().Hit();
        }


        if (coll.gameObject.tag == "Bullet" && des)
        {
            if (opp && !coll.gameObject.GetComponent<Bullet>().opp)
            {
                Destroy(gameObject);
                Destroy(coll.gameObject);
            }
        }

    }
}
