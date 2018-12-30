using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameLifecycleComponent : MonoBehaviour
{
    private float initializationTime;
    float levelTime;
    [SerializeField] GameObject player; //used to check if player has died

    //Game UI
    [SerializeField] TMP_Text time;

    //Gameover Overlay UI
    [SerializeField] GameObject gameOverUI;
    [SerializeField] TMP_Text gameOverTime;
    [SerializeField] TMP_Text gameOverScore;
    Boolean gameOver;

    // Start is called before the first frame update
    void Start()
    {
        initializationTime = Time.timeSinceLevelLoad;
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            if (player == null)
            {
                gameOver = true;
                gameOverTime.text = time.text;
                gameOverUI.SetActive(true);
                StartCoroutine(FetchDataCoroutine(levelTime));
            }
            else
            {
                levelTime = Time.timeSinceLevelLoad - initializationTime;
                TimeSpan t = TimeSpan.FromSeconds(levelTime);
                time.text = t.ToString("mm':'ss':'ff");
            }
        }
    }

    IEnumerator FetchDataCoroutine(float levelTime)
    {
        gameObject.GetComponent<GameOver>().SetHighScore(levelTime);
        yield return null;
    }
}
