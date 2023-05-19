using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateDentalClinic.Views
{
    public class DoctorsAppoinmentsView
    {
        public int DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string Specialization { get; set; }
        public string WorkExp { get; set; }
        public string WorkPhone { get; set; }
        public string PersonalPhone { get; set; }
        public string Photo { get; set; }
        public string CabinetNumber { get; set; }
        public List<AppoinmentsView> AppoinmentsOut { get; set; }
    }

    public class AppoinmentsView
    {
        public int AppointmentID { get; set; }
        public DoctorsAppoinmentsView Doctor { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
        public string Date { get; set; }
        public string TimeBegin { get; set; }
        public string TimeEnd { get; set; }
        public string Status { get; set; }
    }
}
