using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpMeNow.Shared
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public string Role { get; set; } = "User";

        public List<Ticket>? Tickets { get; set; }   

      
    }
}
