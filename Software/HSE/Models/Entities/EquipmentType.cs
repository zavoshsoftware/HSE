using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Models
{
    public class EquipmentType : BaseEntity
    {
        [Display(Name="دسته بندی تجهیزات و ماشین آلات")]
        public string Title { get; set; }

        public virtual ICollection<Equipment> Equipments { get; set; }

      
    }
}