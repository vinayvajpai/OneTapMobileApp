using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace OneTapMobile.LocalDataBases
{
   public abstract class DataBase
    {
        public SQLiteConnection Connection { get; }
        public static string Root { get; set; } = String.Empty;

         protected abstract void InitDatabaseTables();

        public DataBase()
        {
            var location = "onetapmobile.db";
            location = System.IO.Path.Combine(Root, location);
            Connection = new SQLiteConnection(location);
            InitDatabaseTables();
        }

        public int Update<T>(T item)
        {
            return Connection.Update(item);
        }
        public int Delete<T>(int Id)
        {
            return Connection.Delete<T>(Id);
        }
        public int BulkDelete<T>()
        {
            return Connection.DeleteAll<T>();
        }
        public int Save<T>(T item)
        {
            return Connection.Insert(item);
        }
        public int BulkSave<T>(List<T> item)
        {
            return Connection.InsertAll(item);
        }
    }
}
