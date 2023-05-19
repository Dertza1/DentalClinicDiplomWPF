using PrivateDentalClinic.Views;
using System.Windows;

namespace PrivateDentalClinic.Windows
{
    /// <summary>
    /// Логика взаимодействия для ModalQuestionWorkSchedule.xaml
    /// </summary>
    public partial class ModalQuestionWorkSchedule : Window
    {
        public Days day {  get; set; }
        public ModalQuestionWorkSchedule(Days day)
        {
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

        }
    }
}
