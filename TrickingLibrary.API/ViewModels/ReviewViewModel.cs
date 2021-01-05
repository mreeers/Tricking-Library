using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TrickingLibrary.Models.Moderation;

namespace TrickingLibrary.API.ViewModels
{
    public static class ReviewViewModel
    {
        public readonly static Func<Review, object> Create = Projection.Compile();

        public static Expression<Func<Review, object>> Projection =>
            review => new
            {
                review.Id,
                review.ModerationItemId,
                review.Comment,
                review.Status
            };
    }
}
