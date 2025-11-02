using Assisment_Ahmed_Alaa.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

namespace Assisment_Ahmed_Alaa.Views
{
    /// <summary>
    /// Interaction logic for DoctorDashboard.xaml
    /// </summary>
    public partial class DoctorDashboard : Window
    {
        public DoctorDashboard()
        {
            InitializeComponent(); LoadData();
        }
        private void LoadData()
        {

            using var db = new AppoimentHandlingDB();

            var app = db.Appointment
                .Include(e => e.patient)
                .Include(e => e.Doctor).Select(x => new Helper.Helper
                {
                    PatId = x.patient.PatientID,
                    PatName = x.patient.Name,
                    DocName = x.Doctor.Name,
                    DT = x.DateTime,
                    Reas = x.Reason,
                    Status = x.Status,

                }).Where(x => (x.Status == "Scheduled" || x.Status == "Cancelled") && x.DT.Date == DateTime.Today).ToList();
            canceled.ItemsSource = app;


            var appxop = db.Appointment
             .Include(e => e.patient)
             .Include(e => e.Doctor).Select(x => new Helper.Helper
             {
                 PatId = x.patient.PatientID,
                 PatName = x.patient.Name,
                 DocName = x.Doctor.Name,
                 DT = x.DateTime,
                 Reas = x.Reason,
                 Status = x.Status,

             }).Where(x => x.Status == "Completed" && x.DT.Date == DateTime.Today).ToList();
            Comple.ItemsSource = appxop;
        }
        private void Save_btn(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new AppoimentHandlingDB();

                var app = db.Appointment.FirstOrDefault(x => x.Pat_Id == int.Parse(txtPatid.Text));
                var st = ((ComboBoxItem)status.SelectedItem).Content.ToString();
                if (app != null)
                {
                    app.Status = st!;
                    db.SaveChanges();
                    LoadData();
                }
                MessageBox.Show("Ok");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException?.Message ?? ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}