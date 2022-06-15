using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpMeNow.Shared
{
    public class Ticket
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime Created { get; set; } = DateTime.Now;

        public bool isOpen { get; set; } = true;

        public string? Solution { get; set; } = string.Empty;


        public int? UserId { get; set; }

        public string? Email { get; set; }

        public User? User { get; set; }


        public int Prio { get; set; } = 5;






    }
}
