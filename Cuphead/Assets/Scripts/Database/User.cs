using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{

    public int CreateNewUserID(string username)
    {
        string s = "";

        string connStr = "Server=placeholder;Port=3306;Database=CUPHEADCLONE;Uid=Tommy;Pwd=password";
        MySqlConnection conn = new MySqlConnection(connStr);
        try
        {
            Debug.Log("Connecting to MySQL...");
            conn.Open();

            MySqlCommand cmd = new MySqlCommand("INSERT INTO Users (username) VALUES(@username);\nSELECT LAST_INSERT_ID(); ", conn);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Prepare();
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                s += rdr[0];
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
        return int.Parse(s);

    }

}
