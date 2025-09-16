using Dapper;
using Microsoft.Data.SqlClient; //install NuGet package Microsoft.Data.SqlClient by Microsoft
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;


/*using System.Data.SqlClient;
"Message": "An error has occurred.",
  "ExceptionMessage": "Keyword not supported: 'trust server certificate'.",
  "ExceptionType": "System.ArgumentException",
  "StackTrace": "   at System.Data.Common.DbConnectionOptions.ParseInternal(Hashtable parsetable, String connectionString, Boolean buildChain, Hashtable synonyms, Boolean firstKey)\r\n   at System.Data.Common.DbConnectionOptions..ctor(String connectionString, Hashtable synonyms, Boolean useOdbcRules)\r\n   at System.Data.SqlClient.SqlConnectionString..ctor(String connectionString)\r\n   at System.Data.SqlClient.SqlConnectionFactory.CreateConnectionOptions(String connectionString, DbConnectionOptions previous)\r\n   at System.Data.ProviderBase.DbConnectionFactory.GetConnectionPoolGroup(DbConnectionPoolKey key, DbConnectionPoolGroupOptions poolOptions, DbConnectionOptions& userConnectionOptions)\r\n   at System.Data.SqlClient.SqlConnection.ConnectionString_Set(DbConnectionPoolKey key)\r\n   at System.Data.SqlClient.SqlConnection.set_ConnectionString(String value)\r\n   at System.Data.SqlClient.SqlConnection..ctor(String connectionString, SqlCredential credential)\r\n   at Magazin.Library.Internal.DataAccess.SqlDataAccess.LoadData[T,U](String storedProcedure, U parameters, String connectionStringName) in C:\\app_CSharp\\Magazin\\Magazin.Library\\Internal\\DataAccess\\SqlDataAccess.cs:line 27\r\n   at Magazin.Library.DataAccess.UserData.GetUsersById(String Id) in C:\\app_CSharp\\Magazin\\Magazin.Library\\DataAccess\\UserData.cs:line 19\r\n   at Magazin.Controllers.UserController.GetById() in C:\\app_CSharp\\Magazin\\Magazin\\Controllers\\UserController.cs:line 23\r\n   at lambda_method(Closure , Object , Object[] )\r\n   at System.Web.Http.Controllers.ReflectedHttpActionDescriptor.ActionExecutor.<>c__DisplayClass6_0.<GetExecutor>b__2(Object instance, Object[] methodParameters)\r\n   at System.Web.Http.Controllers.ReflectedHttpActionDescriptor.ExecuteAsync(HttpControllerContext controllerContext, IDictionary`2 arguments, CancellationToken cancellationToken)\r\n--- End of stack trace from previous location where exception was thrown ---\r\n   at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()\r\n   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)\r\n   at System.Web.Http.Controllers.ApiControllerActionInvoker.<InvokeActionAsyncCore>d__1.MoveNext()\r\n--- End of stack trace from previous location where exception was thrown ---\r\n   at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()\r\n   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)\r\n   at System.Web.Http.Controllers.ActionFilterResult.<ExecuteAsync>d__5.MoveNext()\r\n--- End of stack trace from previous location where exception was thrown ---\r\n   at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()\r\n   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)\r\n   at System.Web.Http.Filters.AuthorizationFilterAttribute.<ExecuteAuthorizationFilterAsyncCore>d__3.MoveNext()\r\n--- End of stack trace from previous location where exception was thrown ---\r\n   at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()\r\n   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)\r\n   at System.Web.Http.Controllers.AuthenticationFilterResult.<ExecuteAsync>d__5.MoveNext()\r\n--- End of stack trace from previous location where exception was thrown ---\r\n   at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()\r\n   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)\r\n   at System.Web.Http.Dispatcher.HttpControllerDispatcher.<SendAsync>d__15.MoveNext()"
*/
namespace Magazin.Library.Internal.DataAccess

{
    public class SqlDataAccess : IDisposable, ISqlDataAccess
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;
        public SqlDataAccess(IConfiguration config, ILogger<SqlDataAccess> logger)
        {
            _config = config;
            _logger = logger;
        }
        public string GetConnectionString(string name)
        {
            return _config.GetConnectionString(name);
        }

        //Implementation of LoadData method
        public List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                List<T> rows = connection.Query<T>(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure).ToList();
                return rows;
            }
        }
        //Implementation of SaveData method
        public void SaveData<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Execute(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public void StartTransaction(string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            _connection = new SqlConnection(connectionString);

            _connection.Open();

            _transaction = _connection.BeginTransaction();

            isClosed = false;

        }

        public List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters)
        {

            List<T> rows = _connection.Query<T>(storedProcedure, parameters,
                commandType: CommandType.StoredProcedure,
                transaction: _transaction).ToList();

            return rows;

        }

        public void SaveDataInTransaction<T>(string storedProcedure, T parameters)
        {
            _connection.Execute(storedProcedure, parameters,
                     commandType: CommandType.StoredProcedure,
                     transaction: _transaction);

        }

        private bool isClosed = false;

        public void CommitTransaction()
        {
            _transaction?.Commit();
            _connection?.Close();

            isClosed = true;

        }
        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _connection?.Close();

            isClosed = true;
        }

        public void Dispose()
        {
            if (isClosed == false)
            {
                try
                {
                    CommitTransaction();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message + "Error in Dispose CommitTransaction: ");
                }


            }

            _transaction = null;
            _connection = null;
        }

        // Open connect/start transaction method 
        // load using the transaction 
        // save using the transaction
        // close connection/stop transaction method
        // Dispose

    }
}






