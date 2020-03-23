using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace API_16_3_2020.Models
{
    public class RequestViewModel
    {
        public string ID { get; set; }     
        [Required]
        [MaxLength(256)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(50)]
        public string EquipmentType { get; set; }
        public string DateRequest { get; set; }     
        [MaxLength(10)]
        public string Status { get; set; }   
    }
}