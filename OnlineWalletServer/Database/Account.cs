using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database
{
    [Table("account")]
    public partial class Account
    {
        public Account()
        {
            TransactionFromNavigation = new HashSet<Transaction>();
            TransactionToNavigation = new HashSet<Transaction>();
        }

        [Key]
        [Column("id", TypeName = "int(16) unsigned")]
        public uint Id { get; set; }
        [Column("balance", TypeName = "float unsigned")]
        public float Balance { get; set; }
        [Column("isFrozen")]
        public bool IsFrozen { get; set; }

        [InverseProperty("AccountNavigation")]
        public virtual User User { get; set; }
        [InverseProperty(nameof(Transaction.FromNavigation))]
        public virtual ICollection<Transaction> TransactionFromNavigation { get; set; }
        [InverseProperty(nameof(Transaction.ToNavigation))]
        public virtual ICollection<Transaction> TransactionToNavigation { get; set; }
    }
}
