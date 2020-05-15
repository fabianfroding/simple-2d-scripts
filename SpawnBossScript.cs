using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBossScript : MonoBehaviour
{
    bool triggered = false;

    GameObject boss;

    Object bossRef;

    [SerializeField]
    AudioSource music;

    [SerializeField]
    AudioSource spawnSound;

    [SerializeField]
    AudioSource death;

    bool b = false;

    // Start is called before the first frame update
    void Start()
    {
        bossRef = Resources.Load("Boss");
    }

    // Update is called once per frame
    void Update()
    {
        if (!b && boss != null && !boss.activeSelf)
        {
            b = true;
            death.Play();
            music.Stop();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered && collision.name == "Player")
        {
            Debug.Log("Spawning boss...");
            spawnSound.Play();
            Invoke("StartBossMusic", spawnSound.clip.length);
            triggered = true;
        }
        
    }

    private void StartBossMusic()
    {
        boss = (GameObject)Instantiate(bossRef);
        boss.GetComponent<BossScript>().posX = -0.19f;
        boss.GetComponent<BossScript>().posY = 0.15f;
        music.Play();
    }
}
