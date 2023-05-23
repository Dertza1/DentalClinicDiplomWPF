using PrivateDentalClinic.DB;
using PrivateDentalClinic.Views;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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

namespace PrivateDentalClinic.Windows.Edit
{
    /// <summary>
    /// Логика взаимодействия для EditStatusAppointmentWindow.xaml
    /// </summary>
    public partial class EditStatusAppointmentWindow : Window
    {
        private readonly DentalClinicEntities DbContext;
        private static Appointment AppointmentEdit { get; set; } = new Appointment();
        public EditStatusAppointmentWindow(Appointment appoinment)
        {
            DbContext = DentalClinicEntities.GetContext();
            AppointmentEdit = appoinment;
            InitializeComponent();

            ComboBoxStatusData();

        }

        private void ComboBoxStatusData()
        {
            var Services = DbContext.StatusAppointments.ToList();

            ComboBoxStatus.Items.Add("");

            foreach (var item in Services)
            {
                ComboBoxStatus.Items.Add($"{item.StatusName}");
            }
        }
        private void ButtonEditStatus_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxStatus.SelectedItem != null && ComboBoxStatus.SelectedIndex != 0)
            {
                AppointmentEdit.StatusAppointment = DbContext.StatusAppointments.FirstOrDefault(b => b.StatusName == ComboBoxStatus.SelectedItem.ToString());

                var client = AppointmentEdit.Client;

                if (ComboBoxStatus.SelectedItem.ToString() == "Принят")
                {
                    client.HistoryAppointments.Add(new HistoryAppointment { Appointment = AppointmentEdit, Client = client });
                }
                

                DbContext.Appointments.AddOrUpdate(AppointmentEdit);
                DbContext.Clients.AddOrUpdate(client);
                DbContext.SaveChanges();

                InfoMessageWindow infoMessage = new InfoMessageWindow("Статус успешно изменен");
                infoMessage.ShowDialog();
                return;
            }

            InfoMessageWindow errorMessage = new InfoMessageWindow("Выберите статус");
            errorMessage.ShowDialog();
            
        }
    }
}
