using System.Collections;
using System.Collections.Generic;
using System.Data;
using System;
using System.Data.Common;
using Mono.Data.Sqlite;
using System.Drawing;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class DBManager : MonoBehaviour
{
    public static DBManager Instance { get; private set; }
    private string dbUri = "URI=file:mydb.sqlite";
    private string SQL_COUNT_ELEMNTS = "SELECT count(*) FROM Posiciones;";
    //Name; Timestamp; x; y; z
    private string SQL_CREATE_POSICIONES = "CREATE TABLE IF NOT EXISTS Posiciones(id INTEGER PRIMARY KEY, name TEXT not null, timestamp REAL not null, x REAL not null, y REAL not null, z REAL not null);";

    private IDbConnection dbConnection;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        OpenDatabase();
        InitializeDB();
    }

    private void OpenDatabase()
    {
        dbConnection = new SqliteConnection(dbUri);
        dbConnection.Open();
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "PRAGMA foreign_keys = ON";
        dbCommand.ExecuteNonQuery();
    }

    private void InitializeDB()
    {
        IDbCommand dbCmd = dbConnection.CreateCommand();
        dbCmd.CommandText = SQL_CREATE_POSICIONES;
        dbCmd.ExecuteReader();
    }

    public void SavePosition(CharacterPosition position)
    {  
        //Debug.Log($"INSERT INTO Posiciones(name, timestamp, x, y, z) VALUES('{position.name}','{position.timestamp}','{position.position.x}','{position.position.y}','{position.position.z}');");
        string command = $"INSERT INTO Posiciones(name, timestamp, x, y, z) VALUES('{position.name}','{position.timestamp}','{position.position.x}','{position.position.y}','{position.position.z}');";
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = command;
        dbCommand.ExecuteNonQuery();
    }

    private void OnDestroy()
    {
        dbConnection.Close();
    }
}
