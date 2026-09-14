using System;
using System.Collections;
using Random = UnityEngine.Random;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public GameObject player;
    public GameObject objective;
    public GameObject pickup;
    public TMP_Text engineStartText;
    public TMP_Text objectiveText;
    public TMP_Text objectiveTimerText;
    public static float gameTime = 25f;
    private float endDistance = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnPickup", 3.0f, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        ObjectiveDistance();
        DestroyCheck();
        GameText();
    }

    void ObjectiveDistance()
    {
        try
        {
            //Debug.Log($"{gameTime} + {gameTime}");
            float distanceToPlayer = Vector3.Distance(objective.gameObject.transform.position, player.transform.position);
            
            if (TestMove.canRide == true)
            {
                gameTime -= Time.deltaTime;
            }

            if (distanceToPlayer < endDistance)
            {
                gameTime = 25f;
                TestMove.canRide = false;
                SceneManager.LoadScene("Win");
            }

            if (gameTime <= 0)
            {
                gameTime = 25f;
                TestMove.canRide = false;
                SceneManager.LoadScene("Loss");
            }
        }
        catch (NullReferenceException ex)
        {
            Debug.Log($"Player no longer exists, this your error: {ex}");
        }
    }

    void DestroyCheck()
    {
        if (!player.activeInHierarchy)
        {
            StartCoroutine(SwitchScene());
        }
    }

    IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(3.0f);

        gameTime = 25f;
        SceneManager.LoadScene("Menu");
    }
    void SpawnPickup()
    {
        float randomX = Random.Range(145, 5);
        float randomZ = Random.Range(980, 30);

        Instantiate(pickup, new Vector3(randomX, 1, randomZ), Quaternion.identity);
    }

    void GameText()
    {
        if (TestMove.canRide == true)
        {
            engineStartText.enabled = false;
            objectiveText.enabled = true;
            objectiveTimerText.enabled = true;

            objectiveTimerText.text = $"You have {Math.Round(gameTime),0} seconds";
            if (gameTime < 20f)
            {
                objectiveText.enabled = false;
            }
        }
    }
}
