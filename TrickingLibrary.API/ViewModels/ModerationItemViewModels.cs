using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TrickingLibrary.Models;
using TrickingLibrary.Models.Moderation;

namespace TrickingLibrary.API.ViewModels
{
    public static class ModerationItemViewModels
    {
        public readonly static Func<ModerationItem, object> Create = Projection.Compile();

        public static Expression<Func<ModerationItem, object>> Projection =>
            modItem => new
            {
                modItem.Id,
                modItem.Current,
                modItem.Target,
                modItem.Type,
                Comments = modItem.Comments.AsQueryable().Select(CommentViewModel.Projection).ToList(),
                Reviews = modItem.Reviews.AsQueryable().Select(ReviewViewModel.Projection).ToList()
            };
    }
}
