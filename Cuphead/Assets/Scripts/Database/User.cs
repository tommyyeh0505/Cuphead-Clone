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

        string connStr = "Server=cuphead-clone.crjrjqa7nctn.us-east-2.rds.amazonaws.com;Port=3306;Database=CUPHEADCLONE;Uid=Tommy;Pwd=password";
        MySqlConnection conn = new MySqlConnection(connStr);
        try
        {
            Debug.Log("Connecting to MySQL...");
            conn.Open();

            string sql = "INSERT INTO Users (username) VALUES('" + username + "');\n" + "SELECT LAST_INSERT_ID(); ";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
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
