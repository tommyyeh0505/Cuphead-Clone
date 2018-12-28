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
        string connStr = "Server=cuphead-clone.crjrjqa7nctn.us-east-2.rds.amazonaws.com;Port=3306;Database=CUPHEADCLONE;Uid=Tommy;Pwd=password";
        MySqlConnection conn = new MySqlConnection(connStr);
        try
        {
            Console.WriteLine("Connecting to MySQL...");
            conn.Open();

            string sql = "SELECT * FROM Score";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            string s = "";
            while (rdr.Read())
            {
                //Debug.Log(rdr[0] + " -- " + rdr[1] + " -- " + rdr[2] + " -- " + rdr[3]);
                s += rdr[0] + " -- " + rdr[1] + " -- " + rdr[2] + " -- " + rdr[3] + "\n"; //Temporary
            }
            leaderboardtext.text = s;
            rdr.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            conn.Close();

        }
    }
}
