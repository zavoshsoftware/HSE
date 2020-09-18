
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Resources;

namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User : BaseEntity
    {
        public User()
        {
            CompanyUsers = new List<CompanyUser>();
            Anomalies = new List<Anomaly>();
            CreatorAnomalies = new List<Anomaly>();
        }


        [Display(Name = "Password", ResourceType = typeof(Resources.Models.User))]
        [StringLength(150, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string Password { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [StringLength(20, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string Username { get; set; }

        [Display(Name = "FullName", ResourceType = typeof(Resources.Models.User))]
        [StringLength(250, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string FullName { get; set; }

        [Display(Name = "Code", ResourceType = typeof(Resources.Models.User))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public int Code { get; set; }

        [Display(Name = "Email", ResourceType = typeof(Resources.Models.User))]
        public string Email { get; set; }

        [Display(Name = "RoleTitle", ResourceType = typeof(Resources.Models.Role))]
        public Guid RoleId { get; set; }

     
        public virtual Role Role { get; set; }
        public virtual ICollection<CompanyUser> CompanyUsers { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
        [InverseProperty("ResponseUser")]
        public virtual ICollection<Anomaly> Anomalies { get; set; }
        [InverseProperty("CreatorUser")]
        public virtual ICollection<Anomaly> CreatorAnomalies { get; set; }
        public virtual ICollection<UserStage> UserStages { get; set; }
        public virtual ICollection<Accident> Accidents { get; set; }

        [Display(Name="رده شغلی")]
        public Guid? UserJobRateId { get; set; }
        public virtual UserJobRate UserJobRate { get; set; }

        [Display(Name="مدرک تحصیلی")]
        public string Degree { get; set; }

        [Display(Name="رزومه")]
        public string ResumeFileUrl { get; set; }

        internal class configuration : EntityTypeConfiguration<User>
        {
            public configuration()
            {
                HasRequired(p => p.Role).WithMany(j => j.Users).HasForeignKey(p => p.RoleId);
                HasOptional(p => p.UserJobRate).WithMany(j => j.Users).HasForeignKey(p => p.UserJobRateId);
                HasOptional(p => p.Company).WithMany(j => j.Users).HasForeignKey(p => p.CompanyId);
            }
        }

        public Guid? CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}

