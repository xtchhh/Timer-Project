using UnityEngine;
using UnityEngine.LightTransport;

public class Manager : MonoBehaviour
{
    public GameObject player;
    public GameObject objective;
    public GameObject pickup;
    public static float objectiveTime = 15f;
    private float endDistance = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnPickup", 3.0f, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(player.transform.position);
        float distanceToPlayer = Vector3.Distance(objective.gameObject.transform.position, player.transform.position);

        if (distanceToPlayer < endDistance)
        {
            Debug.Log($"Game Over because you reached objective, you won at {Time.time} seconds");
        }

        if (Time.time >= objectiveTime)
        {
            Debug.Log("Game Over because you ran out of time");
        }
    }

    void SpawnPickup()
    {
        float randomX = Random.Range(145, 5);
        float randomZ = Random.Range(980, 15);

        Instantiate(pickup, new Vector3(randomX, 1, randomZ), Quaternion.identity);
    }
}
