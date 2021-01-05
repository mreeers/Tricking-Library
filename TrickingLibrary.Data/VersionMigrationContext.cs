using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrickingLibrary.Models.Abstractions;
using TrickingLibrary.Models.Moderation;

namespace TrickingLibrary.Data
{
    public class VersionMigrationContext
    {
        private readonly AppDbContext _context;

        public VersionMigrationContext(AppDbContext context)
        {
            _context = context;
        }

        public void Migrate(string targetId, int targetVersion, string targetType) 
        {
            var (current, next) = ResolveCurrentAndNextEntities(targetId, targetVersion, targetType);

            if (current != null)
            {
                current.Active = false;
            }

            next.Active = true;
            next.Temporary = false;

            //TODO: toll id's of temporary versions
        }

        private (VersionedModel Current, VersionedModel Next) ResolveCurrentAndNextEntities(string targetId, int targetVersion, string targetType)
        {
            if (targetType == ModerationTypes.Trick)
            {
                var current = _context.Tricks.FirstOrDefault(x => x.Slug == targetId && x.Active);
                var next = _context.Tricks.FirstOrDefault(x => x.Slug == targetId && x.Version == targetVersion);
                return (current, next);
            }
            throw new ArgumentException(nameof(targetType));
        }
    }
}
