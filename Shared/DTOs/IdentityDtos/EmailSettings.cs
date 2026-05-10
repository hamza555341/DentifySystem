using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Services
{
    public class EmailSettings
    {
        public int Port { get; set; }
        public string Host { get; set; }= null!;
        public string Password { get; set; }= null!;
        public string SenderName { get; set; }= null!;
        public string SenderEmail { get; set; }= null!;




    }
}
