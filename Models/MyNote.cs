using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    public class MyNote
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
    }
}
