namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theaters = context
                .Theatres
                .Where(t => t.NumberOfHalls >= numbersOfHalls)
                .Where(t => t.Tickets.Count >= 20)
                .ToArray()
                .Select(t => new
                {
                    Name = t.Name,
                    Halls = t.NumberOfHalls,
                    TotalIncome = t.Tickets
                    .Where(tk => tk.RowNumber >=1)
                    .Where(tk => tk.RowNumber <= 5)
                    .Sum(tk => tk.Price),
                    Tickets = t.Tickets
                    .Where(tk => tk.RowNumber >= 1)
                    .Where(tk => tk.RowNumber <= 5)
                    .Select(tk => new 
                    {
                        Price = tk.Price,
                        RowNumber = tk.RowNumber
                    })
                    .OrderByDescending(tk => tk.Price)
                    .ToArray()
                })
                .OrderByDescending(t => t.Halls)
                .ThenBy(t => t.Name)
                .ToArray();
            string json = JsonConvert.SerializeObject(theaters, Formatting.Indented);
            return json;
        }

        public static string ExportPlays(TheatreContext context, double rating)
        {
            var plays = context
            .Plays
            .Where(p => p.Rating <= rating)
            .OrderBy(p => p.Title)
            .ThenByDescending(p => p.Genre)
            .ToArray()
            .Select(p => new ExportPlayDto
            {
                Title = p.Title,
                Duration = p.Duration.ToString("c"),
                Rating = p.Rating == 0f ? "Premier" : p.Rating.ToString(CultureInfo.InvariantCulture),
                Genre = Enum.GetName(typeof(Genre), p.Genre),
                Actors = p.Casts
                .Where(c => c.IsMainCharacter)
                .Select(c => new ExportActorDto()
                {
                    FullName = c.FullName,
                    MainCharacter = $"Plays main character in '{p.Title}'."
                })
                .OrderByDescending(c => c.FullName)
                .ToArray()
            })
            .ToArray();

            return SerializeXML<ExportPlayDto[]>(plays, "Plays");
        }

        private static string SerializeXML<T>(T dto, string rootName)
        {
            StringBuilder sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);

            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRoot);

            xmlSerializer.Serialize(writer, dto, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
