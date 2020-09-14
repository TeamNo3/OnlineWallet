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
        [Column("id", TypeName = "int(11)")]
        public int Id { get; set; }
        [Column("amount", TypeName = "float unsigned")]
        public float Amount { get; set; }
        [Required]
        [Column("_from", TypeName = "varchar(36)")]
        public string From { get; set; }
        [Required]
        [Column("_to", TypeName = "varchar(36)")]
        public string To { get; set; }
        [Column("_datetime", TypeName = "datetime")]
        public DateTime Datetime { get; set; }

        [ForeignKey(nameof(From))]
        [InverseProperty(nameof(Account.TransactionFromNavigation))]
        public virtual Account FromNavigation { get; set; }
        [ForeignKey(nameof(To))]
        [InverseProperty(nameof(Account.TransactionToNavigation))]
        public virtual Account ToNavigation { get; set; }
    }
}
