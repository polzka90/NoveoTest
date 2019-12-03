using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Api
{
    public class WorkflowDTO
    {
        [Key]
        public Guid UUID { get; set; }
        public StatusWF Status { get; set; }
        [Column(TypeName = "jsonb")]
        public string Data { get; set; }
        public List<String> Steps { get; set; }
    }

    public enum StatusWF
    {
        Inserted = 0,
        Consumed = 1
    }
}
