using PrivateDentalClinic.DB;
using PrivateDentalClinic.Views;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PrivateDentalClinic.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditDayWorkScheduleWindow.xaml
    /// </summary>
    public partial class EditDayWorkScheduleWindow : Window
    {
        private readonly DentalClinicEntities DbContext;
        private Days day { get; set; }
        public EditDayWorkScheduleWindow(Days day)
        {
            DbContext = DentalClinicEntities.GetContext();

            this.day = day;

            InitializeComponent();

            DatePickerWorkDay.SelectedDate = Convert.ToDateTime(day.Date);
            TextBoxTimeBeginWorkDay.Text = day.BeginTimeDay;
            TextBoxTimeEndWorkDay.Text = day.EndTimeDay;
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DatePickerWorkDay.SelectedDate!=null && !string.IsNullOrEmpty(TextBoxTimeBeginWorkDay.Text) && !string.IsNullOrEmpty(TextBoxTimeEndWorkDay.Text))
            {
                var doctorWorkSchedule = DbContext.Doctors.Include(n=>n.WorkSchedules).FirstOrDefault(b=>b.DoctorID == day.DoctorID);

                var DayWorkSchedule = doctorWorkSchedule.WorkSchedules.FirstOrDefault(b=>b.Date == Convert.ToDateTime(day.Date));

                try
                {
                    int hourBegin = Convert.ToInt32(TextBoxTimeBeginWorkDay.Text.Split(':')[0]);
                    int minBegin = Convert.ToInt32(TextBoxTimeBeginWorkDay.Text.Split(':')[1]);

                    int hourEnd = Convert.ToInt32(TextBoxTimeEndWorkDay.Text.Split(':')[0]);
                    int minEnd = Convert.ToInt32(TextBoxTimeEndWorkDay.Text.Split(':')[1]);


                    DayWorkSchedule.Date = (DateTime)DatePickerWorkDay.SelectedDate;
                    DayWorkSchedule.BeginWorkDay = new TimeSpan(hourBegin, minBegin, 0);
                    DayWorkSchedule.EndWorkDay = new TimeSpan(hourEnd, minEnd, 0);


                    DbContext.WorkSchedules.AddOrUpdate(DayWorkSchedule);
                    DbContext.SaveChanges();


                    InfoMessageWindow infomessageWindow = new InfoMessageWindow("Изменения прошли успешно");
                    infomessageWindow.ShowDialog();
                }
                catch (Exception)
                {

                    InfoMessageWindow infomessageWindow = new InfoMessageWindow("Укажите правильный формат времени");
                    infomessageWindow.ShowDialog();
                   
                    return;
                }  
            }
            else
            {
                InfoMessageWindow infomessageWindow = new InfoMessageWindow("Для изменения заполните все поля");
                infomessageWindow.ShowDialog();
                return;
            }
        }

        private void TextBoxTimeEndWorkDay_TextChanged(object sender, TextChangedEventArgs e)
        {
          
        }

        private void TextBoxTimeBeginWorkDay_TextChanged(object sender, TextChangedEventArgs e)
        {
          
        }
    }
}
