using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.ViewModels.Comment;

namespace Business.Abstract
{
    public interface ICommentService
    {
        Task<bool> AddCommentAsync(CommentCreateVM commentCreateVM);
    }
}
