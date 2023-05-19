using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateDentalClinic.Views
{
    public class TalonsView
    {
        public string TimeBeginAppointment { get; set; }

        public TalonsView(string time)
        {
            TimeBeginAppointment = time;
        }
    }
}
