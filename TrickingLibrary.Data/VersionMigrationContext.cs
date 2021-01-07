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

        public void Migrate(ModerationItem modItem) 
        {
            var source = GetSourse(modItem.Type);

            var current = source.FirstOrDefault(x => x.Id == modItem.Current);
            var target = source.FirstOrDefault(x => x.Id == modItem.Target);

            if (target == null)
            {
                throw new InvalidOperationException("Target not found");
            }

            if (current != null)
            {
                if (target.Version - current.Version <= 0)
                {
                    throw new InvalidVersionException($"Current Version is {current.Version}, Target version is {target.Version}, for {modItem.Type}");
                }

                current.Active = false;

                var outdatedModerationItems = _context.ModerationItems
                    .Where(x => !x.Deleted && x.Type == modItem.Type && x.Id != modItem.Id)
                    .ToList();

                foreach(var outdatedModerationItem in outdatedModerationItems)
                {
                    outdatedModerationItem.Current = target.Id;
                }
            }

            target.Active = true;

        }

        private IQueryable<VersionedModel> GetSourse(string type)
        {
            if(type == ModerationTypes.Trick)
            {
                return _context.Tricks;
            }
            throw new ArgumentException(nameof(type));
        }

        public class InvalidVersionException : Exception
        {
            public InvalidVersionException(string message) : base(message)
            {

            }
        }
    }
}
