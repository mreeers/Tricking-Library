using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using TrickingLibrary.Data;

namespace TrickingLibrary.API.BackgroundServices.VideoEditing
{
    public class VideosEditingBackgroundService : BackgroundService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<VideosEditingBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly VideoManager _videoManager;
        private readonly ChannelReader<EditVideoMessage> _channelReader;

        public VideosEditingBackgroundService(IWebHostEnvironment env,
            Channel<EditVideoMessage> channel,
            ILogger<VideosEditingBackgroundService> logger,
            IServiceProvider serviceProvider,
            VideoManager videoManager)
        {
            _env = env;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _videoManager = videoManager;
            _channelReader = channel.Reader;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (await _channelReader.WaitToReadAsync(stoppingToken))
            {
                var message = await _channelReader.ReadAsync(stoppingToken);

                try
                {
                    var inputPath = _videoManager.TemporarySavePath(message.Input);
                    var outputName = _videoManager.GenerateConvertedFileName();
                    var outputPath = _videoManager.TemporarySavePath(outputName);

                    var startInfo = new ProcessStartInfo
                    {
                        FileName = Path.Combine(_env.ContentRootPath, "ffmpeg", "ffmpeg.exe"),
                        Arguments = $"-y -i {inputPath} -an -vf scale=540x380 {outputPath}",
                        WorkingDirectory = _videoManager.WorkingDirectory,
                        CreateNoWindow = true,
                        UseShellExecute = false,
                    };

                    using (var process = new Process { StartInfo = startInfo })
                    {
                        process.Start();
                        process.WaitForExit();
                    }

                    if (!_videoManager.TemporaryVideoExists(outputName))
                    {
                        throw new Exception("FFMPEG failed to generate converted video");
                    }

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                        var subbmission = ctx.Submissions.FirstOrDefault(x => x.Id.Equals(message.SubmmisionId));

                        subbmission.Video = outputName;
                        subbmission.VideoProcessed = true;

                        await ctx.SaveChangesAsync(stoppingToken);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Video Processing Failed for {0}", message.Input);
                }
                finally
                {
                    _videoManager.DeleteTemporaryVideo(message.Input);
                }
            }
        }
    }
}
