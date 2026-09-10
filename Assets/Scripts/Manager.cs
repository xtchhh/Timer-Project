using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject player;
    private float objectiveTime = 15f;
    private float endDistance = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(this.gameObject.transform.position, player.transform.position);

        if (distanceToPlayer < endDistance)
        {
            Debug.Log($"Game Over because you reached objective, you won at {Time.time} seconds");
        }

        if (Time.time >= objectiveTime)
        {
            //Debug.Log("Game Over because you ran out of time");
        }
    }
}
