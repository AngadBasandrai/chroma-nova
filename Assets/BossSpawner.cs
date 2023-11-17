using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{

    public List<GameObject> bosses;
    public List<GameObject> permBosses;
    public List<int> scores;
    public List<int> permScores;
    int ls;
    public static BossSpawner instance;
    public GameObject bloomer;
    public GameObject bloodCloud;

    void Start()
    {
        bosses = permBosses;
        scores = permScores;
        if (instance)
        {
            if (instance != this)
            {
                Destroy(this);
            }
        }
        else
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (scores.Count != 0)
        {
            if (FindObjectOfType<Robot>().score >= scores[0])
            {
                ls = scores[0];
                scores.RemoveAt(0);
                Instantiate(bosses[0], new Vector3(7, 0, 0), bosses[0].transform.rotation);
                bosses.RemoveAt(0);
            }
        }
        else 
        {
            bosses = permBosses;
            scores = permScores;
            for (int i = 0; i < scores.Count; i++)
            {
                scores[i] += ls;
            }
        }
    }
}
