namespace SoftJail.DataProcessor
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Newtonsoft.Json;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context
                .Prisoners
                .Where(p => ids.Contains(p.Id))
                .Select(p => new
                {
                    Id = p.Id,
                    Name = p.FullName,
                    CellNumber = p.Cell.CellNumber,
                    Officers = p.PrisonerOfficers.Select(po => new
                    {
                        OfficerName = po.Officer.FullName,
                        Department = po.Officer.Department.Name
                    })
                    .OrderBy(o => o.OfficerName)
                    .ToArray(),
                    TotalOfficerSalary = decimal.Parse(p.PrisonerOfficers.Sum(po => po.Officer.Salary).ToString("f2"))
                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToArray();
            string json = JsonConvert.SerializeObject(prisoners, Formatting.Indented);
            return json;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            string[] targetPrisoners = prisonersNames.Split(',').ToArray();
            //Моя версия с manual mapping
            //ExportInboxForPrisonerDto[] prisoners = context
            //    .Prisoners
            //    .Where(p => targetPrisoners.Contains(p.FullName))
            //    .Select(p => new ExportInboxForPrisonerDto
            //    {
            //        Id = p.Id,
            //        Name = p.FullName,
            //        IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd"),
            //        EncryptedMessages = p.Mails.Select(m => new ExportMessageDto
            //        {
            //            Description = String.Join("", m.Description.Reverse())
            //        })
            //        .ToArray()
            //    })
            //    .OrderBy(p => p.Name)
            //    .ThenBy(p => p.Id)
            //    .ToArray();

            //версия с Auto Mapping

            ExportInboxForPrisonerDto[] prisoners = context
                .Prisoners
                .Where(p => targetPrisoners.Contains(p.FullName))
                .ProjectTo<ExportInboxForPrisonerDto>(Mapper.Configuration)
                .OrderBy(p => p.FullName)
                .ThenBy(p => p.Id)
                .ToArray();

            return Serialize<ExportInboxForPrisonerDto[]>(prisoners, "Prisoners");
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