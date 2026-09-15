using System;
using Unity.VisualScripting;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private GameObject player;
    private AudioSource pickpSound;
    private float destroyTimer;
    private float pickupTime = 2.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        pickpSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            this.transform.LookAt(player.transform);

            float distanceToPlayer = Vector3.Distance(this.transform.position, player.transform.position);

            destroyTimer += Time.deltaTime;

            if (distanceToPlayer < 5)
            {
                pickpSound.PlayDelayed(0.5f);
                Manager.gameTime += pickupTime;
                Destroy(this.gameObject);
            }

            if (this.gameObject != null & destroyTimer >= 30f)
            {
                Destroy(this.gameObject);
            }
        }
        catch (NullReferenceException ex)
        {
            Debug.Log($"Player no longer exists, this your error: {ex}");

        }
    }
}
