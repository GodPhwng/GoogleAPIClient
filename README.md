# GoogleAPIClient

This software includes the work that is distributed in the Apache License 2.0

## How to use

**Example** VideosService.cs
```C#
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using GoogleAPIClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleAPIClientSample
{
    public class VideosService : GoogleAPIClient<YouTubeService>
    {
        public VideosService(string keyJsonPath) : base(keyJsonPath, new string[] { YouTubeService.Scope.Youtube })
        {
        }

        protected override YouTubeService CreateService(ICredential credential)
        {
            return new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "MyApp"
            });
        }

        public async Task<IList<PlaylistItem>> GetPlayListAsync(string playListId)
        {
            // basic info
            var request = this.Serive.PlaylistItems.List("snippet");
            request.PlaylistId = playListId;
            request.MaxResults = 50;

            var responce = await request.ExecuteAsync();

            return responce.Items;
        }
    }
}
```

**Example** CalendarService.cs
```C#
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using GoogleAPIClient;
using System;
using System.Threading.Tasks;

namespace GoogleAPIClientSample
{
    public class CalendarService : GoogleAPIClient<Google.Apis.Calendar.v3.CalendarService>
    {
        private const string APP_NAME = "Google Calendar API .NET";

        public CalendarService(string keyJsonPath) : base(keyJsonPath, new string[] { Google.Apis.Calendar.v3.CalendarService.Scope.Calendar })
        {
        }

        protected override Google.Apis.Calendar.v3.CalendarService CreateService(ICredential credential)
        {
            return new Google.Apis.Calendar.v3.CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = APP_NAME
            });
        }

        /// <summary>
        /// read events
        /// </summary>
        /// <param name="calendarId">calender ID</param>
        public async Task<Events> ReadEventsAsync(string calendarId)
        {
            var request = new EventsResource.ListRequest(this.Serive, calendarId);
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.
            return await request.ExecuteAsync();
        }
    }
}
```

**Example** Program.cs
```C#
using System;
using System.Threading.Tasks;

namespace GoogleAPIClientSample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine($"★★★★★★★★YoutubeSample★★★★★★★★★★★★★★★");
                await YoutubeSample();
                Console.WriteLine($"★★★★★★★★YoutubeSample★★★★★★★★★★★★★★★");

                Console.WriteLine("|");
                Console.WriteLine("|");
                Console.WriteLine("|");
                Console.WriteLine("|");
                Console.WriteLine("|");

                Console.WriteLine($"★★★★★★★★CalenderSample★★★★★★★★★★★★★★★");
                await CalenderSample();
                Console.WriteLine($"★★★★★★★★CalenderSample★★★★★★★★★★★★★★★");
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static async Task YoutubeSample()
        {            
            var service = new VideosService(@".\ServiceAccountKey.json");
            foreach (var video in await service.GetPlayListAsync("PlayList ID"))
            {
                Console.WriteLine($"Title: {video.Snippet.Title}");
            }
        }

        private static async Task CalenderSample()
        {
            var service = new CalendarService(@".\ServiceAccountKey.json");
            var events = await service.ReadEventsAsync("Calendar ID");

            Console.WriteLine("Upcoming events:");
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    var when = eventItem.Start.DateTime.ToString();
                    if (string.IsNullOrEmpty(when))
                    {
                        when = eventItem.Start.Date;
                    }

                    Console.WriteLine("{0} start：({1}) end：({2})", eventItem.Summary, when, eventItem.End.DateTime.ToString());
                }
            }
            else
            {
                Console.WriteLine("No upcoming events found.");
            }
        }
    }
}
```

## ServiceAccountCredential

```C#
var service = new VideosService(@".\ServiceAccountKey.json");
```

https://qiita.com/GodPhwng/items/4310586e1ec3bda4d91f
