using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

public class Database {
    private static readonly MySqlConnection Connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["TCC_LOSPACO.Properties.Settings.Connection"].ConnectionString);

    static Database() {
        OpenConnection();
    }

    private static void OpenConnection() {
        if (Connection.State == ConnectionState.Closed)
            Connection.Open();
    }

    private static void CloseConnection() {
        if (Connection.State == ConnectionState.Open)
            Connection.Close();
    }

    public static void ExecuteProcedure(string proc, params object[] str) {
        OpenConnection();
        MySqlCommand command = new MySqlCommand($"call {proc}({Global.FormatArray(str, 1)})", Connection);
        command.ExecuteNonQuery();
        CloseConnection();
    }

    public static MySqlCommand ReturnProcedure(string proc, params object[] str) {
        MySqlCommand command = new MySqlCommand($"call {proc}({Global.FormatArray(str, 1)})", Connection);
        return command;
    }

    public static void ExecuteCommand(string str) {
        OpenConnection();
        MySqlCommand command = new MySqlCommand(str, Connection);
        command.ExecuteNonQuery();
        CloseConnection();
    }

    public static MySqlCommand ReturnCommand(string str) {
        MySqlCommand command = new MySqlCommand(str, Connection);
        return command;
    }

    public static string[] ReaderColumns(string table) {
        List<string> columns = new List<string>();
        ReaderRows(ReturnCommand($"describe {table}"), row => {
            columns.Add((string)row[0]);
        });
        return columns.ToArray();
    }

    public static string ReaderColumn(MySqlCommand command) {
        OpenConnection();
        MySqlDataReader reader = command.ExecuteReader();
        string column = reader.GetName(0);
        reader.Close();
        CloseConnection();
        return column;
    }

    public static void ReaderRows(MySqlCommand command, Action<object[]> function) {
        OpenConnection();
        int currentRow = 0;
        List<List<object>> rows = new List<List<object>>();
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read()) {
            rows.Add(new List<object> { });
            for (int i = 0; i < reader.FieldCount; i++) rows[currentRow].Add(reader[i]);
            currentRow++;
        }
        reader.Close();
        rows.Select(r => r.ToArray()).ToList().ForEach(row => function(row));
        CloseConnection();
    }

    public static object[] ReaderAllValue(MySqlCommand command) {
        OpenConnection();
        MySqlDataReader reader = command.ExecuteReader();
        string column = reader.GetName(0);
        object value = null;
        while (reader.Read()) value = reader[0];
        reader.Close();
        CloseConnection();
        return new object[] { column, value };
    }

    public static object ReaderValue(MySqlCommand command) {
        OpenConnection();
        object value = command.ExecuteScalar();
        CloseConnection();
        return value;
    }

    public static object[] ReaderRow(MySqlCommand command) {
        OpenConnection();
        List<object> row = new List<object>();
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read()) for (int i = 0; i < reader.FieldCount; i++) row.Add(reader[i]);
        reader.Close();
        CloseConnection();
        return row.ToArray();
    }
}