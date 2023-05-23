using Microsoft.Win32;
using PrivateDentalClinic.DB;
using PrivateDentalClinic.Views;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PrivateDentalClinic.Windows
{
    /// <summary>
    /// Логика взаимодействия для CreateDoctorWindow.xaml
    /// </summary>
    public partial class CreateDoctorWindow : Window
    {
        private readonly DentalClinicEntities DbContext;
        private string ImagePath { get; set; }
        private Doctor DoctorEdit { get; set; }
        public CreateDoctorWindow(DoctorsView Edit)
        {
            DbContext = DentalClinicEntities.GetContext();
            InitializeComponent();

            ComboBoxSpecializationData();

            if (Edit != null)
            {
                DoctorEdit = DbContext.Doctors.FirstOrDefault(b => b.DoctorID == Edit.DoctorID);
                ButtonCreateDoctor.Content = "Изменить";
                EditDoctorData();
            }


        }


        private void EditDoctorData()
        {
            TextBoxCabinetNumber.Text = DoctorEdit.CabinetNumber;
            TextBoxFirstName.Text = DoctorEdit.FirstName;
            TextBoxLastName.Text = DoctorEdit.LastName;
            TextBoxMiddleName.Text = DoctorEdit.MiddleName;
            TextBoxPhone.Text = DoctorEdit.PersonalPhone;
            TextBoxWorkExp.Text = DoctorEdit.WorkPhone;
            TextBoxWorkPhone.Text = DoctorEdit.WorkPhone;
            ComboBoxSpecialization.Text = DoctorEdit.Specialization.SpecializationName;

            if (!string.IsNullOrEmpty(DoctorEdit.Photo))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(Environment.CurrentDirectory + DoctorEdit.Photo);
                image.EndInit();
                imageDoctor.Source = image;
            }

            
        }

        private void ComboBoxSpecializationData()
        {
            var specialization = DbContext.Specializations.ToList();

            ComboBoxSpecialization.Items.Add("");

            foreach (var item in specialization)
            {
                ComboBoxSpecialization.Items.Add(item.SpecializationName);
            }
        }

        private void bttnSelectPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(dlg.FileName);
                image.EndInit();

                imageDoctor.Source = image;

                ImagePath = dlg.FileName;
            }
        }


        private void ButtonCreateDoctor_Click(object sender, RoutedEventArgs e)
        {
            if (DoctorEdit == null)
            {

                if (!string.IsNullOrEmpty(TextBoxCabinetNumber.Text) &&
                    !string.IsNullOrEmpty(TextBoxFirstName.Text) &&
                    !string.IsNullOrEmpty(TextBoxLastName.Text) &&
                    !string.IsNullOrEmpty(TextBoxMiddleName.Text) &&
                    !string.IsNullOrEmpty(TextBoxPhone.Text) &&
                    !string.IsNullOrEmpty(TextBoxWorkExp.Text) &&
                    !string.IsNullOrEmpty(TextBoxWorkPhone.Text) &&
                    ComboBoxSpecialization.SelectedItem != null &&
                    ComboBoxSpecialization.SelectedIndex != 0)
                {

                    if (imageDoctor.Source == null)
                    {
                        ImagePath = "";
                    }
                    else
                    {
                        Guid guid = Guid.NewGuid();


                        string photoPath = $"\\PhotoDoctors\\{guid}.jpeg";

                        string fullPath = Environment.CurrentDirectory + photoPath;
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.UriSource = new Uri(ImagePath);
                        image.EndInit();

                        BitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(image));

                        using (var fileStream = new System.IO.FileStream(fullPath, System.IO.FileMode.Create))
                        {
                            encoder.Save(fileStream);
                        }

                        ImagePath = photoPath;
                    }


                    var doctor = new Doctor
                    {
                        FirstName = TextBoxFirstName.Text,
                        LastName = TextBoxLastName.Text,
                        MiddleName = TextBoxMiddleName.Text,
                        WorkExperience = TextBoxWorkExp.Text,
                        WorkPhone = TextBoxWorkPhone.Text,
                        CabinetNumber = TextBoxCabinetNumber.Text,
                        PersonalPhone = TextBoxPhone.Text,
                        Photo = ImagePath,
                        Appointments = new List<Appointment>(),
                        WorkSchedules = new List<WorkSchedule>(),
                        Specialization = DbContext.Specializations.FirstOrDefault(b => b.SpecializationName == ComboBoxSpecialization.Text),
                    };

                    DbContext.Doctors.Add(doctor);
                    DbContext.SaveChanges();

                    InfoMessageWindow info = new InfoMessageWindow("Добавление прошло успешно");
                    info.ShowDialog();

                }
                else
                {
                    InfoMessageWindow info = new InfoMessageWindow("Заполните все данные");
                    info.ShowDialog();
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(TextBoxCabinetNumber.Text) &&
                   !string.IsNullOrEmpty(TextBoxFirstName.Text) &&
                   !string.IsNullOrEmpty(TextBoxLastName.Text) &&
                   !string.IsNullOrEmpty(TextBoxMiddleName.Text) &&
                   !string.IsNullOrEmpty(TextBoxPhone.Text) &&
                   !string.IsNullOrEmpty(TextBoxWorkExp.Text) &&
                   !string.IsNullOrEmpty(TextBoxWorkPhone.Text) &&
                   ComboBoxSpecialization.SelectedItem != null &&
                   ComboBoxSpecialization.SelectedIndex != 0)
                {
                    if (imageDoctor.Source == null)
                    {
                        ImagePath = "";
                    }
                    else
                    {
                        Guid guid = Guid.NewGuid();


                        string photoPath = $"\\PhotoDoctors\\{guid}.jpeg";

                        string fullPath = Environment.CurrentDirectory + photoPath;
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.UriSource = new Uri(ImagePath);
                        image.EndInit();

                        BitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(image));

                        using (var fileStream = new System.IO.FileStream(fullPath, System.IO.FileMode.Create))
                        {
                            encoder.Save(fileStream);
                        }

                        ImagePath = photoPath;
                    }


                    DoctorEdit.FirstName = TextBoxFirstName.Text;
                    DoctorEdit.LastName = TextBoxLastName.Text;
                    DoctorEdit.MiddleName = TextBoxMiddleName.Text;
                    DoctorEdit.WorkExperience = TextBoxWorkExp.Text;
                    DoctorEdit.WorkPhone = TextBoxWorkPhone.Text;
                    DoctorEdit.CabinetNumber = TextBoxCabinetNumber.Text;
                    DoctorEdit.PersonalPhone = TextBoxPhone.Text;
                    DoctorEdit.Photo = ImagePath;
                    DoctorEdit.Specialization = DbContext.Specializations.FirstOrDefault(b => b.SpecializationName == ComboBoxSpecialization.Text);


                    DbContext.Doctors.AddOrUpdate(DoctorEdit);
                    DbContext.SaveChanges();

                    InfoMessageWindow info = new InfoMessageWindow("Изменение прошло успешно");
                    info.ShowDialog();

                }
                else
                {
                    InfoMessageWindow info = new InfoMessageWindow("Заполните все данные");
                    info.ShowDialog();
                }
            }

        }

        private void bttnDeletePhoto_Click(object sender, RoutedEventArgs e)
        {
            imageDoctor.Source = null;
        }
    }
}
