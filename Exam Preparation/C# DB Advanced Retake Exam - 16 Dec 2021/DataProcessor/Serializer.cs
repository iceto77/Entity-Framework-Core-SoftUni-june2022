namespace Artillery.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    using Artillery.Data;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ExportDto;

    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var shellDto = context
                .Shells
                .Where(s => s.ShellWeight > shellWeight)
                .Select(s => new
                {
                    ShellWeight = s.ShellWeight,
                    Caliber = s.Caliber,
                    Guns = s.Guns
                    .Where(g => g.GunType == (GunType)Enum.Parse(typeof(GunType), "AntiAircraftGun"))                    
                    .Select(g => new 
                    {
                        GunType = g.GunType.ToString(),
                        GunWeight = g.GunWeight,
                        BarrelLength = g.BarrelLength,
                        Range = g.Range > 3000 ? "Long-range" : "Regular range"
                    })
                    .OrderByDescending(g => g.GunWeight)
                    .ToArray()
                })
                .OrderBy(r => r.ShellWeight)
                .ToArray();
            string json = JsonConvert.SerializeObject(shellDto, Formatting.Indented);
            return json;
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            ExportGunXmlDto[] gunDto = context
                .Guns
                .Where(g => g.Manufacturer.ManufacturerName == manufacturer)
                .OrderBy(g => g.BarrelLength)
                .Select(g => new ExportGunXmlDto
                {
                    Manufacturer = g.Manufacturer.ManufacturerName,
                    GunType = g.GunType.ToString(),
                    GunWeight = g.GunWeight,
                    BarrelLength = g.BarrelLength,
                    Range = g.Range,
                    Countries = g.CountriesGuns                   
                    .Select(cg => new ExportCountryDto 
                    {
                        Country =  cg.Country.CountryName,
                        ArmySize = cg.Country.ArmySize
                    })
                    .Where(c => c.ArmySize > 4500000)
                    .OrderBy(c => c.ArmySize)
                    .ToArray()
                })                
                .ToArray();

            return Serialize<ExportGunXmlDto[]>(gunDto, "Guns");
        }

        private static string Serialize<T>(T dto, string rootName)
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
