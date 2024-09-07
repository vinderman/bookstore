using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.DAL.Entities
{
    public class Book
    {
        [Key]
        public Guid id { get; set; }

        public string title { get; set; } = String.Empty;

        public string description { get; set; } = String.Empty;
    }
}

