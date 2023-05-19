using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для SuccessMessageWindow.xaml
    /// </summary>
    public partial class InfoMessageWindow : Window
    {
        public string Information { get; set; }
        public InfoMessageWindow(string infoMessage)
        {
            Information = infoMessage;
            InitializeComponent();
            TextBoxInfo.Text = Information;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
