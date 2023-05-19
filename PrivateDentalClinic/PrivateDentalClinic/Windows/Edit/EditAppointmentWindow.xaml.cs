using PrivateDentalClinic.DB;
using PrivateDentalClinic.Views;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace PrivateDentalClinic.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditAppointmentWindow.xaml
    /// </summary>
    public partial class EditAppointmentWindow : Window
    {
        private readonly DentalClinicEntities DbContext;
        private static Appointment AppointmentEdit { get; set; } = new Appointment();
        private static Doctor DoctorSelect { get; set; }
        private bool EditTalon { get; set; } = false;
        public EditAppointmentWindow(AppoinmentsView appoinment)
        {

            DbContext = DentalClinicEntities.GetContext();
            AppointmentEdit = DbContext.Appointments.Include(b => b.Service).Include(b => b.Doctor).FirstOrDefault(b => b.AppointmentID == appoinment.AppointmentID);

            InitializeComponent();


            ComboBoxDoctorData();
            ComboBoxServiceData();
            ComboBoxStatusData();
            DataGridClientsData();


            ComboBoxDoctor.Text = AppointmentEdit.Doctor.LastName + " " + AppointmentEdit.Doctor.FirstName + " " + AppointmentEdit.Doctor.MiddleName;
            ComboBoxDate.Text = AppointmentEdit.DateAppointment.ToShortDateString();
            TalonsDataGrid(AppointmentEdit.DateAppointment.ToShortDateString());
            ComboBoxService.Text = AppointmentEdit.Service.ServiceName;
            ComboBoxStatus.Text = AppointmentEdit.StatusAppointment.StatusName;
            TextBlockCurrentTime.Text = AppointmentEdit.BeginTimeAppointment.ToString(@"hh\:mm");


            foreach (var item in DataGridClients.Items)
            {
                var client = item as ClientsView;

                if (client.ClientID == AppointmentEdit.ClientID)
                {
                    DataGridClients.SelectedItem = item;
                }
            }
        }

        private void ComboBoxServiceData()
        {
            var Services = DbContext.Services.ToList();

            ComboBoxService.Items.Add("");

            foreach (var item in Services)
            {
                ComboBoxService.Items.Add($"{item.ServiceName}");
            }
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

        private void ComboBoxDoctorData()
        {
            var date = DateTime.Now.Date;
            var Doctors = DbContext.Doctors.Where(b => b.WorkSchedules.Count > 0 && b.WorkSchedules.Any(x=>x.Date >= date)).ToList();

            ComboBoxDoctor.Items.Add("");
            
            foreach (var item in Doctors)
            {
                ComboBoxDoctor.Items.Add($"{item.LastName} {item.FirstName} {item.MiddleName}");
            }
        }

        private void ComboBoxDatesData()
        {
            var date = DateTime.Now.Date;

            ComboBoxDate.Items.Clear();

            if (ComboBoxDoctor.SelectedItem == null || ComboBoxDoctor.SelectedIndex == 0)
            {
                return;
            }

            string[] doctorSelect = ComboBoxDoctor.SelectedItem.ToString().Split(' ');
            string lname = doctorSelect[0];
            string fname = doctorSelect[1];
            string mname = doctorSelect[2];

            DoctorSelect = DbContext.Doctors.Where(b => b.LastName == lname && b.FirstName == fname && b.MiddleName == mname).Include(b => b.WorkSchedules).Include(b => b.Appointments).FirstOrDefault();

            ComboBoxDate.Items.Add("");


            foreach (var item in DoctorSelect.WorkSchedules)
            {
                ComboBoxDate.Items.Add($"{item.Date.ToShortDateString()}");
            }
        }

        private void DataGridClientsData()
        {
            var clientsDb = DbContext.Clients.ToList();

            var Clients = new List<ClientsView>();


            foreach (var item in clientsDb)
            {
                var client = new ClientsView
                {
                    ClientID = item.ClientID,
                    ClientName = $"{item.LastName} {item.FirstName} {item.MiddleName}",
                    DateOfBirth = item.DayOfBirth.ToShortDateString(),
                };

                Clients.Add(client);
            }


            if (!string.IsNullOrEmpty(TextBoxSearchClient.Text))
            {
                Clients = Clients.Where(b => b.ClientName.Contains(TextBoxSearchClient.Text)).ToList();
            }

            DataGridClients.ItemsSource = Clients;
        }

        private void TalonsDataGrid(string date)
        {
            var doctorAppointment = DoctorSelect.Appointments.Where(f=>f.DateAppointment == Convert.ToDateTime(date)).ToList();

            var doctorSchedule = DoctorSelect.WorkSchedules.FirstOrDefault(b => b.Date == Convert.ToDateTime(date));

            var Talons = new List<TalonsView>();

            var hour = new TimeSpan(1, 0, 0);

            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            for (TimeSpan i = doctorSchedule.BeginWorkDay; i <= doctorSchedule.EndWorkDay - hour; i += hour)
            {
                if (DateTime.Now.Date == Convert.ToDateTime(date))
                {
                    if (!doctorAppointment.Any(b => b.BeginTimeAppointment == i) && i >= currentTime)
                    {
                        var talon = new TalonsView(i.ToString(@"hh\:mm"));

                        Talons.Add(talon);
                    }
                }
                else
                {
                    if (!doctorAppointment.Any(b => b.BeginTimeAppointment == i))
                    {
                        var talon = new TalonsView(i.ToString(@"hh\:mm"));

                        Talons.Add(talon);
                    }
                }

            }

            if (Talons.Count > 0)
            {
                talonsDataGrid.ItemsSource = Talons;
            }
            else
            {
                var talon = new List<TalonsView>
                {
                    new TalonsView("Нет свободного времени"),
                };

                talonsDataGrid.ItemsSource = talon;
            }

        }

        private void ButtonEditAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (EditTalon)
            {
                if (ComboBoxDoctor.SelectedItem == null || ComboBoxDoctor.SelectedIndex == 0 ||
               ComboBoxDate.SelectedItem == null || ComboBoxDate.SelectedIndex == 0 || talonsDataGrid.SelectedItem == null
               || ComboBoxService.SelectedItem == null || ComboBoxService.SelectedIndex == 0 || DataGridClients.SelectedItem == null || ComboBoxStatus.SelectedItem == null || ComboBoxStatus.SelectedIndex == 0)
                {
                    InfoMessageWindow errorMessage = new InfoMessageWindow("Заполните все данные");
                    errorMessage.ShowDialog();
                    return;
                }

                var oldStatus = AppointmentEdit.StatusAppointment;

                var newStatus = DbContext.StatusAppointments.FirstOrDefault(b => b.StatusName == ComboBoxStatus.SelectedItem.ToString());
                var services = DbContext.Services.FirstOrDefault(b => b.ServiceName == ComboBoxService.SelectedItem.ToString());
                var client = DbContext.Clients.FirstOrDefault(b => b.ClientID == ((ClientsView)(DataGridClients.SelectedItem)).ClientID);
                var talonTime = talonsDataGrid.SelectedItem as TalonsView;


                AppointmentEdit.DateAppointment = Convert.ToDateTime(ComboBoxDate.SelectedItem.ToString());
                AppointmentEdit.BeginTimeAppointment = TimeSpan.Parse(talonTime.TimeBeginAppointment);
                AppointmentEdit.EndTimeAppointment = TimeSpan.Parse(talonTime.TimeBeginAppointment) + new TimeSpan(1, 0, 0);

                var currentDate = DateTime.Now.Date;
                var currentTime = DateTime.Now.TimeOfDay;

                if ((newStatus.StatusName == "Принят" || newStatus.StatusName == "Не явился") && AppointmentEdit.DateAppointment > currentDate && AppointmentEdit.BeginTimeAppointment < currentTime)
                {
                    InfoMessageWindow errorMessage = new InfoMessageWindow("Невозможно установить статус 'Принят' до наступления времени приема");
                    errorMessage.ShowDialog();
                    return;
                }

                AppointmentEdit.StatusAppointment = newStatus;

                AppointmentEdit.Service = services;

                AppointmentEdit.Client = client;

                AppointmentEdit.Doctor = DoctorSelect;

                if (newStatus.StatusName == "Принят" && oldStatus.StatusName != "Принят")
                {
                    client.HistoryAppointments.Add(new HistoryAppointment { Appointment = AppointmentEdit, Client = client });
                }

                DbContext.Appointments.AddOrUpdate(AppointmentEdit);
                DbContext.Clients.AddOrUpdate(client);

                DbContext.SaveChangesAsync();

                InfoMessageWindow infoMessage = new InfoMessageWindow("Запись на прием успешно изменена");
                infoMessage.ShowDialog();
            }

            else
            {
                if (ComboBoxDoctor.SelectedItem == null || ComboBoxDoctor.SelectedIndex == 0
                || ComboBoxDate.SelectedItem == null || ComboBoxDate.SelectedIndex == 0
                || ComboBoxService.SelectedItem == null || ComboBoxService.SelectedIndex == 0 
                || DataGridClients.SelectedItem == null || ComboBoxStatus.SelectedItem == null 
                || ComboBoxStatus.SelectedIndex == 0)
                {
                    InfoMessageWindow errorMessage = new InfoMessageWindow("Заполните все данные");
                    errorMessage.ShowDialog();
                    return;
                }

                var oldStatus = AppointmentEdit.StatusAppointment;

                var newStatus = DbContext.StatusAppointments.FirstOrDefault(b => b.StatusName == ComboBoxStatus.SelectedItem.ToString());
                var services = DbContext.Services.FirstOrDefault(b => b.ServiceName == ComboBoxService.SelectedItem.ToString());
                var client = DbContext.Clients.FirstOrDefault(b => b.ClientID == ((ClientsView)(DataGridClients.SelectedItem)).ClientID);
                

                var currentDate = DateTime.Now.Date;
                var currentTime = DateTime.Now.TimeOfDay;

                if ((newStatus.StatusName == "Принят" || newStatus.StatusName == "Не явился")  && AppointmentEdit.DateAppointment > currentDate && AppointmentEdit.BeginTimeAppointment < currentTime)
                {
                    InfoMessageWindow errorMessage = new InfoMessageWindow("Невозможно установить статус до наступления времени приема");
                    errorMessage.ShowDialog();
                    return;
                }

                AppointmentEdit.StatusAppointment = newStatus;

                AppointmentEdit.Service = services;

                AppointmentEdit.Client = client;

                if (newStatus.StatusName == "Принят" && oldStatus.StatusName != "Принят")
                {
                    client.HistoryAppointments.Add(new HistoryAppointment { Appointment = AppointmentEdit, Client = client });
                }

                DbContext.Appointments.AddOrUpdate(AppointmentEdit);
                DbContext.Clients.AddOrUpdate(client);

                DbContext.SaveChangesAsync();

                InfoMessageWindow infoMessage = new InfoMessageWindow("Запись на прием успешно изменена");
                infoMessage.ShowDialog();
            }    
        }

        private void ButtonDeleteAppointment_Click(object sender, RoutedEventArgs e)
        {
            

        }

        private void ComboBoxDoctor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxDate.SelectedIndex = 0;
            ComboBoxDate.Text = "";
            talonsDataGrid.ItemsSource = null;

            var doctor = AppointmentEdit.Doctor.LastName + " " + AppointmentEdit.Doctor.FirstName + " " + AppointmentEdit.Doctor.MiddleName;
            var date = AppointmentEdit.DateAppointment.ToShortDateString();

            if (ComboBoxDoctor.SelectedItem.ToString() != doctor)
            {
                StackPanelEditTalon.Visibility = Visibility.Visible;
                StackPanelCurrentTime.Visibility = Visibility.Collapsed;
                EditTalon = true;
                ButtonCancelEditTalonTime.Visibility = Visibility.Collapsed;
            }
           
            ComboBoxDatesData();

        }

        private void ComboBoxDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var doctor = AppointmentEdit.Doctor.LastName + " " + AppointmentEdit.Doctor.FirstName + " " + AppointmentEdit.Doctor.MiddleName;
            var date = AppointmentEdit.DateAppointment.ToShortDateString();

            if (ComboBoxDate.SelectedIndex != 0 && ComboBoxDate.Items.Count > 0)
            {
                if (ComboBoxDoctor.SelectedItem.ToString() == doctor && ComboBoxDate.SelectedItem.ToString() == date)
                {
                    StackPanelCurrentTime.Visibility = Visibility.Visible;
                    StackPanelEditTalon.Visibility = Visibility.Collapsed;
                    EditTalon = false;
                    ButtonCancelEditTalonTime.Visibility = Visibility.Visible;
                }
                else
                {
                    TalonsDataGrid(ComboBoxDate.SelectedItem.ToString());
                    StackPanelEditTalon.Visibility = Visibility.Visible;
                    StackPanelCurrentTime.Visibility = Visibility.Collapsed;
                    EditTalon = true;
                    ButtonCancelEditTalonTime.Visibility = Visibility.Collapsed;
                }
            }
        }
        private void ButtonEditTalonTime_Click(object sender, RoutedEventArgs e)
        {
            StackPanelEditTalon.Visibility = Visibility.Visible;
            StackPanelCurrentTime.Visibility = Visibility.Collapsed;
            TalonsDataGrid(ComboBoxDate.SelectedItem.ToString());
            EditTalon = true;
        }

        private void ButtonCancelEditTalonTime_Click(object sender, RoutedEventArgs e)
        {
            StackPanelCurrentTime.Visibility = Visibility.Visible;
            StackPanelEditTalon.Visibility = Visibility.Collapsed;
            EditTalon = false;
        }

        private void ButtonCancelAppointment_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBoxSearchClient_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            DataGridClientsData();
        }


    }
}
