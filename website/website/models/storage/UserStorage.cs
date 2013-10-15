namespace TeenWise.WebSite.Models.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlTypes;
    
    public abstract class UserStorage
    {
        public abstract void RegisterUser(string emailAddress);
        public abstract RegisteredUser[] GetUsers();
        public abstract RegisteredUser GetUser(string emailAddress);
    }

    public class SqlUserStorage : UserStorage
    {
        const string SPROC_REGISTERUSER = "RegisterUser";
        const string SPROC_GETUSER = "GetUser";
        const string SPROC_GETUSERS = "GetUsers";

        const string COLUMN_USERID = "UserID";
        const string COLUMN_EMAILADDRESS = "EmailAddress";
        const string COLUMN_REGISTEREDAT = "RegisteredAt";

        const string PARAM_EMAILADDRESS = "EmailAddress";

        string connection;
        const int retryCount = 3;

        public SqlUserStorage(string connection)
        {
            this.connection = connection;
        }

        public override void RegisterUser(string emailAddress)
        {
            SqlStorageCommand.Execute(this.connection, SPROC_REGISTERUSER,
                CommandType.StoredProcedure, (SqlStorageCommand command) =>
                {
                    command.AddParameter(PARAM_EMAILADDRESS, emailAddress);
                    command.ExecuteNonQuery();
                });
        }

        public override RegisteredUser GetUser(string emailAddress)
        {
            return SqlStorageCommand.Execute<RegisteredUser>(this.connection, SPROC_GETUSER,
                CommandType.StoredProcedure, (SqlStorageCommand command) =>
                {
                    command.AddParameter(PARAM_EMAILADDRESS, emailAddress);
                    command.ExecuteReader();
                    if (command.TryNextRow())
                    {
                        return GetNextUser(command);
                    }
                    return null;
                });
        }

        public override RegisteredUser[] GetUsers()
        {
            return SqlStorageCommand.Execute<RegisteredUser[]>(this.connection, SPROC_GETUSERS,
                CommandType.StoredProcedure, (SqlStorageCommand command) =>
                {
                    List<RegisteredUser> users = new List<RegisteredUser>();
                    command.ExecuteReader();
                    while (command.TryNextRow())
                    {
                        users.Add(GetNextUser(command));
                    }
                    return users.ToArray();
                });
        }

        RegisteredUser GetNextUser(SqlStorageCommand command)
        {
            long userid = command.GetColumnAsLong(COLUMN_USERID);
            string email = command.GetColumn(COLUMN_EMAILADDRESS);
            DateTime registeredAt = command.GetColumnAsDateTime(COLUMN_REGISTEREDAT);
            return new RegisteredUser(userid, email, registeredAt);
        }
    }

}