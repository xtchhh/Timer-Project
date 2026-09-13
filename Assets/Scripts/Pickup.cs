using UnityEngine;

public class Pickup : MonoBehaviour
{
    private GameObject player;
    private float pickupTime = 2.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(player.transform);

        float distanceToPlayer = Vector3.Distance(this.transform.position, player.transform.position);

        if (distanceToPlayer < 5)
        {
            Manager.objectiveTime += pickupTime;
            Destroy(this.gameObject);
        }

    }
}
