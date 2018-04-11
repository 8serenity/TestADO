using System;
using System.Collections.Generic;
using System.Data.Common;
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

namespace TestADO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Registration(object sender, RoutedEventArgs e)
        {
            RegistrationPage newWindow = new RegistrationPage();
            this.Close();
            newWindow.ShowDialog();
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {

            //return context.Product_comments.Where(comment => comment.product_id == product.products_id).ToList();
            User loginUser;


            DbConnection connection = ConnectionHelper.Connection;
            connection.Open();
            
            DbCommand command = ConnectionHelper.Connection.CreateCommand();

            command.Connection = connection;

            List<User> users = new List<User>();

            command.CommandText = "select * from Users";
            
            DbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                users.Add(
                    new User
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"].ToString(),
                        PasswordHash = reader["PasswordHash"].ToString(),
                        DateRegistered = (DateTime)reader["DateRegistered"]
                    });
            }

            loginUser = users.SingleOrDefault(c => c.Name == loginBox.Text.ToString());


            if ((loginUser != null) && (loginUser.PasswordHash == passwordBox.Text.GetHashCode().ToString()))
            {
                UserFiles newWindow = new UserFiles(loginUser);
                this.Close();
                newWindow.Show();
            }

        }
    }
}
