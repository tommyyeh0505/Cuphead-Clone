using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MysqlConnector : MonoBehaviour
{
    [SerializeField] private TMP_Text leaderboardtext;

    // Start is called before the first frame update
    void Start()
    {
        Connect();
    }

    // Update is called once per frame
    void Update()
    {
    }

    //Connects to MySQL Database and Retrieves Score Data.
    //MYSQL user = Tommy/password
    public void Connect()
    {
        string connStr = "Server=placeholder;Port=3306;Database=CUPHEADCLONE;Uid=Tommy;Pwd=password";
        MySqlConnection conn = new MySqlConnection(connStr);
        try
        {
            Console.WriteLine("Connecting to MySQL...");
            conn.Open();

            string sql = "SELECT * FROM Score ORDER BY clearTime LIMIT 10";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            string s = "";
            while (rdr.Read())
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
            }
            leaderboardtext.text = s;
            rdr.Close();
        }
        catch (Exception ex)
        {
           Debug.Log(ex.ToString());
        }
        finally
        {
            conn.Close();

        }
    }
}
