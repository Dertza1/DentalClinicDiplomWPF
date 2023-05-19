using PrivateDentalClinic.DB;
using PrivateDentalClinic.Views;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;

namespace PrivateDentalClinic.Windows
{
    public partial class CreateAppointment : Window
    {
        private readonly DentalClinicEntities DbContext;

        private Doctor DoctorSelect { get; set; }
        private bool IsRegistration { get; set; } = false;
        public CreateAppointment()
        {
            DbContext = DentalClinicEntities.GetContext();

            InitializeComponent();

            DoctorsDataComboBox();
            ServiceDataComboBox();
            ClientsDataDataGrid();
        }

        #region
        private void TalonsDataGrid(string date)
        {
            var doctorAppointment = DoctorSelect.Appointments.Where(b=>b.DateAppointment == Convert.ToDateTime(date)).ToList();

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

            talonsDataGrid.ItemsSource = Talons;
        }

        private void ClientsDataDataGrid()
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

        private void DoctorsDataComboBox()
         {
            var date = DateTime.Now.Date;
            var Doctors = DbContext.Doctors.Where(b=>b.WorkSchedules.Count > 0 && b.WorkSchedules.Any(x => x.Date >= date)).ToList();

            if (Doctors.Count == 0)
            {
                ComboBoxSelectDoctor.Items.Add("Нет времени для записи");
                return;
            }

            ComboBoxSelectDoctor.Items.Add("");

            foreach (var item in Doctors)
            {
                ComboBoxSelectDoctor.Items.Add($"{item.LastName} {item.FirstName} {item.MiddleName}");
            }
        }

        private void ServiceDataComboBox()
        {
            var Services = DbContext.Services.ToList();

            ComboBoxSelectService.Items.Add("");

            foreach (var item in Services)
            {
                ComboBoxSelectService.Items.Add($"{item.ServiceName}");
            }
        }

        private void TextBoxSearchClient_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            ClientsDataDataGrid();
        }

        private void ButtonRegistrationClient_Click(object sender, RoutedEventArgs e)
        {
            StackPanelClients.Visibility = Visibility.Hidden;
            StackPanelRegistration.Visibility = Visibility.Visible;
            IsRegistration = true;
        }

        private void ButtonBackToClients_Click(object sender, RoutedEventArgs e)
        {
            StackPanelClients.Visibility = Visibility.Visible;
            StackPanelRegistration.Visibility = Visibility.Hidden;
            IsRegistration = false;
            ClientsDataDataGrid();
        }

        private void ComboBoxSelectDoctor_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ComboBoxSelectDate.SelectedIndex = 0;
            talonsDataGrid.ItemsSource = null;
            StackPanelTimes.Visibility = Visibility.Collapsed;

            DatesWorkSchedule();
        }

        private void DatesWorkSchedule()
        {
            ComboBoxSelectDate.Items.Clear();

            if (ComboBoxSelectDoctor.SelectedItem == null || ComboBoxSelectDoctor.SelectedIndex == 0)
            {
                return;
            }

            string[] doctorSelect = ComboBoxSelectDoctor.SelectedItem.ToString().Split(' ');
            string lname = doctorSelect[0];
            string fname = doctorSelect[1];
            string mname = doctorSelect[2];

            DoctorSelect = DbContext.Doctors.Where(b => b.LastName == lname && b.FirstName == fname && b.MiddleName == mname).Include(b => b.WorkSchedules).Include(b => b.Appointments).FirstOrDefault();

            ComboBoxSelectDate.Items.Add("");

            
            foreach (var item in DoctorSelect.WorkSchedules)
            {
                ComboBoxSelectDate.Items.Add($"{item.Date.ToShortDateString()}");
            }
        }

        private void ComboBoxSelectDate_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ComboBoxSelectDate.SelectedIndex != 0 && ComboBoxSelectDate.Items.Count > 0)
            {
                StackPanelTimes.Visibility = Visibility.Visible;
                TalonsDataGrid(ComboBoxSelectDate.SelectedItem.ToString());
            }
        }

        #endregion

        private async void ButtonCreateAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (!IsRegistration)
            {
                if (ComboBoxSelectDoctor.SelectedItem == null || ComboBoxSelectDoctor.SelectedIndex == 0 ||
                ComboBoxSelectDate.SelectedItem == null || ComboBoxSelectDate.SelectedIndex == 0 || talonsDataGrid.SelectedItem == null
                || ComboBoxSelectService.SelectedItem == null || ComboBoxSelectService.SelectedIndex == 0 || DataGridClients.SelectedItem == null)
                {
                    InfoMessageWindow errorMessage = new InfoMessageWindow("Заполните все данные");
                    errorMessage.ShowDialog();
                    return;
                }


                var doctor = DoctorSelect;

                var client = DbContext.Clients.Include(b => b.Appointments).Include(b => b.HistoryAppointments).FirstOrDefault(b => b.ClientID == ((ClientsView)(DataGridClients.SelectedItem)).ClientID);

                var services = DbContext.Services.ToList();

                var status = DbContext.StatusAppointments.FirstOrDefault(b => b.StatusName == "Записан");

                var talon = talonsDataGrid.SelectedItem as TalonsView;

                var date = Convert.ToDateTime(ComboBoxSelectDate.Text);



                var newAppointment = new Appointment
                {
                    BeginTimeAppointment = TimeSpan.Parse(talon.TimeBeginAppointment),
                    EndTimeAppointment = TimeSpan.Parse(talon.TimeBeginAppointment) + new TimeSpan(1, 0, 0),
                    Client = client,
                    Doctor = doctor,
                    DateAppointment = date,
                    Service = services.First(b => b.ServiceName == ComboBoxSelectService.Text),
                    StatusAppointment = status
                };

               


                DbContext.Appointments.Add(newAppointment);
                DbContext.Clients.AddOrUpdate(client);

                await DbContext.SaveChangesAsync();

                InfoMessageWindow infoMessage = new InfoMessageWindow("Пациент успешно записан");
                infoMessage.ShowDialog();

            }
            else
            {
                if (ComboBoxSelectDoctor.SelectedItem == null 
                               || ComboBoxSelectDoctor.SelectedIndex == 0 
                               || ComboBoxSelectDate.SelectedItem == null 
                               || ComboBoxSelectDate.SelectedIndex == 0 
                               || ComboBoxSelectService.SelectedItem == null 
                               || ComboBoxSelectService.SelectedIndex == 0
                               || string.IsNullOrEmpty(TextBoxClientLastName.Text) 
                               || string.IsNullOrEmpty(TextBoxClientFirstName.Text)
                               || string.IsNullOrEmpty(TextBoxClientMiddleName.Text) 
                               || string.IsNullOrEmpty(TextBoxClientPhone.Text) 
                               || string.IsNullOrEmpty(TextBoxClientDateOfBirth.Text)
                               || talonsDataGrid.SelectedItem == null)
                {
                    InfoMessageWindow errorMessage = new InfoMessageWindow("Заполните все данные");
                    errorMessage.ShowDialog();
                    return;
                }


                var newClient = new Client();

                try
                {
                    newClient.LastName = TextBoxClientLastName.Text;
                    newClient.FirstName = TextBoxClientFirstName.Text;
                    newClient.MiddleName = TextBoxClientMiddleName.Text;
                    newClient.Phone = TextBoxClientPhone.Text;
                    newClient.DayOfBirth = Convert.ToDateTime(TextBoxClientDateOfBirth.Text);

                }
                catch (Exception)
                {
                    InfoMessageWindow errorMessage = new InfoMessageWindow("Введите корректные данные");
                    errorMessage.ShowDialog();
                    return;
                }


                var doctor = DoctorSelect;

                var services = DbContext.Services.ToList();

                var status = DbContext.StatusAppointments.FirstOrDefault(b => b.StatusName == "Записан");

                var talon = talonsDataGrid.SelectedItem as TalonsView;

                var date = Convert.ToDateTime(ComboBoxSelectDate.Text);



                var newAppointment = new Appointment
                {
                    BeginTimeAppointment = TimeSpan.Parse(talon.TimeBeginAppointment),
                    EndTimeAppointment = TimeSpan.Parse(talon.TimeBeginAppointment) + new TimeSpan(1, 0, 0),
                    Client = newClient,
                    Doctor = doctor,
                    DateAppointment = date,
                    Service = services.First(b => b.ServiceName == ComboBoxSelectService.Text),
                    StatusAppointment = status
                };

                newClient.HistoryAppointments.Add(new HistoryAppointment { Appointment = newAppointment, Client = newClient });


                DbContext.Appointments.Add(newAppointment);
                DbContext.Clients.Add(newClient);

                await DbContext.SaveChangesAsync();

                InfoMessageWindow infoMessage = new InfoMessageWindow("Пациент успешно записан");
                infoMessage.ShowDialog();

            }

        }


    }
}
