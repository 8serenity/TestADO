using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestADO;

namespace TestADO
{
    /// <summary>
    /// Interaction logic for RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Window
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private void BackToLogin(object sender, RoutedEventArgs e)
        {
            MainWindow loginWindow = new MainWindow();
            this.Close();
            loginWindow.Show();
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            if ((passwordConfirmation != password) || (password.Text.Length <= 6))
            {
                RegistrationPage newRegistrationPage = new RegistrationPage();
                newRegistrationPage.Show();
                this.Close();
            }


            DbCommand command = ConnectionHelper.Connection.CreateCommand();

            DbParameter nameParameter = command.CreateParameter();
            nameParameter.DbType = System.Data.DbType.String;
            nameParameter.IsNullable = false;
            nameParameter.ParameterName = "@Name";
            nameParameter.Value = newLogin.Text;


            DbParameter passwordHash = command.CreateParameter();
            passwordHash.DbType = System.Data.DbType.String;
            passwordHash.IsNullable = false;
            passwordHash.ParameterName = "@PasswordHash";
            passwordHash.Value = password.Text.GetHashCode().ToString();


            command.Parameters.AddRange(new DbParameter[] { nameParameter, passwordHash });
            command.CommandText = "insert into Users (Name,PasswordHash) values (@Name,@PasswordHash);";

            ConnectionHelper.ExecuteCommands(command);


        }
    }
}
