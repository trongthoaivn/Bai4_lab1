namespace Bai4_lab1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Course")]
    public partial class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(128)]
        public string LecturerId { get; set; }

        [StringLength(255)]
        public string Place { get; set; }

        public DateTime? DateTime { get; set; }

        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public List<Category> ListCategory = new List<Category>();
    }
}
