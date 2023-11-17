using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warper : MonoBehaviour
{

    public float speed = 10;
    public float exit = -10;
    Vector2 sp;

    // Start is called before the first frame update
    void Start()
    {
        sp = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= Vector3.right * speed * Time.deltaTime;
        if (transform.position.x < exit)
        {
            transform.position = sp;
        }
    }
}
