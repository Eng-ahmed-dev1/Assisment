using Assisment_Ahmed_Alaa.Models;
using Assisment_Ahmed_Alaa.Views;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Assisment_Ahmed_Alaa
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_btn(object sender, RoutedEventArgs e)
        {

            try
            {
                using var db = new AppoimentHandlingDB();


                if (string.IsNullOrWhiteSpace(txtUserName.Text))
                {
                    MessageBox.Show("Please enter valied name ","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                    return;

                }
                if (string.IsNullOrWhiteSpace(txtpassword.Password))
                {
                    MessageBox.Show("Please enter valied password ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;

                }

                var user = db.User.FirstOrDefault(x => x.Name == txtUserName.Text && x.Password == txtpassword.Password);
                if (user == null)
                {
                    MessageBox.Show("The User Is Not Found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (user.Role == "Doctor")
                {
                    new DoctorDashboard().Show();
                    this.Close();

                }
                if (user.Role == "Receptionist")
                {
                    new ReceptionistDashboard().Show();
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException?.Message ?? ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}