using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.ViewModels
{
    // Class ============= || Get - User-Data - ViewModel ||==========================================
    public class GetUserDataViewModel
    {
        // Props
        public string Email { get; set; } = "";
        public string Firstname { get; set; } = "";
        public string Lastname { get; set; } = "";
        public int Age { get; set; }
        public string Phonenumber { get; set; } = "";


        // || Constructor || ===================================================================
        public GetUserDataViewModel()
        {
        }
    }
}
