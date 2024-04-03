using MySql.Data.MySqlClient;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MariaDBTest : MonoBehaviour
{
    public InputField idInputField;
    public InputField nameInputField;

    private void Start()
    {
        Debug.Log("Connection Test: " + ConnectionTest());
    }

    public bool ConnectionTest()
    {
        string server = "127.0.0.1";
        string database = "test_en";
        string uid = "root";
        string password = "4231";
        int port = 3307;

        string conStr = $"Server={server};Port={port};Database={database};Uid={uid};Pwd={password};";

        try
        {
            using (MySqlConnection conn = new MySqlConnection(conStr))
            {
                conn.Open();
                Debug.Log("Connected to MariaDB");
            }
            return true;
        }
        catch (Exception e)
        {
            Debug.Log("Exception: " + e.ToString());
            return false;
        }
    }

    public void SaveToDatabase()
    {
        string server = "127.0.0.1";
        string database = "test_en";
        string uid = "root";
        string password = "4231";
        int port = 3307;

        string conStr = $"Server={server};Port={port};Database={database};Uid={uid};Pwd={password};";

        try
        {
            using (MySqlConnection conn = new MySqlConnection(conStr))
            {
                conn.Open();
                Debug.Log("Connected to MariaDB");

                string id = idInputField.text;
                string name = nameInputField.text;

                string query = $"INSERT INTO your_table_name (id, name) VALUES ('{id}', '{name}')";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                    Debug.Log("Data inserted successfully");
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Exception: " + e.ToString());
        }
    }
}