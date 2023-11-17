using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Robot : MonoBehaviour
{
    public Slider healthSlider;
    public Slider shieldSlider;
    public Text scoreText;
    public int score;
    public float damage = 5;
    int maxHealth = 250;
    public float health;
    public GameObject bullet;
    public GameObject Bullet;
    public GameObject Bullet2;
    public GameObject Bullet3;
    public GameObject Bullet4;
    public GameObject Bullet5;
    bool shoot = true;
    Rigidbody2D rb;
    public float shield = 0;
    public GameObject shield1Ind;
    public GameObject shield2Ind;
    int rockets = 0;
    int rocketsOnPrevFrame = 0;
    public float totDmgBns;
    public bool shocked;
    public GameObject gunShock;
    public GameObject header;

    // Start is called before the first frame update
    void Start()
    {
        healthSlider.maxValue = maxHealth;
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {        
        scoreText.text = "Score: " + score.ToString();
        if (health <= 0)
        {
            Destroy(gameObject);
            totDmgBns = 0;
            rockets = 0;
            score = 0;
            Enemy.bossBeat = 1;
            Enemy.boss_ = false;
            SceneManager.LoadScene(0);
        }
        healthSlider.value = health;
        shieldSlider.value = shield;
        if (!shocked)
        {
            gunShock.SetActive(false);
            rb.AddForce(Vector3.up * Time.deltaTime * 500 * Input.GetAxisRaw("Vertical") * ((UniversalTime.timeScale + 1) / 2));
            rb.gravityScale = UniversalTime.timeScale / 2;
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                GetComponent<Animator>().SetBool("Fly", true);
            }
            else
            {
                GetComponent<Animator>().SetBool("Fly", false);
            }
            if (Input.GetKeyDown(KeyCode.Space) && shoot)
            {
                if (rockets > 0)
                {
                    rockets -= 1;
                }
                StartCoroutine(FireAgain());
            }

            if (rocketsOnPrevFrame != 0 && rockets == 0)
            {
                ChangeBack();
            }

            rocketsOnPrevFrame = rockets;
        }
        else
        {
            rb.velocity = Vector3.zero;
            rb.gravityScale = 0;
            gunShock.SetActive(true);
        }
    }

    IEnumerator FireAgain()
    {
        GetComponent<Animator>().SetBool("Fire", true);
        shoot = false;
        yield return new WaitForSeconds(0.05f);
        if (bullet != Bullet5)
        {
            GameObject b = Instantiate(bullet, transform.position + Vector3.right, bullet.transform.rotation);
            b.GetComponent<Bullet>().dmg = damage;
        }
        else
        {
            for (int i = -2; i < 3; i++)
            {
                GameObject b = Instantiate(bullet, transform.position + Vector3.right + (Vector3.up * .05f * i), bullet.transform.rotation);
                b.GetComponent<Bullet>().minFallOff += (totDmgBns / 5);
                b.transform.Rotate(Vector3.forward * 5 * i);
                b.GetComponent<Bullet>().dmg = damage;
            }
        }
        yield return new WaitForSeconds(0.025f);
        GetComponent<Animator>().SetBool("Fire", false);
        yield return new WaitForSeconds(0.025f);
        shoot = true;
    }

    public void Shield1()
    {
        shield += 50;
        if (shield > 375)
        {
            shield = 375;
        }
        shield1Ind.SetActive(true);
    }


    public void Shield2()
    {
        shield += 100;
        if (shield > 375)
        {
            shield = 375;
        }
        shield2Ind.SetActive(true);
    }

    public IEnumerator ChangeAmmo()
    {
        bullet = Bullet2;
        damage = 20 + totDmgBns;
        yield return new WaitForSeconds(12.5f);
        bullet = Bullet;
        damage = 30 + totDmgBns;
    }

    public void ChangeAmmo2()
    {
        bullet = Bullet3;
        damage = 150 + totDmgBns;
        rockets = 6;
    }

    public void ChangeAmmo3()
    {
        bullet = Bullet4;
        damage = 10;
        rockets = 2;
    }

    public void ChangeAmmo4()
    {
        bullet = Bullet5;
        damage = 75 + (totDmgBns/5);
        rockets += 4;
    }

    void ChangeBack()
    {
        bullet = Bullet;
        damage = 30 + totDmgBns;
        rockets = 0;
    }

    public IEnumerator SlowMo()
    {
        UniversalTime.timeScale = 0.1f;
        yield return new WaitForSeconds(7f);
        UniversalTime.timeScale = 1f;
    }

    public IEnumerator Shocked(float t)
    {
        shocked = true;
        yield return new WaitForSeconds(t);
        shocked = false;
    }

    public void StopShield()
    {
        shield = 0;
        shield1Ind.SetActive(false);
        shield2Ind.SetActive(false);
    }

    public void ChangeHead(GameObject head)
    {
        header.GetComponent<SpriteRenderer>().sprite = head.GetComponent<SpriteRenderer>().sprite;
    }
}
