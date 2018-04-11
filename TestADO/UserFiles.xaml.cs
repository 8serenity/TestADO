using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.IO;
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
    /// Interaction logic for UserFiles.xaml
    /// </summary>
    public partial class UserFiles : Window
    {
        private User loggedUser;
        List<int> userFileIdsShow;
        public UserFiles(User newUser)
        {
            InitializeComponent();
            loggedUser = newUser;
            userFileIdsShow = new List<int>();
            DbConnection connection = ConnectionHelper.Connection;
            connection.Open();

            DbCommand command = ConnectionHelper.Connection.CreateCommand();

            command.Connection = connection;

            ObservableCollection<UserFile> userData = new ObservableCollection<UserFile>();

            command.CommandText = "select * from UserFiles where UserId =" + newUser.Id.ToString();

            DbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                userData.Add(
                    new UserFile
                    {
                        Id = (int)reader["Id"],
                        UserId = (int)reader["UserId"],
                        FilePath = reader["FilePath"].ToString(),
                        Filesize = (double)reader["Filesize"],
                        FileExtension = reader["FileExtension"].ToString(),
                        DateCreated = DateTime.Parse(reader["DateCreated"].ToString())
                    });

                userFileIdsShow.Add((int)reader["Id"]);
            }

            userFilesView.ItemsSource = userData;
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            DeleteSelection newPage = new DeleteSelection(userFileIdsShow, loggedUser);
            Close();
            newPage.ShowDialog();
        }

        private void UploadClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a file";
            op.ShowDialog();

            string destinationFile = @"C:\test\" + op.SafeFileName;

            File.Copy(op.FileName, destinationFile, true);
            FileInfo newFileInfo = new FileInfo(op.FileName);

            DbCommand command = ConnectionHelper.Connection.CreateCommand();

            DbParameter userIdParameter = command.CreateParameter();
            userIdParameter.DbType = System.Data.DbType.Int32;
            userIdParameter.IsNullable = false;
            userIdParameter.ParameterName = "@UserId";
            userIdParameter.Value = loggedUser.Id;


            DbParameter FilePath = command.CreateParameter();
            FilePath.DbType = System.Data.DbType.String;
            FilePath.IsNullable = false;
            FilePath.ParameterName = "@FilePath";
            FilePath.Value = destinationFile;


            DbParameter Filesize = command.CreateParameter();
            Filesize.DbType = System.Data.DbType.Decimal;
            Filesize.IsNullable = false;
            Filesize.ParameterName = "@Filesize";
            Filesize.Value = newFileInfo.Length;

            DbParameter FileExtension = command.CreateParameter();
            FileExtension.DbType = System.Data.DbType.String;
            FileExtension.IsNullable = false;
            FileExtension.ParameterName = "@FileExtension";
            FileExtension.Value = newFileInfo.Extension;

            command.Parameters.AddRange(new DbParameter[] { userIdParameter, FilePath,Filesize,FileExtension });
            command.CommandText = "insert into UserFiles (UserId,FilePath,Filesize,FileExtension) values (@UserId,@FilePath,@Filesize,@FileExtension);";

            ConnectionHelper.ExecuteCommands(command);

            UserFiles newWindow = new UserFiles(loggedUser);
            newWindow.Show();
            Close();
        }
    }
}
