using PrivateDentalClinic.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace PrivateDentalClinic.Windows
{
    /// <summary>
    /// Логика взаимодействия для CreatingWorkScheduleWindow.xaml
    /// </summary>
    public partial class CreatingWorkScheduleWindow : Window
    {
        private readonly DentalClinicEntities
            DbContext;
        public CreatingWorkScheduleWindow()
        {
            InitializeComponent();
            DbContext = DentalClinicEntities.GetContext();

            DatePickerBenginRange.DisplayDateStart = DateTime.Now.AddDays(1);
            DatePickerEndRange.DisplayDateStart = DateTime.Now.AddDays(1);

            DataComboBoxDoctors();
        }

        private void DataComboBoxDoctors()
        {
            var doctors = DbContext.Doctors.ToList();

            ComboBoxDoctors.Items.Add("");
            
            foreach (var doctor in doctors)
            {
                ComboBoxDoctors.Items.Add($"{doctor.LastName} {doctor.FirstName} {doctor.MiddleName}");
            }
            ComboBoxDoctors.SelectedIndex = 0;

        }
        private void DatePickerEndRange_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DatePickerBenginRange.SelectedDate != null && DatePickerEndRange.SelectedDate !=null && (ComboBoxDoctors.SelectedItem !=null && ComboBoxDoctors.SelectedIndex != 0))
            {
                DatesScheduleWork();
            }
        }
        private void DatePickerBenginRange_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DatePickerBenginRange.SelectedDate != null && DatePickerEndRange.SelectedDate != null && (ComboBoxDoctors.SelectedItem != null && ComboBoxDoctors.SelectedIndex != 0))
            {
                DatesScheduleWork();
            }
        }
        private void ComboBoxDoctors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DatePickerBenginRange.SelectedDate != null && DatePickerEndRange.SelectedDate != null && (ComboBoxDoctors.SelectedItem != null && ComboBoxDoctors.SelectedIndex != 0))
            {
                DatesScheduleWork();
            }
        }

        private void DatesScheduleWork()
        {
            var DatesWorkSchedule = new List<DatesScheduleWork>();

            for (DateTime date = (DateTime)DatePickerBenginRange.SelectedDate; date <= (DateTime)DatePickerEndRange.SelectedDate; date = date.AddDays(1))
            {
                DatesWorkSchedule.Add(
                    new DatesScheduleWork
                    {
                        Dates = date.ToShortDateString(),
                        TimeBegin = "00:00",
                        TimeEnd = "00:00",
                    }
                );
            }

            DataGridScheduleWork.ItemsSource = DatesWorkSchedule;
        }


        private void CreateWorkSchedule_Click(object sender, RoutedEventArgs e)
        {

            if (DatePickerBenginRange.SelectedDate == null || DatePickerEndRange.SelectedDate == null || (ComboBoxDoctors.SelectedItem == null || ComboBoxDoctors.SelectedIndex == 0))
            {
                InfoMessageWindow infoMessageWindow = new InfoMessageWindow("Заполните все поля");
                infoMessageWindow.ShowDialog();

                return;
            }


            string[] LfmDoctor = ComboBoxDoctors.Text.Split(' ');
            string lastName = LfmDoctor[0];
            string firstName = LfmDoctor[1];
            string middleName = LfmDoctor[2];

            var doctor = DbContext.Doctors.Where(b => b.LastName == lastName && b.FirstName == firstName && b.MiddleName == middleName).FirstOrDefault();

            var workScheduleDataGrid = (List<DatesScheduleWork>)DataGridScheduleWork.ItemsSource;
            var workSchedule = DbContext.WorkSchedules.Where(b=>b.DoctorID == doctor.DoctorID).ToList();

            var newWorkSchedules = new List<WorkSchedule>();

            foreach (var day in workScheduleDataGrid)
            {
                if (workSchedule.Any(b=>b.Date == Convert.ToDateTime(day.Dates)))
                {

                    InfoMessageWindow infoWindow = new InfoMessageWindow($"На {day.Dates} уже составлен график работы.\nПроверьте выбранные данные.");
                    infoWindow.ShowDialog();

                    DatePickerBenginRange.SelectedDate = null;
                    DatePickerEndRange.SelectedDate = null;
                    ComboBoxDoctors.SelectedIndex = 0;
                    DataGridScheduleWork.ItemsSource = null;

                    return;
                }

                newWorkSchedules.Add(
                    new WorkSchedule
                    {
                        Doctor = doctor,
                        Date = Convert.ToDateTime(day.Dates),
                        BeginWorkDay = TimeSpan.Parse(day.TimeBegin),
                        EndWorkDay = TimeSpan.Parse(day.TimeEnd),
                        DoctorID = doctor.DoctorID
                    });
            }

            DbContext.WorkSchedules.AddRange(newWorkSchedules);
            DbContext.SaveChangesAsync();

            InfoMessageWindow MessageSuccessWindow = new InfoMessageWindow("График работы успешно создан.");
            MessageSuccessWindow.ShowDialog();
        }
    }


    public class DatesScheduleWork
    {
        public string Dates { get; set; }
        public string TimeBegin { get; set; }
        public string TimeEnd { get; set; }
    }
}
