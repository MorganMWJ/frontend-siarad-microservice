using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    [Table("group")]
    public class Group
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public virtual int Id { get; set; }

        [Required]
        [Column("name")]
        public virtual string Name { get; set; }

        [Column("description")]
        public virtual string Description { get; set; }

        [Required]
        [Column("module_id")]
        public virtual int ModuleId { get; set; }

        [Required]
        [Column("is_private")]
        public virtual bool IsPrivate { get; set; }

        [Column("uid_1")]
        public virtual string Uid1 { get; set; }

        [Column("uid_2")]
        public virtual string Uid2 { get; set; }

        [NotMapped]
        public virtual ModuleViewModel Module { get; set; }

    }
}
