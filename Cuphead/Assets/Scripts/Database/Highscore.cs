using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highscore : MonoBehaviour
{
    //Connects to MySQL Database and Retrieves Score Data.
    //MYSQL user = Tommy/password
    public string GetPreviousClearTime(int userID, int levelID)
    {
        string s = "";

        string connStr = "Server=cuphead-clone.crjrjqa7nctn.us-east-2.rds.amazonaws.com;Port=3306;Database=CUPHEADCLONE;Uid=Tommy;Pwd=password";
        MySqlConnection conn = new MySqlConnection(connStr);
        try
        {
            Debug.Log("Connecting to MySQL...");
            conn.Open();

            MySqlCommand cmd = new MySqlCommand("SELECT clearTime FROM Score WHERE (USERID = @userID AND LEVELID = @levelID)", conn);
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@levelID", levelID);
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
        return s;

    }

    public void UpdateClearTime(int userID, int levelID, float currentClearTime)
    {
        string connStr = "Server=cuphead-clone.crjrjqa7nctn.us-east-2.rds.amazonaws.com;Port=3306;Database=CUPHEADCLONE;Uid=Tommy;Pwd=password";
        MySqlConnection conn = new MySqlConnection(connStr);
        try
        {
            Console.WriteLine("Connecting to MySQL...");
            conn.Open();

            MySqlCommand cmd = new MySqlCommand("UPDATE Score SET clearTime = @clearTime WHERE (userID = @userID AND levelID = @levelID)", conn);
            cmd.Parameters.AddWithValue("@clearTime", currentClearTime);
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@levelID", levelID);
            cmd.Prepare();


            MySqlDataReader rdr = cmd.ExecuteReader();
            rdr.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            Debug.Log("Closed MySQL Connection");
            conn.Close();
        }
    }

    public void InsertNewClearTime(int userID, int levelID, float currentClearTime)
    {
        string connStr = "Server=cuphead-clone.crjrjqa7nctn.us-east-2.rds.amazonaws.com;Port=3306;Database=CUPHEADCLONE;Uid=Tommy;Pwd=password";
        MySqlConnection conn = new MySqlConnection(connStr);
        try
        {
            Console.WriteLine("Connecting to MySQL...");
            conn.Open();

            string sql = "INSERT INTO Score VALUES(@userID, @levelID, @clearTime)";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@clearTime", currentClearTime);
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@levelID", levelID);
            cmd.Prepare();

            MySqlDataReader rdr = cmd.ExecuteReader();
            rdr.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            Debug.Log("Closed MySQL Connection");
            conn.Close();
        }
    }
}
