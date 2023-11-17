using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 1000;

    [SerializeField]
    private float speed = 15;

    private Transform target;

    void Start()
    {

    }

    private void Update()
    {
        if (!target)
        {
            try
            {
                target = FindObjectOfType<Enemy>().transform;
                
            }
            catch 
            {
                
            }
        }

        if (target)
        {

            Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.x);

            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, rotationSpeed * Time.deltaTime);

            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.x);

            transform.position += transform.right * speed * Time.deltaTime;
        }
    }
}