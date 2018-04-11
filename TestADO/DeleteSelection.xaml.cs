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
using System.Windows.Shapes;

namespace TestADO
{
    /// <summary>
    /// Interaction logic for DeleteSelection.xaml
    /// </summary>
    public partial class DeleteSelection : Window
    {
        List<int> userFileIdsShow;
        User loggedUser;
        public DeleteSelection(List<int>userFileIds, User loggedUser)
        {
            InitializeComponent();
            this.loggedUser = loggedUser;
            userFileIdsShow = userFileIds;
            userFilesBox.ItemsSource = userFileIdsShow;

        }

        private void DeleteUserFile(object sender, RoutedEventArgs e)
        {
            DbCommand command = ConnectionHelper.Connection.CreateCommand();

            DbParameter fileIdParameter = command.CreateParameter();
            fileIdParameter.DbType = System.Data.DbType.Int32;
            fileIdParameter.IsNullable = false;
            fileIdParameter.ParameterName = "@Id";
            fileIdParameter.Value = userFilesBox.SelectedValue;

            command.Parameters.AddRange(new DbParameter[] { fileIdParameter });
            command.CommandText = "DELETE FROM UserFiles WHERE Id = @Id";

            ConnectionHelper.ExecuteCommands(command);
            UserFiles newWindow = new UserFiles(loggedUser);
            newWindow.Show();
            Close();
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            UserFiles newWindow = new UserFiles(loggedUser);
            newWindow.Show();
            Close();
        }
    }
}
