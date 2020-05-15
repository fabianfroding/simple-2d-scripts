using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkOrb : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    public Vector2 _centre;
    public float _angle;
    private float RotateSpeed = 1.5f;
    private float Radius = 1.5f;

    [SerializeField]
    AudioSource deathSound;

    public bool timedLife;


    void Start()
    {
        if (timedLife)
        {
            Invoke("DestroySelf", 8f);
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        _angle += RotateSpeed * Time.deltaTime;

        var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
        transform.position = _centre + offset;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            deathSound.Play();
        }
    }

    private void DestroySelf()
    {
        spriteRenderer.enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;

        Destroy(gameObject);
    }


}
