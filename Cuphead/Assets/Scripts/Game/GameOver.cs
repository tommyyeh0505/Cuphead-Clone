using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    [SerializeField] TMP_Text bestTimeText;
    [SerializeField] TMP_Text currentTimeText;

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
        float bestTime;
        int userID = PlayerPrefs.GetInt("USERID");
        int levelID = PlayerPrefs.GetInt("CURRENTLEVELID");
        string timeQuery = gameObject.GetComponent<Highscore>().GetPreviousClearTime(userID, levelID);
        if (timeQuery == null || timeQuery == "")
        {

            gameObject.GetComponent<Highscore>().InsertNewClearTime(userID, levelID, currentTime);
            currentTimeText.text = SecondsToString(currentTime);
            bestTimeText.text = SecondsToString(currentTime);
        }
        else
        {

            float previousBestClearTime = float.Parse(timeQuery, CultureInfo.InvariantCulture.NumberFormat);
            if (previousBestClearTime > currentTime)
            {
                bestTime = previousBestClearTime;
            }
            else
            {
                bestTime = currentTime;
                gameObject.GetComponent<Highscore>().UpdateClearTime(userID, levelID, bestTime);
            }

            currentTimeText.text = SecondsToString(currentTime);
            bestTimeText.text = SecondsToString(bestTime);

        }

    }

    public string SecondsToString(float time)
    {
        TimeSpan t = TimeSpan.FromSeconds(time);
        return t.ToString("mm':'ss':'ff");
    }
}
