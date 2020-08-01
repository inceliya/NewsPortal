using NewsPortal.BLL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsPortal.ViewModels
{
    public class NewsViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
            ErrorMessageResourceName = "TitleRequired")]
        [StringLength(256)]
        public string Title { get; set; }

        //[RegularExpression(@"[^<[^>]*>]", ErrorMessage = "Error text")]
        [AllowHtml]
        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
            ErrorMessageResourceName = "DescriptionRequired")]
        public string Description { get; set; }

        public string Image { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
                  ErrorMessageResourceName = "PublicationDateRequired")]
        [DataType(DataType.Date)]
        [Display(Name = "Creation Date")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}")]
        public DateTime PublicationDate { get; set; }

        public bool Visibility { get; set; }

        private ICollection<CommentViewModel> _comments;

        public ICollection<CommentViewModel> Comments
        {
            get
            {
                return _comments;
            }
            set
            {
                _comments = value;
            }
        }

        public NewsViewModel() { }

        public NewsViewModel(NewsItem news, ICollection<Comment> comments)
        {
            Id = news.Id;
            Title = news.Title;
            Description = news.Description;
            PublicationDate = news.PublicationDate;
            Image = news.Image;
            Visibility = news.Visibility;

            var commentsViewModel = new List<CommentViewModel>();
            foreach(var comment in comments)
            {
                var commentViewModel = new CommentViewModel(comment);
                commentsViewModel.Add(commentViewModel);
            }

            Comments = commentsViewModel;
        }
    }
}