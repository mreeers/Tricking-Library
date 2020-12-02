using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TrickingLibrary.Models;

namespace TrickingLibrary.API.ViewModels
{
    public static class TrickViewModels
    {
        public readonly static Func<Trick, object> Create = Projection.Compile();

        public static Expression<Func<Trick, object>> Projection =>
            trick => new
            {
                trick.Id,
                trick.Name,
                trick.Description,
                trick.Difficulty,
                Categories = trick.TrickCategories.Select(x => x.CategoryId),
                Prerequisites = trick.Prerequisite.Select(x => x.PrerequisiteId),
                Progressions = trick.Progressions.Select(x => x.ProgressionId),
            };
    }
}
