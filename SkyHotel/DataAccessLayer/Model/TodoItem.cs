using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class TodoItem
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public bool IsCompleted { get; set; }
    }
}
