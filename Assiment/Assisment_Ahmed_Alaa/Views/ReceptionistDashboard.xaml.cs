using Assisment_Ahmed_Alaa.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
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
    /// Interaction logic for ReceptionistDashboard.xaml
    /// </summary>
    public partial class ReceptionistDashboard : Window
    {
        private DateTimeStyles style;

        public ReceptionistDashboard()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                using var db = new AppoimentHandlingDB();

                var app = db.Appointment
                    .Include(e => e.patient)
                    .Include(e => e.Doctor).Select(x => new Helper.Helper
                    {
                        PatId = x.patient.PatientID,
                        PatName = x.patient.Name,
                        Phone = x.patient.Phone,
                        Age = x.patient.Age,
                        Notes = x.patient.Notes,
                        DocName = x.Doctor.Name,
                        DT = x.DateTime,
                        Reas = x.Reason,
                        Status = x.Status,
                        Docid = x.DoctorID,

                    }).ToList();
                Display.ItemsSource = app;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException?.Message ?? ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Add_btn(object sender, RoutedEventArgs e)
        {
            using var db = new AppoimentHandlingDB();

            if (string.IsNullOrWhiteSpace(txtPatName.Text)
                || string.IsNullOrWhiteSpace(txtphone.Text)
                || string.IsNullOrWhiteSpace(txtAge.Text)
                || string.IsNullOrWhiteSpace(txtNotes.Text)
                || string.IsNullOrWhiteSpace(txtreason.Text)

                )
            {
                MessageBox.Show("Please Fill all the inputs!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!int.TryParse(txtDocId.Text, out int DocId))
            {
                MessageBox.Show("Please Enter Doctor id as a number !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;


            }
            if (!int.TryParse(txtpatid.Text, out int PatId))
            {
                MessageBox.Show("Please Enter Patient ID as a number !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }
            if (!int.TryParse(txtAge.Text, out int Age))
            {
                MessageBox.Show("Please Enter Age as a number !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var combosta = ((ComboBoxItem)combostatus.SelectedItem).Content.ToString();
            if (combosta == null)
            {
                MessageBox.Show("Please select status !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var doctor = db.User.FirstOrDefault(x => x.UserId == DocId);
            if (doctor == null)
            {
                MessageBox.Show("Doctor is not found ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }
            var patient = db.Patient.FirstOrDefault(x => x.PatientID == PatId);
            if (patient != null)
            {
                MessageBox.Show("The patient has an Appointment");
                return;
            }

            var app = new Appointment()
            {
                Reason = txtreason.Text,
                DoctorID = DocId,
                Status = combosta,
                Pat_Id = PatId,
                DateTime = DateTime.Now,
            };
            var newPatient = new Patient()
            {
                Age = Age,
                Name = txtPatName.Text,
                Notes = txtNotes.Text,
                appointment = app,
                Phone = txtphone.Text,
                PatientID = PatId

            };
            db.Appointment.Add(app);
            db.Patient.Add(newPatient);
            db.SaveChanges();
            LoadData();
            MessageBox.Show("The Appointment is added successfully !");


        }

        private void Delete_btn(object sender, RoutedEventArgs e)
        {
            using var db = new AppoimentHandlingDB();

            try
            {
                var dele = db.Patient.FirstOrDefault(x => x.PatientID == int.Parse(txtpatid.Text) && x.Name == txtPatName.Text);
                if (dele == null)
                {
                    MessageBox.Show("Patient is not found to delete", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                db.Patient.Remove(dele);
                db.SaveChanges();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException?.Message ?? ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Search_Btn(object sender, RoutedEventArgs e)
        {
            using var db = new AppoimentHandlingDB();

            if (string.IsNullOrWhiteSpace(txtsearch.Text))
            {
                MessageBox.Show("Please Fill the input", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var pat = db.Patient.FirstOrDefault(x => x.Name == txtsearch.Text);
            if (pat == null)
            {
                MessageBox.Show("User Is Not Found ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show($"PatientID:{pat.PatientID}\n,PatientName:{pat.Name}\n,PatientPhone:{pat.Phone}\n," +
                $"PatientDOB:{pat.Age}\n,PatientNotes:{pat.Notes}\n", "Patient Info", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void Display_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Display.SelectedItem == null) return;

            var x = Display.SelectedItem as Helper.Helper;
            if (x != null)
            {
                txtAge.Text = x.Age.ToString();
                txtDocId.Text = x.Docid.ToString();
                txtNotes.Text = x.Notes.ToString();
                txtpatid.Text = x.PatId.ToString();
                txtPatName.Text = x.PatName.ToString();
                txtphone.Text = x.Phone.ToString();
                txtreason.Text = x.Reas.ToString();
                txtAge.Text = x.Age.ToString();
                foreach (ComboBoxItem item in combostatus.Items)
                {
                    if (item.Content.ToString() == x.Status)
                    {
                        combostatus.SelectedItem = item;
                        break;
                    }

                }
            }
        }

        private void clear_btn(object sender, RoutedEventArgs e)
        {
            txtAge.Clear();
            txtDocId.Clear();
            txtNotes.Clear();
            txtpatid.Clear();
            txtPatName.Clear();
            txtphone.Clear();
            txtreason.Clear();
            txtdattime.Clear();
            combostatus.SelectedIndex = -1;
        }

        private void Edit_btn(object sender, RoutedEventArgs e)
        {
            try
            {


                using var db = new AppoimentHandlingDB();



                var x = db.Patient.FirstOrDefault(x => x.PatientID == int.Parse(txtpatid.Text));
                if (x == null)
                {
                    MessageBox.Show("patient is not found ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                x.Name = txtPatName.Text;
                x.Notes = txtNotes.Text;
                x.Age = int.Parse(txtAge.Text);
                x.Phone = txtphone.Text;
                db.SaveChanges();
                LoadData();
                MessageBox.Show("Patient Update Sucessfully", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException?.Message ?? ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
