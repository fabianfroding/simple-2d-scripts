using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    bool isShaking = false;
    Vector2 startPos;

    [SerializeField]
    float shakeAmount = .05f;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShaking)
        {
            transform.position = startPos + UnityEngine.Random.insideUnitCircle * shakeAmount;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "AttackHitbox")
        {
            isShaking = true;
            Invoke("StopShaking", .3f);
        }
    }

    private void StopShaking()
    {
        isShaking = false;
        transform.position = startPos;
    }
}
