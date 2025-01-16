using Dapper;
using Employee_Management.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Employee_Management.Utils
{
    internal class SQLiteUtil
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        internal class TableStructure
        {
            public string? TableName { get;private set; }
            public List<ColumnInfo> Columns { get;private set; } = new List<ColumnInfo>();
            public List<ForeignKeyInfo> ForeignKeys { get;private set; } = new List<ForeignKeyInfo>();

            public TableStructure(string tableName)
            {
                GetTableStructure(tableName);
                this.TableName = tableName;
            }

            private void GetTableStructure(string tableName)
            {
                try
                {
                    using (SQLiteConnection con = new SQLiteConnection(_connectionString))
                    {
                        // Open the connection
                        con.Open();

                        // Get columns
                        using (SQLiteCommand cmd = new SQLiteCommand($"PRAGMA table_info({tableName});", con))
                        {
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    this.Columns.Add(new ColumnInfo
                                    {
                                        Name = reader["name"].ToString(),
                                        DataType = reader["type"].ToString(),
                                        IsNotNull = reader["notnull"].ToString() == "1",
                                        DefaultValue = reader["dflt_value"]?.ToString(),
                                        IsPrimaryKey = reader["pk"].ToString() == "1"
                                    });
                                }
                            }
                        }

                        // Get foreign keys
                        using (SQLiteCommand cmd = new SQLiteCommand($"PRAGMA foreign_key_list({tableName});", con))
                        {
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    this.ForeignKeys.Add(new ForeignKeyInfo
                                    {
                                        ColumnName = reader["from"].ToString(),
                                        ReferencedTable = reader["table"].ToString(),
                                        ReferencedColumn = reader["to"].ToString(),
                                        OnDelete = reader["on_delete"].ToString(),
                                        OnUpdate = reader["on_update"].ToString()
                                    });
                                }
                            }
                        }

                        // Get unique constraints (indexes)
                        using (SQLiteCommand cmd = new SQLiteCommand($"PRAGMA index_list({tableName});", con))
                        {
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (reader["unique"].ToString() == "1")
                                    {
                                        string indexName = reader["name"].ToString();

                                        // Get columns in this unique index
                                        using (SQLiteCommand cmdIndex = new SQLiteCommand($"PRAGMA index_info({indexName});", con))
                                        using (SQLiteDataReader readerIndex = cmdIndex.ExecuteReader())
                                        {
                                            while (readerIndex.Read())
                                            {
                                                var columnName = readerIndex["name"].ToString();
                                                var column = this.Columns.FirstOrDefault(c => c.Name == columnName);
                                                if (column != null)
                                                {
                                                    column.IsUnique = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (SQLiteException ex)
                {
                    SQLiteExceptionUtil.ExceptionHandler(ex);
                }
                catch (Exception ex)
                {
                    GeneralExceptionUtil.ExceptionHandler(ex);
                }
            }
        }

        internal class ColumnInfo
        {
            public string? Name { get; set; }
            public string? DataType { get; set; }
            public bool IsNotNull { get; set; }
            public string? DefaultValue { get; set; }
            public bool IsPrimaryKey { get; set; }
            public bool IsUnique { get; set; }
        }

        internal class ForeignKeyInfo
        {
            public string? ColumnName { get; set; }
            public string? ReferencedTable { get; set; }
            public string? ReferencedColumn { get; set; }
            public string? OnDelete { get; set; }
            public string? OnUpdate { get; set; }
        }

        internal static List<T> ReadData<T>(string queryString, object? parameters = null)
        {
            using (IDbConnection con = new SQLiteConnection(_connectionString))
            {
                try
                {
                    return con.Query<T>(queryString, parameters).ToList();
                }
                catch (SQLiteException ex)
                {
                    SQLiteExceptionUtil.ExceptionHandler(ex);
                }
                catch (Exception ex)
                {
                    GeneralExceptionUtil.ExceptionHandler(ex);
                }
                return new List<T>();
            }
        }
    
        internal static void UpdateEntry(TableStructure tableStructure, string colName,string updateValue ,string[] idArray)
        {
            string sqlQuery = $"UPDATE {tableStructure.TableName} SET {colName} = @Value WHERE ";

            /*
            if (idArray.Length == 1)
            {
                foreach (ColumnInfo col in tableStructure.Columns)
                {
                    if (col.IsPrimaryKey)
                    {
                        sqlQuery = sqlQuery + col.Name + " = @ID";
                        break;
                    }
                }

                using (IDbConnection con = new SQLiteConnection(_connectionString))
                {
                    con.Execute(sqlQuery, new {Value = updateValue, ID = idArray[0]});
                }
            }
            else if (idArray.Length >= 1)
            {
                int idIndex = 0;

                foreach (ColumnInfo col in tableStructure.Columns)
                {
                    if (col.IsPrimaryKey)
                    {
                        if (idIndex == 0)
                        {
                            sqlQuery = sqlQuery + col.Name + " = @ID";
                            idIndex++;
                        }
                        else if (idIndex == idArray.Length-1)
                        {
                            sqlQuery = sqlQuery + col.Name + " = @ID";
                        }
                        else
                        {
                            sqlQuery = sqlQuery + col.Name + " = @ID AND";
                            idIndex++;
                        }
                    }
                }
            }
            */

            int pkCount = 0;
            //var pkList = new Dictionary<string, int>();
            List<string> pkList = new List<string>();
            var parameters = new object();

            foreach (var col in tableStructure.Columns)
            {
                if (col.IsPrimaryKey)
                {
                    //pkList[colName] = pkCount;
                    pkList.Add(col.Name);
                    pkCount++;
                }
            }

            if (pkCount == idArray.Length)
            {
                // If there is only one primary key
                if (pkCount == 1)
                {
                    sqlQuery += pkList[0] + " = @ID";
                    parameters = new { Value = updateValue, ID = idArray[0] };
                }
                // If there are multiple primary keyes
                else if (pkCount >= 1)
                {
                    /*var pkConditions = new List<string>();
                    var pkParameters = new List<object>();

                    for (int i = 0; i < pkList.Count; i++)
                    {
                        var pkName = pkList.Keys.ElementAt(i);
                        pkConditions.Add($"{pkName} = @PK{i}");
                        pkParameters.Add(idArray[i]);
                    }

                    sqlQuery += string.Join(" AND ", pkConditions);
                    parameters = new { Value = updateValue, PK = pkParameters.ToArray() };*/
                }
                using (IDbConnection con = new SQLiteConnection(_connectionString))
                {
                    try
                    {
                        con.Execute(sqlQuery, parameters);
                    }
                    catch (SQLiteException ex)
                    {
                        SQLiteExceptionUtil.ExceptionHandler(ex);
                    }
                    catch (Exception ex)
                    {
                        GeneralExceptionUtil.ExceptionHandler(ex);
                    }
                }
            }
            else
            {
                throw new ArgumentException("The number of IDs provided does not match the number of primary keys.");
            }

        }
    
    }
}
