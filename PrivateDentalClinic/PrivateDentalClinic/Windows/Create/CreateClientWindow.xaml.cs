using PrivateDentalClinic.DB;
using PrivateDentalClinic.Views;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;

namespace PrivateDentalClinic.Windows.Create
{
    /// <summary>
    /// Логика взаимодействия для CreateClientWindow.xaml
    /// </summary>
    public partial class CreateClientWindow : Window
    {
        private DentalClinicEntities DbContext;

        private Client Client { get; set; }
        public CreateClientWindow(ClientsHistoryView IsEdit)
        {
            DbContext = DentalClinicEntities.GetContext();
            InitializeComponent();

            

            if (IsEdit != null)
            {
                Client = DbContext.Clients.FirstOrDefault(b => b.ClientID == IsEdit.ClientID);
                ButtonCreateClient.Content = "Изменить";
                EditClientData();
            }

        }

        private void EditClientData()
        {
            TextBoxClientLastName.Text = Client.LastName;
            TextBoxClientFirstName.Text = Client.FirstName;
            TextBoxClientMiddleName.Text = Client.MiddleName;
            TextBoxClientPhone.Text = Client.Phone;
            TextBoxClientDateOfBirth.Text = Client.DayOfBirth.ToShortDateString();
        }

        private void ButtonCreateClient_Click(object sender, RoutedEventArgs e)
        {
            if (Client == null)
            {
                if (string.IsNullOrEmpty(TextBoxClientLastName.Text)
                   || string.IsNullOrEmpty(TextBoxClientFirstName.Text)
                   || string.IsNullOrEmpty(TextBoxClientMiddleName.Text)
                   || string.IsNullOrEmpty(TextBoxClientPhone.Text)
                   || string.IsNullOrEmpty(TextBoxClientDateOfBirth.Text))
                {
                    InfoMessageWindow infoMessage = new InfoMessageWindow("Заполните все данные");
                    infoMessage.ShowDialog();
                }
                else
                {

                    var newClient = new Client
                    {
                        LastName = TextBoxClientLastName.Text,
                        FirstName = TextBoxClientFirstName.Text,
                        MiddleName = TextBoxClientMiddleName.Text,
                        Phone = TextBoxClientPhone.Text,
                        DayOfBirth = Convert.ToDateTime(TextBoxClientDateOfBirth.Text),
                        Appointments = new List<Appointment>(),
                        HistoryAppointments = new List<HistoryAppointment>()
                    };

                    DbContext.Clients.Add(newClient);
                    DbContext.SaveChanges();


                    InfoMessageWindow info = new InfoMessageWindow("Регистрация прошла успешно");
                    info.ShowDialog();
                }
            }
            else
            {
                if (string.IsNullOrEmpty(TextBoxClientLastName.Text)
                  || string.IsNullOrEmpty(TextBoxClientFirstName.Text)
                  || string.IsNullOrEmpty(TextBoxClientMiddleName.Text)
                  || string.IsNullOrEmpty(TextBoxClientPhone.Text)
                  || string.IsNullOrEmpty(TextBoxClientDateOfBirth.Text))
                {
                    InfoMessageWindow infoMessage = new InfoMessageWindow("Заполните все данные");
                    infoMessage.ShowDialog();
                }
                else
                {
                    Client.LastName = TextBoxClientLastName.Text;
                    Client.FirstName = TextBoxClientFirstName.Text;
                    Client.MiddleName = TextBoxClientMiddleName.Text;
                    Client.Phone = TextBoxClientPhone.Text;
                    Client.DayOfBirth = Convert.ToDateTime(TextBoxClientDateOfBirth.Text);

                    DbContext.Clients.AddOrUpdate(Client);
                    DbContext.SaveChanges();

                    InfoMessageWindow info = new InfoMessageWindow("Изменение прошло успешно");
                    info.ShowDialog();
                }
            }


        }
    }
}
