using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class MyTask
    {
        public int Id { get; set; }
        public string Content {  get; set; }
        public DateTime Deadline { get; set; }
        public int Priority { get; set; }
        public bool IsCompleted { get; set; }
    }
}
