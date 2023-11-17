using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public GameObject bullet;
    public int speed = 100;
    public bool shoot = true;
    public float damage = 5;
    public int fireRate = 5;
    public float health = 10;
    public bool boss = false;
    public int bossBullets = 0;
    public List<Vector3> bossShoot = new List<Vector3>();
    public List<Enemy> bossSmallGuys = new List<Enemy>();
    public List<Vector3> bossSmallGuysPos = new List<Vector3>();
    bool bossCanShoot = true;
    public int bossSpeed = 0;
    public int bossFireRate = 0;
    public float bossHealth = 0;
    public float bossMaxHealth = 0;
    public float bossDmg = 0;
    public int bossSpawnTime = 5;
    bool bossCanSpawn = true;
    public List<int> bossBonusAmt = new List<int>();
    public List<float> bossSpecAttTime = new List<float>();
    public List<GameObject> bossSpecAtt = new List<GameObject>();
    public GameObject lz;
    public GameObject ab;
    public bool shocked;
    public static bool boss_ = false;
    public static int bossBeat = 1;
    public int score = 5;

    // Start is called before the first frame update
    void Start()
    {
        if (boss)
        {
            boss_ = true;
        }
        health *= (bossBeat + 1) / 2.0f;
        damage *= (bossBeat + 1) / 2.0f;
        bossHealth *= (bossBeat + 1) / 2.0f;
        bossDmg *= (bossBeat + 1) / 2.0f;
        for (int i = 0; i < bossSpecAtt.Count; i++)
        {
            if (bossSpecAtt[i].name == "Lazer(Clone)")
            {
                StartCoroutine(Lazer(bossSpecAttTime[i]));
            }

            else if (bossSpecAtt[i].name == "ArmorBreak(Clone)")
            {
                StartCoroutine(ArmorBreak(bossSpecAttTime[i]));
            }
        }
        if (boss)
        {
            BossSpawner.instance.bloodCloud.SetActive(true);
            BossSpawner.instance.bloomer.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!boss)
        {
            if (health <= 0)
            {
                if (!boss_)
                {
                    GameObject.Find("Robot").GetComponent<Robot>().score += score;
                }
                for (int i = 0; i < bossBonusAmt.Count; i++)
                {
                    if (i == 0)
                    {
                        FindObjectOfType<Robot>().totDmgBns += bossBonusAmt[i];
                        FindObjectOfType<Robot>().damage += bossBonusAmt[i];
                    }
                    if (i == 1)
                    {
                        FindObjectOfType<Robot>().health += bossBonusAmt[i];
                        Mathf.Clamp(FindObjectOfType<Robot>().health, 0, 250);
                    }
                    if (i == 2)
                    {
                        FindObjectOfType<Robot>().shield += bossBonusAmt[i];
                        Mathf.Clamp(FindObjectOfType<Robot>().shield, 0, 375);
                    }
                }
                Destroy(gameObject);
            }
            if (!shocked)
            {
                GetComponent<Rigidbody2D>().velocity = Time.deltaTime * -Vector3.right * speed * UniversalTime.timeScale;
                if (shoot)
                {
                    StartCoroutine(Fire());
                }
                if (transform.position.x < -9)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                StopAllCoroutines();
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        if (boss)
        {
            if (bossHealth <= 0)
            {
                StopAllCoroutines();
                boss_ = false;
                bossBeat += 1;
                for (int i = 0; i < bossBonusAmt.Count; i++)
                {
                    if (i == 0)
                    {
                        FindObjectOfType<Robot>().totDmgBns += bossBonusAmt[i];
                        FindObjectOfType<Robot>().damage += bossBonusAmt[i];
                    }
                    if (i == 1)
                    {
                        FindObjectOfType<Robot>().health += bossBonusAmt[i];
                        Mathf.Clamp(FindObjectOfType<Robot>().health, 0, 250);
                    }
                    if (i == 2)
                    {
                        FindObjectOfType<Robot>().shield += bossBonusAmt[i];
                        Mathf.Clamp(FindObjectOfType<Robot>().shield, 0, 375);
                    }
                    BossSpawner.instance.bloodCloud.SetActive(false);
                    BossSpawner.instance.bloomer.SetActive(false);

                    if (Random.value <= 0.01f)
                    {
                        GameObject.Find("Robot").GetComponent<Robot>().ChangeHead(gameObject);
                    }
                    Destroy(gameObject);
                }
            }
            if (bossCanShoot)
            {
                StartCoroutine(BossFire());
            }
            if (bossCanSpawn)
            {
                StartCoroutine(BossSpawneing());
            }
        }
    }

    public void Hit()
    {
        if (boss)
        {
            Vector2 v = Vector2.MoveTowards(transform.position, transform.position + (Vector3.up * Random.Range(-3.5f, 3.5f)), Time.deltaTime * bossSpeed);
            v = new Vector2(v.x, Mathf.Clamp(v.y, -3, 4));
            transform.position = v;
        }
    }

    IEnumerator Fire()
    {
        shoot = false;
        GameObject b = Instantiate(bullet, transform.position + -Vector3.right, Quaternion.identity);
        b.GetComponent<Bullet>().dmg = damage;
        b.GetComponent<Bullet>().opp = true;
        yield return new WaitForSeconds((10 - fireRate) / UniversalTime.timeScale);
        shoot = true;
    }

    IEnumerator BossFire()
    {
        bossCanShoot = false;
        for (int i = 0; i < bossShoot.Count; i++)
        {
            GameObject b = Instantiate(bullet, transform.position + -Vector3.right + bossShoot[i], Quaternion.identity);
            b.GetComponent<Bullet>().dmg = bossDmg;
            b.GetComponent<Bullet>().opp = true;
        }
        yield return new WaitForSeconds((10 - bossFireRate) / UniversalTime.timeScale);
        bossCanShoot = true;
    }

    public IEnumerator Shocked(float t)
    {
        shocked = true;
        yield return new WaitForSeconds(t);
        shocked = false;
    }
    IEnumerator BossSpawneing()
    {
        bossCanSpawn = false;
        for (int i = 0; i < bossSmallGuys.Count; i++)
        {
            Instantiate(bossSmallGuys[i], bossSmallGuysPos[i] + transform.position, bossSmallGuys[i].transform.rotation);
        }
        yield return new WaitForSeconds(bossSpawnTime);
        bossCanSpawn = true;
    }

    IEnumerator Lazer(float s)
    {
        yield return new WaitForSeconds(s);
        Instantiate(lz, transform).gameObject.name = "Lazer(Clone)";
        GameObject.Find("Lazer(Clone)").GetComponent<SpriteRenderer>().enabled = true;
        GameObject.Find("Lazer(Clone)").GetComponent<Bullet>().enabled = true;
        StartCoroutine(Lazer(s));
    }

    IEnumerator ArmorBreak(float s)
    {
        yield return new WaitForSeconds(s);
        Instantiate(ab, transform).gameObject.name = "ArmorBreak(Clone)";
        GameObject.Find("ArmorBreak(Clone)").GetComponent<SpriteRenderer>().enabled = true;
        GameObject.Find("ArmorBreak(Clone)").GetComponent<Bullet>().enabled = true;
        StartCoroutine(ArmorBreak(s));
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (!boss)
        {
            if (coll.gameObject.tag == "Player")
            {
                if (coll.gameObject.GetComponent<Robot>().shield > 0)
                {
                    if (coll.gameObject.GetComponent<Robot>().shield >= damage * 3)
                    {
                        coll.gameObject.GetComponent<Robot>().shield -= damage * 3;
                    }
                    else
                    {
                        float leftover = (damage * 3) - coll.gameObject.GetComponent<Robot>().shield;
                        coll.gameObject.GetComponent<Robot>().shield = 0;
                        coll.gameObject.GetComponent<Robot>().StopShield();
                        coll.gameObject.GetComponent<Robot>().health -= leftover;
                    }
                }
                else
                {
                    coll.gameObject.GetComponent<Robot>().health -= damage * 3;
                }

                Destroy(gameObject);
            }
        }
        else
        {
            if (coll.gameObject.tag == "Enemy")
            {
                Destroy(coll.gameObject);
            }
        }
    }
}
