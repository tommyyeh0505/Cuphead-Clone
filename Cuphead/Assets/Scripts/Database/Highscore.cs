using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highscore : MonoBehaviour
{
    private string ExecuteQuery(MySqlCommand cmd, string type)
    {
        string s = "";

        string connStr = "Server=placeholder;Port=3306;Database=CUPHEADCLONE;Uid=Tommy;Pwd=password";
        MySqlConnection conn = new MySqlConnection(connStr);
        try
        {
            Debug.Log("Connecting to MySQL...");
            conn.Open();
            cmd.Connection = conn;
            cmd.Prepare();

            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (type == "leaderboard")
                {
                    string userID = "" + rdr[0];
                    string levelID = "" + rdr[1];
                    string clearTime = "" + rdr[2];

                    float userIDFloat = float.Parse(userID);
                    float levelIDFloat = float.Parse(levelID);
                    float clearTimeFloat = float.Parse(clearTime);

                    userID = userIDFloat.ToString().PadLeft(3, '0');
                    levelID = levelIDFloat.ToString().PadLeft(3, '0');
                    clearTime = clearTimeFloat.ToString().PadLeft(7, '0');


                    //Debug.Log(rdr[0] + " -- " + rdr[1] + " -- " + rdr[2] + " -- " + rdr[3]);
                    s += userID + " ---------- " + levelID + " ---------- " + clearTime + "\n"; //Temporary    
                } else
                {
                    s += rdr[0];
                }
            }
            rdr.Close();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
        finally
        {
            Debug.Log("Closed MySQL Connection");
            conn.Close();
        }

        return s;
    }

    //Connects to MySQL Database and Retrieves Score Data.
    //MYSQL user = Tommy/password
    public string GetPreviousClearTime(int userID, int levelID)
    {
        MySqlCommand cmd = new MySqlCommand("SELECT clearTime FROM Score WHERE (USERID = @userID AND LEVELID = @levelID)");
        cmd.Parameters.AddWithValue("@userID", userID);
        cmd.Parameters.AddWithValue("@levelID", levelID);

        return ExecuteQuery(cmd, "single");
    }

    //Connects to MySQL Database and Retrieves Score Data.
    //MYSQL user = Tommy/password
    public string GetTopTenClearTimes()
    {
        MySqlCommand cmd = new MySqlCommand("SELECT * FROM Score ORDER BY clearTime LIMIT 10");
        return ExecuteQuery(cmd, "leaderboard");
    }

    public void UpdateClearTime(int userID, int levelID, float currentClearTime)
    {
        MySqlCommand cmd = new MySqlCommand("UPDATE Score SET clearTime = @clearTime WHERE (userID = @userID AND levelID = @levelID)");
        cmd.Parameters.AddWithValue("@clearTime", currentClearTime);
        cmd.Parameters.AddWithValue("@userID", userID);
        cmd.Parameters.AddWithValue("@levelID", levelID);

        ExecuteQuery(cmd, "none");
    }

    public void InsertNewClearTime(int userID, int levelID, float currentClearTime)
    {
        string sql = "INSERT INTO Score VALUES(@userID, @levelID, @clearTime)";
        MySqlCommand cmd = new MySqlCommand(sql);
        cmd.Parameters.AddWithValue("@clearTime", currentClearTime);
        cmd.Parameters.AddWithValue("@userID", userID);
        cmd.Parameters.AddWithValue("@levelID", levelID);

        ExecuteQuery(cmd, "none");
    }
}
