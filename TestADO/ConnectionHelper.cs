using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestADO
{
    public class ConnectionHelper
    {
        public static DbConnection Connection
        {
            get
            {
                DbProviderFactory providerFactory = DbProviderFactories.GetFactory(ConfigurationManager
                                            .ConnectionStrings["Default"].ProviderName);
                DbConnection connection = providerFactory.CreateConnection();

                connection.ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                return connection;
            }
        }
        public static bool ExecuteCommands(params DbCommand[] commands)
        {
            using (DbConnection connection = Connection)
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    foreach (var command in commands)
                    {
                        command.Connection = connection;
                        command.Transaction = transaction;
                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    return true;
                }
                catch (DbException ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
    }
}

