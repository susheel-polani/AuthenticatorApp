using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Windows.Storage;
using System.IO;

namespace AuthenticatorApp
{
    internal class DataAccess
    {
        public async static void InitializeDatabase()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync("passwordless-KeyPairs.db", CreationCollisionOption.OpenIfExists);
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "passwordless-KeyPairs.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                String tableCommand = "CREATE TABLE IF NOT " + "EXISTS MyTable (Primary_Key INTEGER PRIMARY KEY, " + "Domain NVARCHAR(2048) NOT NULL," + "Username NVARCHAR(2048) NOT NULL," + "PrivateKey NVARCHAR(2048) NOT NULL UNIQUE)";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }
        }

        public static void AddData(string inputText1, string inputText2, string inputText3)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "passwordless-KeyPairs.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO MyTable VALUES (NULL, @Entry1, @Entry2, @Entry3);";
                insertCommand.Parameters.AddWithValue("@Entry1", inputText1);
                insertCommand.Parameters.AddWithValue("@Entry2", inputText2);
                insertCommand.Parameters.AddWithValue("@Entry3", inputText3);

                insertCommand.ExecuteReader();
            }

        }
        public static List<String> GetData(string inputText1, string inputText2)
        {
            List<String> entries = new List<string>();

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "passwordless-KeyPairs.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand("SELECT PrivateKey from MyTable where Domain = @Entry1 and Username = @Entry2", db);
                selectCommand.Parameters.AddWithValue("@Entry1", inputText1);
                selectCommand.Parameters.AddWithValue("@Entry2", inputText2);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    entries.Add(query.GetString(0));
                }
            }

            return entries;
        }

        public static void DropTable()
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "passwordless-KeyPairs.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                String tableCommand = "DROP TABLE IF EXISTS MyTable;";

                SqliteCommand dropTable = new SqliteCommand(tableCommand, db);

                dropTable.ExecuteReader();
            }
        }
    }
}
