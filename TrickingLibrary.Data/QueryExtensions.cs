using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrickingLibrary.Models.Abstractions;

namespace TrickingLibrary.Data
{
    public static class QueryExtensions
    {
        public static int LatestVersion<TSource> (this IQueryable<TSource> source, int offset = 0)
            where TSource : VersionedModel =>
            source.Max(x => x.Version) + offset;
    }
}
