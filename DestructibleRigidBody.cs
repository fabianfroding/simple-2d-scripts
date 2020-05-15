using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleRigidBody : MonoBehaviour
{
    [SerializeField]
    Vector2 forceDirection;

    [SerializeField]
    float torque;

    Rigidbody2D rb2d;

    void Start()
    {
        float randTorque = UnityEngine.Random.Range(torque - 20, torque + 20);
        float randForceX = UnityEngine.Random.Range(forceDirection.x - 50, forceDirection.x + 50);
        float randForceY = UnityEngine.Random.Range(forceDirection.y, forceDirection.y + 50);

        forceDirection.x = randForceX;
        forceDirection.y = randForceY;

        rb2d = GetComponent<Rigidbody2D>();
        rb2d.AddForce(forceDirection);
        rb2d.AddTorque(randTorque);

        Invoke("DestroySelf", 3.5f);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

}
