using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using UnityEngine;
using System;
using System.Data;
using System.Xml;


public class MySQLConnection : MonoBehaviour
{
    private string connectionString = "server=127.0.0.1;user=root;database=testdb;port=3306;password=1234;";

    public void RegisterUser(string username, string password)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string query = "INSERT INTO Users (username, password) VALUES (@username, @password)";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password); // ��ȣȭ�� ��й�ȣ�� ����ϴ� ���� ��õ

                cmd.ExecuteNonQuery();
                Debug.Log("User registered successfully");
            }
            catch (MySqlException ex)
            {
                Debug.LogError("Error: " + ex.Message);
            }
        }
    }
    private static MySqlConnection _connection = null;
    private static MySqlConnection connection // ȣ�� �� ����Ǵ� ���� -> ȣ�� �� ������ ����
    {
        get
        {
            if (_connection == null)
            {
                try
                {
                    string formatSql = string.Format("Server={0};Database={1};Uid={2};Pwd={3};Port={4};SslMode=none;",
                                                        "127.0.0.1", "testdb", "root", "1234", "3306");
                    _connection = new MySqlConnection(formatSql);
                }
                catch (MySqlException e)
                {
                    Debug.LogError(e);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }
            return _connection;
        }
    }

    private static bool m_OnChange(string query)
    {
        bool result = false;
        try
        {
            MySqlCommand sqlCommand = new MySqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandText = query;
            connection.Open();

            sqlCommand.ExecuteNonQuery();

            connection.Close();

            result = true;
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }

        connection.Close();
        return result;
    }

    // �����ͺ��̽����� �����͸� �������� �Լ�
    private static DataSet m_OnLoad(string tableName, string query)
    {
        DataSet ds = null; ;
        try
        {
            connection.Open();   //DB ����

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = query;

            MySqlDataAdapter sd = new MySqlDataAdapter(cmd);
            ds = new DataSet();
            sd.Fill(ds, tableName);
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }

        connection.Close();  //DB ���� ����
        return ds;
    }

    /// <summary>
    /// ������ �˻�
    /// </summary>
    /// <param name="tableName">�˻��� ���̺�</param>
    /// <param name="field">�˻��� �ʵ� (�Է����� ���� ��� ��ü �ε�)</param>
    /// <param name="condition">����</param>
    /// <returns></returns>
    public static XmlNodeList Select(string tableName, string condition = "") // ���ǹ��� ��� ��� �����͸� ȣ��
    {
        DataSet dataSet = m_OnLoad(tableName, $"SELECT * FROM {tableName} {condition}");

        if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
            return null;

        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(dataSet.GetXml());

        return xmlDocument.GetElementsByTagName(tableName);
    }


    public static XmlNodeList SelectOriginal(string tableName, string query) // ���� �������� �� ��� �����͸� ȣ��
    {
        DataSet dataSet = m_OnLoad(tableName, query);

        if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
            return null;

        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(dataSet.GetXml());

        return xmlDocument.GetElementsByTagName(tableName);
    }

    /// <summary>
    /// ������ �Է�
    /// </summary>
    /// <param name="tableName">�Է��� ���̺�</param>
    /// <param name="fieldName">�Է��� �ʵ� �̸�</param>
    /// <param name="value">�Է��� ��</param>
    /// <returns></returns>
    public static bool Insert(string tableName, string value)  // ���� �Է� �Լ�
    {
        return m_OnChange($"INSERT INTO {tableName} VALUES ({value})");
    }
    //public static bool Insert(string tableName, string fieldName, string value)  // �� �ϳ��� ������ �� ���� �Լ� -> ���ϰ� ������ �뵵
    //{
    //    return m_OnChange($"INSERT INTO {tableName} ({fieldName}) VALUES ({value})");
    //}

    /// <summary>
    /// ���ڵ� ����
    /// </summary>
    /// <param name="tableName">�Է��� ���̺�</param>
    /// <param name="fieldName">�Է��� �ʵ� �̸�</param>
    /// <param name="value">�Է��� ��</param>
    /// <param name="condition">����</param>
    /// <returns></returns>
    public static bool UpdateOriginal(string query) // ���� �������� �� �Ἥ ����ϴ� �Լ�
    {
        Debug.Log("Update Data");
        return m_OnChange(query);
    }
    public static bool UpdateRanking(string tableName, string fieldName, float score, string condition) // �ϳ��� �÷��� ���� ������ �� ���
    {
        Debug.Log("Update Ranking Data");
        return m_OnChange($"UPDATE {tableName} SET {fieldName}={score} WHERE {condition}");
    }


    /// <summary>
    /// ���ڵ� ����
    /// </summary>
    /// <param name="tableName">������ ���ڵ尡 ���Ե� ���̺�</param>
    /// <param name="condition">����</param>
    /// <returns></returns>
    public static bool Delete(string tableName, string condition)
    {
        return m_OnChange($"DELETE FROM {tableName} WHERE {condition}");
    }
}
