using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleScript : MonoBehaviour
{
    [SerializeField]
    int health = 3;

    [SerializeField]
    UnityEngine.Object destructibleRef;

    [SerializeField]
    UnityEngine.Object explosionRef;

    bool isShaking = false;

    float shakeAmount = .05f;

    Vector2 startPos;

    void Start()
    {
        
    }

    void Update()
    {
        if (isShaking)
        {
            transform.position = startPos + UnityEngine.Random.insideUnitCircle * shakeAmount;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            startPos = transform.position;
            health--;

            if (health <= 0)
            {
                ExplodeThisGameObject();
            }
            else
            {
                isShaking = true;
                Invoke("ResetShake", .2f);
            }
        }
    }

    void ResetShake()
    {
        isShaking = false;
        transform.position = startPos;
    }

    private void ExplodeThisGameObject()
    {
        GameObject destructible = (GameObject)Instantiate(destructibleRef);
        GameObject particles = (GameObject)Instantiate(explosionRef);

        destructible.transform.position = transform.position;
        particles.transform.position = transform.position;

        Destroy(gameObject);
    }
}
