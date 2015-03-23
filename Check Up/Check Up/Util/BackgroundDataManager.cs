using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace Check_Up.Util {
    class BackgroundDataManager {

        static string DatabaseName = "CheckUp.sqlite";
        static string BackgroundTableName = "BackgroundData";

        SQLiteConnection m_dbConnection;

        public BackgroundDataManager() {
            if (!File.Exists(DatabaseName)) {
                CreateDatabase();
            }
        }

        internal void ConnectDatabase() {
            m_dbConnection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", DatabaseName));
            m_dbConnection.Open();
        }

        internal void DisconnectDatabase() {
            m_dbConnection.Close();
        }

        public void InsertData(List<BackgroundData> dataValues) {
            ConnectDatabase();
            foreach (BackgroundData item in dataValues) {
                InsertData(item.CounterName, item.Cpu, item.Mem);
            }
            DisconnectDatabase();
            System.GC.Collect();
        }

        internal void InsertData(string key, float cpu, float mem) {
            string sql = string.Format("insert into {0} (name, cpu, mem) values ('{1}', '{2}', '{3}')", BackgroundTableName,
                                                                                                  key,
                                                                                                  cpu,
                                                                                                  mem);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);

            try {
                command.ExecuteNonQuery();
            }
            catch (Exception e) {
                Logger.Error(e.StackTrace);
                Logger.Error("[InsertData] Couldn't execute SQLite command");
            }
        }

        public void ExtractData() {

        }

        internal void CreateDatabase() {
            SQLiteConnection.CreateFile(DatabaseName);
            Logger.Info("Created database with name " + BackgroundTableName);

            ConnectDatabase();
            CreateTable(BackgroundTableName);
            DisconnectDatabase();
            System.GC.Collect();
            
        }

        internal void CreateTable(string table) {
            SQLiteCommand CreateTableCommand = new SQLiteCommand(string.Format("create table if not exists {0} (name text,  cpu float, mem float)", table), m_dbConnection);
            CreateTableCommand.ExecuteNonQuery();
            Logger.Info("Created Table with name " + table);
        }
        
    }
}
