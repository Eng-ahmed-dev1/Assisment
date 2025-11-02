using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Assisment_Ahmed_Alaa.Models
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentID { get; set; }
        [Required]
        public int DoctorID { get; set; }
        [ForeignKey(nameof(DoctorID))]
        public User Doctor { get; set; }
        public int Pat_Id { get; set; }
        [ForeignKey(nameof(Pat_Id))]
        public Patient patient { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        [Required]
        [StringLength(500)]
        public string Reason { get; set; }
        [Required]
        [StringLength(25)]
        public string Status { get; set; }
    }
}
