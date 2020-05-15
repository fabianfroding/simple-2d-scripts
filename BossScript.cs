using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    [SerializeField]
    int health = 15;

    SpriteRenderer spriteRenderer;
    private Material matWhite;
    private Material matDefault;
    private Object explosionRef;

    Object darkOrbRef;
    public float posX;
    public float posY;
    Object shadowCoilRef;

    GameObject[] orbs;

    [SerializeField]
    AudioSource teleport;

    [SerializeField]
    AudioSource summonOrbs;

    void Start()
    {
        posX = transform.position.x;
        posY = transform.position.y;
        darkOrbRef = Resources.Load("DarkOrb");
        shadowCoilRef = Resources.Load("ShadowCoil");
        orbs = new GameObject[4];
        spriteRenderer = GetComponent<SpriteRenderer>();
        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
        matDefault = spriteRenderer.material;
        explosionRef = Resources.Load("Explosion");


        Invoke("Teleport", 4f);

        Invoke("SummonOrbs", 2f);

        Invoke("ShadowCoil", 8f);
        
    }

    void ShadowCoil()
    {
        GameObject coil = (GameObject)Instantiate(shadowCoilRef);
        coil.transform.position = transform.position;
        coil.GetComponent<ShadowCoil>().movementSpeed = 1f;
        coil.GetComponent<ShadowCoil>().angleChangingSpeed = 50f;
        
        Invoke("ShadowCoil", 4f);
    }

    void Update()
    {
        posX = transform.position.x;
        posY = transform.position.y;
        for (int i = 0; i < 4; i++)
        {
            if (orbs[i] != null)
            {
                orbs[i].GetComponent<DarkOrb>()._centre.x = transform.position.x;
                orbs[i].GetComponent<DarkOrb>()._centre.y = transform.position.y;
            }
            
        }
    }

    void SummonOrbs()
    {
        if (gameObject != null && health > 0)
        {
            summonOrbs.Play();
            for (int i = 0; i < 4; i++)
            {
                orbs[i] = (GameObject)Instantiate(darkOrbRef);
                orbs[i].GetComponent<DarkOrb>().timedLife = true;
                orbs[i].GetComponent<DarkOrb>()._centre.x = transform.position.x;
                orbs[i].GetComponent<DarkOrb>()._centre.y = transform.position.y;
                orbs[i].GetComponent<DarkOrb>()._angle = 77f * i;
            }
            Invoke("SummonOrbs", 12f);
        }
        
        
    }

    void Teleport()
    {
        if (gameObject != null)
        {
            

            float xMin = -1.29f;
            float xMax = 1.26f;
            float yMin = -0.49f;
            float yMax = 0.83f;

            transform.position = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax));

            if (health > 0)
            {
                Invoke("Teleport", 6f);
            }
            teleport.Play();
        }
        
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Move this to bullet object instead, and look for enemy tag.
        if (gameObject != null)
        {
            if (collision.CompareTag("Bullet"))
            {
                collision.gameObject.GetComponent<BulletScript>().DestroySelf();
                health--;
                spriteRenderer.material = matWhite;
                if (health <= 0)
                {
                    KillSelf();
                }
                else
                {
                    Invoke("ResetMaterial", .1f);
                }
            }
        }
        
    }

    void ResetMaterial()
    {
        spriteRenderer.material = matDefault;
    }

    private void KillSelf()
    {
        if (gameObject != null)
        {
            GameObject explosion = (GameObject)Instantiate(explosionRef);
            explosion.transform.position = new Vector3(transform.position.x, transform.position.y + .3f, transform.position.z);

            for (int i = 0; i < 4; i++)
            {
                if (orbs[i] != null)
                {
                    orbs[i].SetActive(false);
                }
                
            }

            gameObject.SetActive(false);
        }
        
    }
}
