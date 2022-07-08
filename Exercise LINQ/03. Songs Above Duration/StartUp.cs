namespace MusicHub
{
    using System;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context = 
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here
            string result = ExportSongsAboveDuration(context, 4);
            Console.WriteLine(result);
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            StringBuilder output = new StringBuilder();
            var albumsInfo = context
                .Albums
                .Where(a => a.ProducerId.Value == producerId)
                .Include(a => a.Producer )
                .Include(a => a.Songs)
                .ThenInclude(s => s.Writer)
                .ToArray()
                .Select(a => new
                {
                    AlbumName = a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy"),
                    ProducerName = a.Producer.Name,
                    Songs = a.Songs
                    .Select(s => new
                    {
                        SongName = s.Name,
                        s.Price,
                        SongWriter = s.Writer.Name
                    })
                    .OrderByDescending(s => s.SongName)
                    .ThenBy(s => s.SongWriter)
                    .ToArray(),
                    TotalPrice = a.Price
                })
                .OrderByDescending(a => a.TotalPrice)
                .ToArray();
            foreach (var album in albumsInfo)
            {
                output
                    .AppendLine($"-AlbumName: {album.AlbumName}")
                    .AppendLine($"-ReleaseDate: {album.ReleaseDate}")
                    .AppendLine($"-ProducerName: {album.ProducerName}")
                    .AppendLine($"-Songs:");
                int songCounter = 1;
                foreach (var song in album.Songs)
                {
                    output
                    .AppendLine($"---#{songCounter++}")
                    .AppendLine($"---SongName: {song.SongName}")
                    .AppendLine($"---Price: {song.Price:f2}")
                    .AppendLine($"---Writer: {song.SongWriter}");
                }
                output.AppendLine($"-AlbumPrice: {album.TotalPrice:f2}");
            }
            return output.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            StringBuilder output = new StringBuilder();
            var songsAboveDuration = context
                .Songs
                .Include(s => s.SongPerformers)
                .ThenInclude(sp => sp.Performer)
                .Include(s => s.Writer)
                .Include(s => s.Album)
                .ThenInclude(a => a.Producer)
                .ToArray()
                .Where(s => s.Duration.TotalSeconds > duration)
                .Select(s => new
                {
                    s.Name,
                    WriterName = s.Writer.Name,
                    PerformerName = s.SongPerformers
                    .Select(sp => $"{sp.Performer.FirstName} {sp.Performer.LastName}")
                    .FirstOrDefault(),
                    AlbumProducer = s.Album.Producer.Name,
                    Duration = s.Duration.ToString("c")
                })
                .OrderBy(s => s.Name)
                .ThenBy(s => s.WriterName)
                .ThenBy(s => s.PerformerName)
                .ToArray();
            int songCounter = 1;
            foreach (var sad in songsAboveDuration)
            {
                output
                    .AppendLine($"-Song #{songCounter++}")
                    .AppendLine($"---SongName: {sad.Name}")
                    .AppendLine($"---Writer: {sad.WriterName}")
                    .AppendLine($"---Performer: {sad.PerformerName}")
                    .AppendLine($"---AlbumProducer: {sad.AlbumProducer}")
                    .AppendLine($"---Duration: {sad.Duration}");
            }

            return output.ToString().TrimEnd();
        }
    }
}
