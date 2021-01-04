using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrickingLibrary.API.Settings;

namespace TrickingLibrary.API.BackgroundServices.VideoEditing
{
    public static class RegisterService
    {
        public static IServiceCollection AddFileManager(this IServiceCollection services, IConfiguration configuration)
        {
            var settingsSection = configuration.GetSection(nameof(FileSettings));
            var settings = settingsSection.Get<FileSettings>();
            services.Configure<FileSettings>(settingsSection);

            if (settings.Provider.Equals(TrickingLibraryConstants.Files.Providers.Local))
            {
                services.AddSingleton<IFileManager, FileManagerLocal>();
            }
            else if (settings.Provider.Equals(TrickingLibraryConstants.Files.Providers.S3))
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new Exception($"Invalid File Manager Provider: {settings.Provider}");
            }

            services.AddSingleton<IFileManager, FileManagerLocal>();
            return services;
        }
    }
}
