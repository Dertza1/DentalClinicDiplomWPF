using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PrivateDentalClinic.Views
{
    public class DoctorsView
    {
        public int DoctorID { get; set; }
        public string Photo { get; set; }
        public string DoctorName { get; set; }
        public string Specialization { get; set; }
        public string WorkExp { get; set; }
        public string WorkPhone { get; set; }
        public string PersonalPhone { get; set; }
        public string CabinetNumber { get; set; }
    }
}
