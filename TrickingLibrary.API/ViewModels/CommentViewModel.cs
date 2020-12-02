using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TrickingLibrary.Models;

namespace TrickingLibrary.API.ViewModels
{
    public static class CommentViewModel
    {
        public readonly static Func<Comment, object> Create = Projection.Compile();

        public static Expression<Func<Comment, object>> Projection =>
            comment => new
            {
                comment.Id,
                comment.ParentId,
                comment.Content,
                comment.HtmlContent
            };
    }
}
