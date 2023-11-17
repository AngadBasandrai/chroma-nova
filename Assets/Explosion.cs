using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject dmgPop;
    public float dmg;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyGO", 0.3f);
    }

    void DestroyGO()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
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
        }

        if (coll.gameObject.tag == "Enemy")
        {
            GameObject tmp = Instantiate(dmgPop, coll.transform.position + (Vector3)(Random.insideUnitCircle * 1.5f), Quaternion.identity);
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
        }
    }
}
