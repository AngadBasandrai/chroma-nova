using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public TextMeshPro tmp;
    void Awake()
    {
        tmp = GetComponent<TextMeshPro>();
    }

    public void Setup(float dmgAmt)
    {
        tmp.SetText(((int)dmgAmt).ToString());
    }

    void Update()
    {
        tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, tmp.color.a - (Time.deltaTime * 0.5f));
        tmp.fontSize += 2 * Time.deltaTime;

        if (tmp.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
