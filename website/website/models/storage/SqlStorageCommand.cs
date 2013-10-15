// Copyright (c) Microsoft Corporation.  All rights reserved.

namespace TeenWise.WebSite.Models.Storage
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Threading;

    public class SqlStorageCommand : IDisposable
    {
        const int maxRetries = 5;
        static readonly TimeSpan waitTime = TimeSpan.FromSeconds(1);
        static readonly TimeSpan retryDelay = TimeSpan.FromMilliseconds(50);

        SqlCommand sqlCommand;
        SqlDataReader sqlDataReader;
        
        SqlStorageCommand(string connection, string command, CommandType commandType)
        {
            this.sqlCommand = new SqlCommand(command, new SqlConnection(connection));
            this.sqlCommand.CommandType = commandType;
        }

        public string Command
        {
            get { return this.sqlCommand.CommandText; }
        }

        public string Connection
        {
            get { return this.sqlCommand.Connection.ConnectionString; }
        }

        public int RetryCount
        {
            get { return maxRetries; }
        }

        public void AddParameter(string name, object value)
        {
            this.sqlCommand.Parameters.Add(new SqlParameter(name, value));
        }

        public void AddParameter(string name, DataTable value)
        {
            SqlParameter parameter = this.sqlCommand.Parameters.Add(name, SqlDbType.Structured);
            parameter.Value = value;
        }

        public void AddOutputParameter(string name, SqlDbType outputType)
        {
            this.sqlCommand.Parameters.Add(name, outputType).Direction = ParameterDirection.Output;
        }

        public void AddOutputParameter(string name, SqlDbType outputType, int size)
        {
            this.sqlCommand.Parameters.Add(name, outputType, size).Direction = ParameterDirection.Output;
        }

        public void Dispose()
        {
            this.sqlCommand.Connection.Dispose();
            this.sqlCommand.Dispose();
            if (this.sqlDataReader != null)
            {
                this.sqlDataReader.Dispose();
            }
        }

        public static void Execute(string connection, string command, CommandType commandType, Action<SqlStorageCommand> action)
        {
            Execute<int>(connection, command, commandType,
                (SqlStorageCommand dataCommand) => 
                {
                    action(dataCommand);
                    return 0;
                });
        }

        public static T Execute<T>(string connection, string command, CommandType commandType, Func<SqlStorageCommand, T> func)
        {
            DateTime startTime = DateTime.UtcNow;

            int tryCount = 0;
            while (true)
            {
                DateTime startTryTime = DateTime.UtcNow;
                tryCount++;

                using (SqlStorageCommand dataCommand = new SqlStorageCommand(connection, command, commandType))
                {
                    try
                    {
                        T result = func(dataCommand);

                        TimeSpan totalTime = DateTime.UtcNow - startTime;
                        if (totalTime > waitTime)
                        {   // TODO: log to diagnostics that command took longer than expected
                        }

                        return result;
                    }
                    catch (SqlException e)
                    {
                        TimeSpan totalTryTime = DateTime.UtcNow - startTryTime;

                        if (CanRetry(e))
                        {
                            // TODO: log error to diagnostics
                            if (tryCount > maxRetries)
                            {
                                // TODO: log that number of retries has been exceeded
                                throw new Exception("Number of retries has been exceeded.", e);
                            }
                            SqlConnection.ClearPool(dataCommand.sqlCommand.Connection);
                        }
                        else
                        {
                            // TODO: log error to diagnostics
                            throw e;
                        }
                    }
                    catch (Exception e)
                    {
                        // TODO: log error to diagnostics
                        throw e;
                    }
                }

                Thread.Sleep(retryDelay);
            }
        }

        public int ExecuteNonQuery()
        {
            this.sqlCommand.Connection.Open();
            return this.sqlCommand.ExecuteNonQuery();
        }

        public void ExecuteReader()
        {
            this.sqlCommand.Connection.Open();
            this.sqlDataReader = this.sqlCommand.ExecuteReader();
        }

        public object ExecuteScalar()
        {
            this.sqlCommand.Connection.Open();
            return this.sqlCommand.ExecuteScalar();
        }

        public string[] GetColumnNames()
        {
            string[] names = new string[this.sqlDataReader.FieldCount];
            for (int i = 0; i < this.sqlDataReader.FieldCount; i++)
            {
                names[i] = this.sqlDataReader.GetName(i);
            }
            return names;
        }

        public object[] GetColumnValues()
        {
            object[] values = new object[this.sqlDataReader.FieldCount];
            this.sqlDataReader.GetValues(values);
            return values;
        }

        public string GetColumn(string name)
        {
            try
            {
                int ordinal = GetColumnOrdinal(name);
                if (this.sqlDataReader.IsDBNull(ordinal))
                {
                    return null;
                }
                return this.sqlDataReader.GetString(ordinal);
            }
            catch (Exception e)
            {
                throw ColumnException(name, e);
            }
        }

        public bool GetColumnAsBool(string name)
        {
            try
            {
                return this.sqlDataReader.GetBoolean(GetColumnOrdinal(name));
            }
            catch (Exception e)
            {
                throw ColumnException(name, e);
            }
        }

        public DateTime GetColumnAsDateTime(string name)
        {
            try
            {
                return this.sqlDataReader.GetDateTime(GetColumnOrdinal(name));
            }
            catch (Exception e)
            {
                throw ColumnException(name, e);
            }
        }

        public Guid GetColumnAsGuid(string name)
        {
            try
            {
                return this.sqlDataReader.GetGuid(GetColumnOrdinal(name));
            }
            catch (Exception e)
            {
                throw ColumnException(name, e);
            }
        }

        public int GetColumnAsInt(string name)
        {
            try
            {
                return this.sqlDataReader.GetInt32(GetColumnOrdinal(name));
            }
            catch (Exception e)
            {
                throw ColumnException(name, e);
            }
        }

        public long GetColumnAsLong(string name)
        {
            try
            {
                return this.sqlDataReader.GetInt64(GetColumnOrdinal(name));
            }
            catch (Exception e)
            {
                throw ColumnException(name, e);
            }
        }

        public DateTime? GetColumnAsNullableDateTime(string name)
        {
            try
            {
                int ordinal = GetColumnOrdinal(name);
                if (this.sqlDataReader.IsDBNull(ordinal))
                {
                    return null;
                }
                else
                {
                    return this.sqlDataReader.GetDateTime(ordinal);
                }
            }
            catch (Exception e)
            {
                throw ColumnException(name, e);
            }
        }

        public int? GetColumnAsNullableInt(string name)
        {
            try
            {
                int ordinal = GetColumnOrdinal(name);
                if (this.sqlDataReader.IsDBNull(ordinal))
                {
                    return null;
                }
                else
                {
                    return this.sqlDataReader.GetInt32(ordinal);
                }
            }
            catch (Exception e)
            {
                throw ColumnException(name, e);
            }
        }

        public long? GetColumnAsNullableLong(string name)
        {
            try
            {
                int ordinal = GetColumnOrdinal(name);
                if (this.sqlDataReader.IsDBNull(ordinal))
                {
                    return null;
                }
                else
                {
                    return this.sqlDataReader.GetInt64(ordinal);
                }
            }
            catch (Exception e)
            {
                throw ColumnException(name, e);
            }
        }

        public object GetColumnAsObject(string name)
        {
            try
            {
                return this.sqlDataReader[name];
            }
            catch (Exception e)
            {
                throw ColumnException(name, e);
            }
        }

        public SqlParameterCollection GetParameters()
        {
            return this.sqlCommand.Parameters;
        }

        public string GetParameter(string name)
        {
            return (string)this.sqlCommand.Parameters[name].Value;
        }

        public int GetParameterAsInt(string name)
        {
            return Convert.ToInt32(this.sqlCommand.Parameters[name].Value, CultureInfo.InvariantCulture);
        }

        public long GetParameterAsLong(string name)
        {
            return Convert.ToInt64(this.sqlCommand.Parameters[name].Value, CultureInfo.InvariantCulture);
        }

        public object GetParameterAsObject(string name)
        {
            return this.sqlCommand.Parameters[name].Value;
        }

        public bool HasColumn(string name)
        {
            for (int i = 0; i < this.sqlDataReader.FieldCount; i++)
            {
                if (string.Compare(this.sqlDataReader.GetName(i), name, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public void NextResult()
        {
            if (!TryNextResult())
            {
                throw new Exception("Additional results were expected.");
            }
        }

        public void NextRow()
        {
            if (!TryNextRow())
            {
                throw new Exception("Additonal rows were expected in the data.");
            }
        }

        public bool TryNextResult()
        {
            return this.sqlDataReader.NextResult();
        }

        public bool TryNextRow()
        {
            return this.sqlDataReader.Read();
        }

        Exception ColumnException(string name, Exception innerException)
        {
            return new Exception( 
                string.Format("Error accessing column '{0}' of result.  {1}", name, innerException.Message), 
                innerException);
        }

        static bool CanRetry(SqlException sqlException)
        {
            return CanRetry(sqlException.Number);
        }

        static bool CanRetry(int error)
        {
            for (int i = 0; i < retryErrors.Length; i++)
            {
                if (retryErrors[i] == error)
                {
                    return true;
                }
            }
            return false;
        }

        int GetColumnOrdinal(string name)
        {
            try
            {
                return this.sqlDataReader.GetOrdinal(name);
            }
            catch (IndexOutOfRangeException e)
            {
                throw new Exception(string.Format("The column '{0}' is not defined.", name), e);
            }
        }

        static readonly int[] retryErrors = new int[] 
        { 
            -2,             // timeout                        
            20,             // encryption error
            64,             // login error
            109,            // pipe closed
            232,            // pipe being closed
            233,            // connection initialization error
            1205,           // deadlock (serializable isolation)
            1608,           // network error 
            3960,           // deadlock (snapshot isolation)
            10008,          // bad token: datastream processing out of sync
            10010,          // read failed
            10018,          // closing network connection error
            10025,          // write failed
            10053,          // aborted connection
            10054,          // connection reset
            10058,          // socket shutdown
            10060,          // network related error
            17824,          // write failure to server-side connection
            17825,          // closing server-side connection error
            17832,          // read failure of login packets
            40143,          // error processing request
            40197,          // error processing request
            40501,          // service busy
            40549,          // long running transaction
            40550,          // too many locks
            40551,          // excessive tempdb
            40552,          // excessive log space
            40553,          // excessive memory
            40613,          // database not available
        };

    }
}
