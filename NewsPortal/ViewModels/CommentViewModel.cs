using NewsPortal.BLL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsPortal.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public int NewsId { get; set; }

        [Required]
        [MaxLength(256)]
        public string Author { get; set; }

        [Required]
        [MaxLength(5000)]
        public string Text { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Creation Date")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}")]
        public DateTime CreationDate { get; set; }
        public int TimeZone { get; set; }
        public CommentViewModel() { }

        public CommentViewModel(Comment comment)
        {
            Id = comment.Id;
            NewsId = comment.NewsId;
            Author = comment.Author;
            Text = comment.Text;
            CreationDate = comment.CreationDate;
        }
    }
}