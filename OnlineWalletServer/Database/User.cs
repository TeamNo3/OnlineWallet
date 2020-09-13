using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database
{
    [Table("user")]
    public partial class User
    {
        [Key]
        [Column("id", TypeName = "int(11)")]
        public int Id { get; set; }
        [Required]
        [Column("username", TypeName = "varchar(20)")]
        public string Username { get; set; }
        [Required]
        [Column("firstname", TypeName = "varchar(20)")]
        public string Firstname { get; set; }
        [Column("middlename", TypeName = "varchar(20)")]
        public string Middlename { get; set; }
        [Required]
        [Column("lastname", TypeName = "varchar(30)")]
        public string Lastname { get; set; }
        [Required]
        [Column("email", TypeName = "varchar(100)")]
        public string Email { get; set; }
        [Column("isConfirmed")]
        public bool IsConfirmed { get; set; }
        [Required]
        [Column("password", TypeName = "varchar(45)")]
        public string Password { get; set; }
        [Required]
        [Column("account")]
        [MaxLength(16)]
        public byte[] Account { get; set; }
    }
}
