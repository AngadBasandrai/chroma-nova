using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{

    public float speed = 45f;
    bool grow;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Size());   
    }

    // Update is called once per frame
    void Update()
    {
        if (grow)
        {
            transform.localScale += Vector3.one * 2 * Time.deltaTime;
        }
        else
        {
            transform.localScale -= Vector3.one * 2 * Time.deltaTime;
        }
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
    }

    IEnumerator Size()
    {
        grow = true;
        yield return new WaitForSeconds(0.5f);
        grow = false;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Size());
    }
}
