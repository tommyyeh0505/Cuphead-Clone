using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;

public class GameClear : MonoBehaviour
{
    [SerializeField] TMP_Text bestTimeText;
    [SerializeField] TMP_Text currentTimeText;
    [SerializeField] TMP_Text leaderboardText;

    private float bestTime;
    private bool queryDone = false;
    private Highscore highScore;
    private string topTenClearTimes;

    private void Start()
    {
        highScore = GetComponent<Highscore>();
    }

    private void Update()
    {
        if (queryDone)
        {
            leaderboardText.text = topTenClearTimes;
            bestTimeText.text = SecondsToString(bestTime);
            queryDone = false;
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetHighScore(float currentTime)
    {
        int userID = PlayerPrefs.GetInt("USERID");
        int levelID = PlayerPrefs.GetInt("CURRENTLEVELID");
        currentTimeText.text = SecondsToString(currentTime);
        Thread sqlThread = new Thread(() => DoHighScoreQuery(currentTime, userID, levelID));
        sqlThread.Start();
    }

    // Runs on own thread because SQL
    // note: async-await does not work for this implementation of SQL, so we have to use a thread
    public void DoHighScoreQuery(float currentTime, int userID, int levelID)
    {
        topTenClearTimes = highScore.GetTopTenClearTimes();
        string queryResult = highScore.GetPreviousClearTime(userID, levelID);
        string timeQuery = queryResult;

        if (timeQuery == null || timeQuery == "")
        {
            highScore.InsertNewClearTime(userID, levelID, currentTime);
        }
        else
        {
            float previousBestClearTime = float.Parse(timeQuery, CultureInfo.InvariantCulture.NumberFormat);
            if (previousBestClearTime < currentTime)
            {
                bestTime = previousBestClearTime;
            }
            else
            {
                bestTime = currentTime;
                highScore.UpdateClearTime(userID, levelID, bestTime);
            }
        }

        queryDone = true;
    }

    public string SecondsToString(float time)
    {
        TimeSpan t = TimeSpan.FromSeconds(time);
        return t.ToString("mm':'ss':'ff");
    }
}
