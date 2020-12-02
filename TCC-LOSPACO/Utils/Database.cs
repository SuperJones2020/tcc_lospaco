using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

public class Database {
    public MySqlConnection ReturnConnection() => new MySqlConnection(ConfigurationManager.ConnectionStrings["TCC_LOSPACO.Properties.Settings.Connection"].ConnectionString);
    private void OpenConnection(MySqlConnection connection) {
        if (connection.State == ConnectionState.Closed)
            connection.Open();
    }

    private void CloseConnection(MySqlConnection connection) {
        if (connection.State == ConnectionState.Open)
            connection.Close();
    }

    public void ExecuteProcedure(string proc, params object[] str) {
        var c = ReturnConnection();
        OpenConnection(c);
        MySqlCommand comm = ReturnProcedure(proc, str);
        comm.Connection = c;
        comm.ExecuteNonQuery();
        CloseConnection(c);
    }

    public MySqlCommand ReturnProcedure(string proc, params object[] str) => new MySqlCommand($"call {proc}({Global.FormatArray(str, 1)})");
    public MySqlCommand ReturnCommand(string str) => new MySqlCommand(str);

    public void ExecuteCommand(string str) {
        var c = ReturnConnection();
        OpenConnection(c);
        MySqlCommand comm = ReturnCommand(str);
        comm.Connection = c;
        comm.ExecuteNonQuery();
        CloseConnection(c);
    }


    public string[] ReaderColumns(string table) {
        List<string> columns = new List<string>();
        ReaderRows(ReturnCommand($"describe {table}"), row => {
            columns.Add((string)row[0]);
        });
        return columns.ToArray();
    }

    public string ReaderColumn(MySqlCommand command) {
        var c = ReturnConnection();
        OpenConnection(c);
        command.Connection = c;
        MySqlDataReader reader = command.ExecuteReader();
        string column = reader.GetName(0);
        reader.Close();
        CloseConnection(c);
        return column;
    }

    public void ReaderRows(MySqlCommand command, Action<object[]> function) {
        var c = ReturnConnection();
        OpenConnection(c);
        int currentRow = 0;
        List<List<object>> rows = new List<List<object>>();
        command.Connection = c;
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read()) {
            rows.Add(new List<object> { });
            for (int i = 0; i < reader.FieldCount; i++) rows[currentRow].Add(reader[i]);
            currentRow++;
        }
        reader.Close();
        CloseConnection(c);
        rows.Select(r => r.ToArray()).ToList().ForEach(row => function(row));
    }

    public object[] ReaderAllValue(MySqlCommand command) {
        var c = ReturnConnection();
        OpenConnection(c);
        command.Connection = c;
        MySqlDataReader reader = command.ExecuteReader();
        string column = reader.GetName(0);
        object value = null;
        while (reader.Read()) value = reader[0];
        reader.Close();
        CloseConnection(c);
        return new object[] { column, value };
    }

    public bool HasRows(MySqlCommand command) {
        bool hasRows = false;
        var c = ReturnConnection();
        OpenConnection(c);
        command.Connection = c;
        MySqlDataReader reader = command.ExecuteReader();
        hasRows = reader.HasRows;
        reader.Close();
        CloseConnection(c);
        return hasRows;
    }

    public object ReaderValue(MySqlCommand command) {
        var c = ReturnConnection();
        OpenConnection(c);
        command.Connection = c;
        object value = command.ExecuteScalar();
        CloseConnection(c);
        return value;
    }

    public object[] ReaderRow(MySqlCommand command) {
        var c = ReturnConnection();
        OpenConnection(c);
        List<object> row = new List<object>();
        command.Connection = c;
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read()) for (int i = 0; i < reader.FieldCount; i++) row.Add(reader[i]);
        reader.Close();
        CloseConnection(c);
        return row.ToArray();
    }
}