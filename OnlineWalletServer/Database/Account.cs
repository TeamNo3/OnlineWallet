using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database
{
    [Table("account")]
    public partial class Account
    {
        [Key]
        [Column("id")]
        [MaxLength(16)]
        public byte[] Id { get; set; }
        [Column("balance", TypeName = "float unsigned")]
        public float Balance { get; set; }
        [Column("isFrozen")]
        public bool IsFrozen { get; set; }
    }
}
