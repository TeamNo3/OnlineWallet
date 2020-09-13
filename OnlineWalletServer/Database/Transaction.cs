using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database
{
    [Table("transaction")]
    public partial class Transaction
    {
        [Key]
        [Column("id", TypeName = "int(16)")]
        public int Id { get; set; }
        [Column("amount", TypeName = "float unsigned")]
        public float Amount { get; set; }
        [Required]
        [Column("_from")]
        [MaxLength(16)]
        public byte[] From { get; set; }
        [Required]
        [Column("_to")]
        [MaxLength(16)]
        public byte[] To { get; set; }
        [Column("_datetime", TypeName = "datetime")]
        public DateTime Datetime { get; set; }
    }
}
