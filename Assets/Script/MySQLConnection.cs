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
                cmd.Parameters.AddWithValue("@password", password); // 암호화된 비밀번호를 사용하는 것을 추천

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
    private static MySqlConnection connection // 호출 시 실행되는 구조 -> 호출 할 때마다 접속
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

    // 데이터베이스에서 데이터를 가져오는 함수
    private static DataSet m_OnLoad(string tableName, string query)
    {
        DataSet ds = null; ;
        try
        {
            connection.Open();   //DB 연결

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

        connection.Close();  //DB 연결 해제
        return ds;
    }

    /// <summary>
    /// 데이터 검색
    /// </summary>
    /// <param name="tableName">검색할 테이블</param>
    /// <param name="field">검색할 필드 (입력하지 않을 경우 전체 로드)</param>
    /// <param name="condition">조건</param>
    /// <returns></returns>
    public static XmlNodeList Select(string tableName, string condition = "") // 조건문만 적어서 모든 데이터를 호출
    {
        DataSet dataSet = m_OnLoad(tableName, $"SELECT * FROM {tableName} {condition}");

        if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
            return null;

        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(dataSet.GetXml());

        return xmlDocument.GetElementsByTagName(tableName);
    }


    public static XmlNodeList SelectOriginal(string tableName, string query) // 직접 쿼리문을 다 적어서 데이터를 호출
    {
        DataSet dataSet = m_OnLoad(tableName, query);

        if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
            return null;

        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(dataSet.GetXml());

        return xmlDocument.GetElementsByTagName(tableName);
    }

    /// <summary>
    /// 데이터 입력
    /// </summary>
    /// <param name="tableName">입력할 테이블</param>
    /// <param name="fieldName">입력할 필드 이름</param>
    /// <param name="value">입력할 값</param>
    /// <returns></returns>
    public static bool Insert(string tableName, string value)  // 직접 입력 함수
    {
        return m_OnChange($"INSERT INTO {tableName} VALUES ({value})");
    }
    //public static bool Insert(string tableName, string fieldName, string value)  // 값 하나만 변경할 때 쓰는 함수 -> 편하게 쓰려는 용도
    //{
    //    return m_OnChange($"INSERT INTO {tableName} ({fieldName}) VALUES ({value})");
    //}

    /// <summary>
    /// 레코드 갱신
    /// </summary>
    /// <param name="tableName">입력할 테이블</param>
    /// <param name="fieldName">입력할 필드 이름</param>
    /// <param name="value">입력할 값</param>
    /// <param name="condition">조건</param>
    /// <returns></returns>
    public static bool UpdateOriginal(string query) // 직접 쿼리문을 다 써서 사용하는 함수
    {
        Debug.Log("Update Data");
        return m_OnChange(query);
    }
    public static bool UpdateRanking(string tableName, string fieldName, float score, string condition) // 하나의 컬럼만 값을 변경할 때 사용
    {
        Debug.Log("Update Ranking Data");
        return m_OnChange($"UPDATE {tableName} SET {fieldName}={score} WHERE {condition}");
    }


    /// <summary>
    /// 레코드 제거
    /// </summary>
    /// <param name="tableName">제거할 레코드가 포함된 테이블</param>
    /// <param name="condition">조건</param>
    /// <returns></returns>
    public static bool Delete(string tableName, string condition)
    {
        return m_OnChange($"DELETE FROM {tableName} WHERE {condition}");
    }
}
