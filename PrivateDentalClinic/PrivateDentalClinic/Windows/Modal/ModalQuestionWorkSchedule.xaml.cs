using PrivateDentalClinic.DB;
using PrivateDentalClinic.Views;
using System;
using System.Linq;
using System.Windows;

namespace PrivateDentalClinic.Windows
{
    /// <summary>
    /// Логика взаимодействия для ModalQuestionWorkSchedule.xaml
    /// </summary>
    public partial class ModalQuestionWorkSchedule : Window
    {

        private readonly DentalClinicEntities DbContext;
        public Days day {  get; set; }
        public ModalQuestionWorkSchedule(Days day)
        {
            DbContext = DentalClinicEntities.GetContext();

            this.day = day;
            InitializeComponent();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonEditDayWorkSchedule_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            EditDayWorkScheduleWindow editDayWork = new EditDayWorkScheduleWindow(day);

            editDayWork.ShowDialog();
        }

        private void ButtonDeleteDayWorkSchedule_Click(object sender, RoutedEventArgs e)
        {

            var date = Convert.ToDateTime(day.Date);
            var dayWorkSchedule = DbContext.WorkSchedules.FirstOrDefault(x => x.Date == date && x.DoctorID == day.DoctorID);

            if (dayWorkSchedule != null)
            {
                DbContext.WorkSchedules.Remove(dayWorkSchedule);
                DbContext.SaveChanges();

                MessageBox.Show("День удален из графика");

                this.Close();
            }
        }
    }
}
