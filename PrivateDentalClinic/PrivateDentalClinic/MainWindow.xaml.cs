using PrivateDentalClinic.DB;
using PrivateDentalClinic.Views;
using PrivateDentalClinic.Windows;
using PrivateDentalClinic.Windows.Create;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PrivateDentalClinic
{
    public partial class MainWindow : Window
    {
        private readonly DentalClinicEntities DbContext;

        private static Dictionary<int, string> keyMonths = new Dictionary<int, string>();
        private static int NumberMonth { get; set; }
        private static int NumberMonthSelect { get; set; }
       
        private static DateTime SelectDate { get; set; }
        private static string SearchField { get; set; }

        public MainWindow()
        {
            DbContext = DentalClinicEntities.GetContext();

            InitializeComponent();

            NumberMonth = DateTime.Now.Date.Month;
            NumberMonthSelect = NumberMonth;

            SelectDate = DateTime.Now.Date;

            DatePickerAppointment.Text = SelectDate.ToShortDateString();

            DictionaryData();
            ComboBoxDataFilterSpec();

            WorkSchedulesDoctorsData();
            AppointmentListViewData();
            DoctorsListViewData();
            ListViewClientsData();
        }


        #region TabItem Appointments
        private void ComboBoxDataFilterSpec()
        {
            var specializations = DbContext.Specializations.Select(n => n.SpecializationName).ToListAsync();

            ComboBoxFilterSpecAppoinments.Items.Add("");
            ComboBoxSpecFilterDoctors.Items.Add("");

            foreach (var specialization in specializations.Result)
            {
                ComboBoxFilterSpecAppoinments.Items.Add(specialization);
                ComboBoxSpecFilterDoctors.Items.Add(specialization);
            }
        }
        private void AppointmentListViewData()
        {

            var doctorAppointmentsDb = DbContext.Doctors
                .Include(b => b.Appointments)
                .Where(b => b.Appointments
                .Any(x => x.DateAppointment == SelectDate))
                .ToList();

            if (ComboBoxFilterSpecAppoinments.SelectedItem != null && ComboBoxFilterSpecAppoinments.SelectedIndex != 0)
            {
                doctorAppointmentsDb = doctorAppointmentsDb
                    .Where(b => b.Specialization.SpecializationName == ComboBoxFilterSpecAppoinments.SelectedItem.ToString())
                    .ToList();
            }

            if (!string.IsNullOrEmpty(TextBoxSearchDoctorAppointments.Text))
            {
                doctorAppointmentsDb = doctorAppointmentsDb
                    .Where(b => b.LastName.ToLower().Contains(TextBoxSearchDoctorAppointments.Text.ToLower())
                    || b.FirstName.ToLower().Contains(TextBoxSearchDoctorAppointments.Text.ToLower())
                    || b.MiddleName.ToLower().Contains(TextBoxSearchDoctorAppointments.Text.ToLower()))
                    .ToList();
            }

            if (doctorAppointmentsDb.Count < 1)
            {
                TextBoxNoAppointment.Visibility = Visibility.Visible;
                ListViewAppointments.Visibility = Visibility.Hidden;
                return;
            }
            else
            {
                TextBoxNoAppointment.Visibility = Visibility.Hidden;
                ListViewAppointments.Visibility = Visibility.Visible;
            }


            var DoctorsAppointmentOut = new List<DoctorsAppoinmentsView>();


            foreach (var doctor in doctorAppointmentsDb)
            {
                string exp = $"{doctor.WorkExperience} год";

                if (Convert.ToDouble(doctor.WorkExperience) < 5 && Convert.ToDouble(doctor.WorkExperience) > 1)
                {
                    exp = $"{doctor.WorkExperience} года";
                }
                else
                {
                    exp = $"{doctor.WorkExperience} лет";
                }



                string path = Environment.CurrentDirectory + $"\\PhotoDoctors\\blankPhoto.jpeg";

                if (!string.IsNullOrEmpty(doctor.Photo))
                {
                    path = Environment.CurrentDirectory + doctor.Photo;
                }



                var doctorAppointment = new DoctorsAppoinmentsView
                {
                    DoctorName = $"{doctor.LastName} {doctor.FirstName} {doctor.MiddleName}",
                    CabinetNumber = $"{doctor.CabinetNumber}",
                    PersonalPhone = $"{doctor.PersonalPhone}",
                    Specialization = $"{doctor.Specialization.SpecializationName}",
                    WorkExp = exp,
                    Photo = path,
                    WorkPhone = $"{doctor.WorkPhone}",
                    AppoinmentsOut = new List<AppoinmentsView>()
                };

                foreach (var appointments in doctor.Appointments.Where(b=>b.DateAppointment == SelectDate))
                {
                    doctorAppointment.AppoinmentsOut.Add(new AppoinmentsView
                    {
                        AppointmentID = appointments.AppointmentID,
                        ClientName = $"{appointments.Client.LastName} {appointments.Client.FirstName} {appointments.Client.MiddleName}",
                        Doctor = doctorAppointment,
                        ClientPhone = appointments.Client.Phone,
                        Date = appointments.DateAppointment.ToShortDateString(),
                        TimeBegin = appointments.BeginTimeAppointment.ToString(@"hh\:mm"),
                        TimeEnd = appointments.EndTimeAppointment.ToString(@"hh\:mm"),
                        Status = appointments.StatusAppointment.StatusName
                    }); ;
                }

                DoctorsAppointmentOut.Add(doctorAppointment);
            }

            ListViewAppointments.ItemsSource = DoctorsAppointmentOut;
        }
        private void ButtonCreateAppointment_Click(object sender, RoutedEventArgs e)
        {
            CreateAppointment createAppointment = new CreateAppointment();
            createAppointment.ShowDialog();

            AppointmentListViewData();
        }
        private void TableAppointments_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            var appointment = dataGrid.SelectedItem as AppoinmentsView;

            EditAppointmentWindow editAppointment = new EditAppointmentWindow(appointment);
            editAppointment.ShowDialog();

            AppointmentListViewData();

        }
        private void DatePickerAppointment_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectDate = (DateTime)DatePickerAppointment.SelectedDate;
            AppointmentListViewData();
        }
        private void ComboBoxFilterSpecAppoinments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AppointmentListViewData();
        }
        private void TextBoxSearchDoctorAppointments_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            AppointmentListViewData();
        }

        #endregion

        #region TabItem WorkSchedules
        private void WorkSchedulesDoctorsData()
        {
            var workSchedules = DbContext.Doctors.Include(v => v.WorkSchedules).ToList();

            TextBoxMonth.Text = keyMonths[NumberMonthSelect];


            var WorkSchedulesOutput = new List<WorkSchedulesView>();

            foreach (var item in workSchedules)
            {
                var days = new List<Days>();

                if (DatePickerWorkSchedules.SelectedDate == null && string.IsNullOrEmpty(TextBoxSearchWorkSchedule.Text))
                {
                    foreach (var itemDay in item.WorkSchedules
                        .Where(b => b.Date.Month == NumberMonthSelect)
                        .OrderBy(b => b.Date))
                    {
                        var day = new Days
                        {
                            DoctorID = item.DoctorID,
                            Date = itemDay.Date.ToShortDateString(),
                            BeginTimeDay = itemDay.BeginWorkDay,
                            EndTimeDay = itemDay.EndWorkDay
                        };

                        days.Add(day);
                    }
                }
                else if (DatePickerWorkSchedules.SelectedDate != null && string.IsNullOrEmpty(TextBoxSearchWorkSchedule.Text))
                {
                    foreach (var itemDay in item.WorkSchedules
                        .Where(b => b.Date.Month == NumberMonthSelect && b.Date == DatePickerWorkSchedules.SelectedDate)
                        .OrderBy(b => b.Date))
                    {
                        var day = new Days
                        {
                            DoctorID = item.DoctorID,
                            Date = itemDay.Date.ToShortDateString(),
                            BeginTimeDay = itemDay.BeginWorkDay,
                            EndTimeDay = itemDay.EndWorkDay
                        };

                        days.Add(day);
                    }
                }
                else if ((DatePickerWorkSchedules.SelectedDate == null && !string.IsNullOrEmpty(TextBoxSearchWorkSchedule.Text)))
                {
                    foreach (var itemDay in item.WorkSchedules
                        .Where(b => b.Date.Month == NumberMonthSelect && b.Doctor.LastName.Contains(SearchField))
                        .OrderBy(b => b.Date))
                    {
                        var day = new Days
                        {
                            DoctorID = item.DoctorID,
                            Date = itemDay.Date.ToShortDateString(),
                            BeginTimeDay = itemDay.BeginWorkDay,
                            EndTimeDay = itemDay.EndWorkDay
                        };

                        days.Add(day);
                    }
                }
                else
                {
                    foreach (var itemDay in item.WorkSchedules
                        .Where(b => b.Date.Month == NumberMonthSelect && b.Date == DatePickerWorkSchedules.SelectedDate && b.Doctor.LastName.Contains(SearchField))
                        .OrderBy(b => b.Date))
                    {
                        var day = new Days
                        {
                            DoctorID = item.DoctorID,
                            Date = itemDay.Date.ToShortDateString(),
                            BeginTimeDay = itemDay.BeginWorkDay,
                            EndTimeDay = itemDay.EndWorkDay
                        };

                        days.Add(day);
                    }
                }


                var objWorkSchedule = new WorkSchedulesView
                {
                    DoctorName = $"{item.LastName} {item.FirstName} {item.MiddleName}",
                    Specialization = item.Specialization.SpecializationName,
                    Days = days
                };

                if (objWorkSchedule.Days.Count > 0)
                {
                    WorkSchedulesOutput.Add(objWorkSchedule);
                }

            }

            if (WorkSchedulesOutput.Count > 0)
            {
                ListViewWorkSchedules.ItemsSource = WorkSchedulesOutput;
                ListViewWorkSchedules.Visibility = Visibility.Visible;
                TextBoxWorkSchedulesNoCreate.Visibility = Visibility.Hidden;
            }
            else
            {
                ListViewWorkSchedules.Visibility = Visibility.Hidden;
                TextBoxWorkSchedulesNoCreate.Visibility = Visibility.Visible;
            }

        }
        private void MonthBack_Click(object sender, RoutedEventArgs e)
        {
            int blank = NumberMonth - 1;

            if (NumberMonth >= NumberMonthSelect)
            {
                return;
            }
            else
            {
                NumberMonthSelect = NumberMonthSelect - 1;

                WorkSchedulesDoctorsData();
            }
        }
        private void MonthNext_Click(object sender, RoutedEventArgs e)
        {
            int blank = NumberMonthSelect + 1;

            if (blank >= 13 || blank <= 0)
            {
                return;
            }
            else
            {
                NumberMonthSelect = NumberMonthSelect + 1;

                WorkSchedulesDoctorsData();
            }


        }
        private void CreateWorkSchedule_Click(object sender, RoutedEventArgs e)
        {
            CreatingWorkScheduleWindow creatingWorkScheduleWindow = new CreatingWorkScheduleWindow();
            creatingWorkScheduleWindow.ShowDialog();

            WorkSchedulesDoctorsData();
        }
        private void TableWorkDays_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            DataGrid dataGrid = (DataGrid)sender;
            var day = dataGrid.SelectedItem as Days;


            ModalQuestionWorkSchedule modalQuestion = new ModalQuestionWorkSchedule(day);
            modalQuestion.ShowDialog();

            WorkSchedulesDoctorsData();
        }
        private void DatePickerWorkSchedules_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            WorkSchedulesDoctorsData();
        }
        private void TextBoxSearchWorkSchedule_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            SearchField = TextBoxSearchWorkSchedule.Text;
            WorkSchedulesDoctorsData();
        }

        #endregion


        #region TabItem Doctors

        

        private void DoctorsListViewData()
        {
            var doctors = DbContext.Doctors.ToList();


            if (!string.IsNullOrEmpty(TextBoxSearchDoctors.Text))
            {
                doctors = doctors.Where(b => b.LastName.ToLower().Contains(TextBoxSearchDoctors.Text.ToLower())
                    || b.FirstName.ToLower().Contains(TextBoxSearchDoctors.Text.ToLower())
                    || b.MiddleName.ToLower().Contains(TextBoxSearchDoctors.Text.ToLower()))
                    .ToList();
            }

            if (ComboBoxSpecFilterDoctors.SelectedItem != null && ComboBoxSpecFilterDoctors.SelectedIndex != 0)
            {
                doctors = doctors
                    .Where(b => b.Specialization.SpecializationName == ComboBoxSpecFilterDoctors.SelectedItem.ToString())
                    .ToList();
            }

            var DoctorsOutput = new List<DoctorsView>();

            foreach (var item in doctors)
            {
                string path = Environment.CurrentDirectory + $"\\PhotoDoctors\\blankPhoto.jpeg";

                if (!string.IsNullOrEmpty(item.Photo))
                {
                   path = Environment.CurrentDirectory + item.Photo;
                }


                var doctor = new DoctorsView
                {
                    DoctorID = item.DoctorID,
                    DoctorName = $"{item.LastName} {item.FirstName} {item.MiddleName}",
                    PersonalPhone = item.PersonalPhone,
                    CabinetNumber = item.CabinetNumber,
                    Specialization = item.Specialization.SpecializationName,
                    WorkExp = item.WorkExperience,
                    WorkPhone = item.WorkPhone,
                    Photo = path
                };

                DoctorsOutput.Add(doctor);
            }

            ListViewDoctors.ItemsSource = DoctorsOutput;
        }


        private void ButtonAddDoctor_Click(object sender, RoutedEventArgs e)
        {
            CreateDoctorWindow createDoctor = new CreateDoctorWindow(null);
            createDoctor.ShowDialog();

            DoctorsListViewData();
        }
        #endregion

        #region TabItem Clients

        private void ListViewClientsData()
        {
            var clients = DbContext.Clients.Include(b => b.HistoryAppointments).ToList();

            if (!string.IsNullOrEmpty(TextBoxSearchClient.Text))
            {
                clients = clients.Where(b => b.LastName.ToLower().Contains(TextBoxSearchClient.Text.ToLower())
                    || b.FirstName.ToLower().Contains(TextBoxSearchClient.Text.ToLower())
                    || b.MiddleName.ToLower().Contains(TextBoxSearchClient.Text.ToLower()))
                    .ToList();
            }

            var Clients = new List<ClientsHistoryView>();

            foreach (var item in clients)
            {
                var client = new ClientsHistoryView();
                client.ClientName = item.LastName + " " + item.FirstName + " " + item.MiddleName;
                client.ClientID = item.ClientID;
                client.ClientPhone = item.Phone;
                client.ClientDateBirth = item.DayOfBirth.ToShortDateString();

                if (item.HistoryAppointments.Count == 0)
                {
                    client.ClientHistory.Add(new History
                    {
                        Client = client,
                        Date = "Записей нет",
                    });
                }
                else
                {
                    foreach (var history in item.HistoryAppointments)
                    {

                        client.ClientHistory.Add(new History
                        {
                            Client = client,
                            Date = history.Appointment.DateAppointment.ToShortDateString(),
                            DoctorName = history.Appointment.Doctor.LastName,
                            HistoryID = history.HistoryAppointmentID,
                            ServiceName = history.Appointment.Service.ServiceName
                        });
                    }
                }

               

                Clients.Add(client);
            }

            ListViewClients.ItemsSource = null;
            ListViewClients.ItemsSource = Clients;
        }

        #endregion



        private static void DictionaryData()
        {
            keyMonths.Add(1, "Январь");
            keyMonths.Add(2, "Февраль");
            keyMonths.Add(3, "Март");
            keyMonths.Add(4, "Апрель");
            keyMonths.Add(5, "Май");
            keyMonths.Add(6, "Июнь");
            keyMonths.Add(7, "Июль");
            keyMonths.Add(8, "Август");
            keyMonths.Add(9, "Сентябрь");
            keyMonths.Add(10, "Октябрь");
            keyMonths.Add(11, "Ноябрь");
            keyMonths.Add(12, "Декабрь");
        }

  

        private void bttnOpenHistory_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            StackPanel stackPanel = (StackPanel)button.Parent;
            StackPanel mainStackPanel = (StackPanel)stackPanel.Parent;
            StackPanel main1 = (StackPanel)mainStackPanel.Parent;
            StackPanel main2 = (StackPanel)main1.Parent;

            Border border = (Border)(main2.Parent);

            var obj = (ListViewItem)border.Parent;

            

            var stackPanels = main1.Children.OfType<StackPanel>().ToList();
            
            foreach (var st in stackPanels)
            {
                if (st.Name == "stPanHistory")
                {
                    if (st.Visibility == Visibility.Visible)
                    {
                        st.Visibility = Visibility.Collapsed;
                        button.Content = "Показать историю";
                        border.Height = 160;
                    }
                    else
                    {
                        st.Visibility = Visibility.Visible;
                        button.Content = "Скрыть историю";
                        border.Height = 350;
                    }
                }      
            }

        }

        private void TextBoxSearchClient_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ListViewClientsData();
        }

        private void ListViewClients_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var client = ListViewClients.SelectedItem as ClientsHistoryView;


            CreateClientWindow editClient = new CreateClientWindow(client);
            editClient.ShowDialog();

            ListViewClientsData();
        }

        private void ButtonAddClient_Click(object sender, RoutedEventArgs e)
        {
            CreateClientWindow editClient = new CreateClientWindow(null);
            editClient.ShowDialog();

            ListViewClientsData();
        }

        private void ListViewDoctors_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var doctor = ListViewDoctors.SelectedItem as DoctorsView;

            CreateDoctorWindow editDoctor = new CreateDoctorWindow(doctor);
            editDoctor.ShowDialog();

            DoctorsListViewData();
        }

        private void ComboBoxSpecFilterDoctors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DoctorsListViewData();
        }

        private void TextBoxSearchDoctors_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            DoctorsListViewData();
        }
    }
}
