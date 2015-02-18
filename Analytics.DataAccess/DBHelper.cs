using System;
using Common.ExtensionMethods;
using MongoDB.Driver;


namespace DataAccess
{
   public class DBHelper
    {
        public MongoServer GetMongoConnection()
        {
            // connection string 
            const string connectionString = "mongodb://localhost";

            // reference to client object using connection string
            var client = new MongoClient(connectionString);

            if (connectionString.IsNull())
            {
                throw new Exception(@"'ConnectionString' connection string configuration is missing.");
            }
            //server
            var server = client.GetServer();
            try
            {
                server.Connect();
                return server;
            }
            catch (Exception ex)
            {
                throw new Exception(@"Server is not up and Running");
            }
        }

       public 
    }
}
