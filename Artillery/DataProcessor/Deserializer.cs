namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.DataProcessor.ImportDto;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            var countryDtos = Deserialize<ImportXmlCoutry[]>(xmlString, "Countries");
            var sb = new StringBuilder();
            var countries = new List<Country>();

            foreach (var dto in countryDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;   
                }

                var country = new Country()
                {
                    CountryName = dto.CountryName,
                    ArmySize = dto.ArmySize
                };

                countries.Add(country);
                sb.AppendLine(String.Format(SuccessfulImportCountry, dto.CountryName, dto.ArmySize));
            }

            context.AddRange(countries);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            var manufacturerDtos = Deserialize<ImportXmlManufacturer[]>(xmlString, "Manufacturers");
            var sb = new StringBuilder();
            var manufacturers = new List<Manufacturer>();

            foreach (var dto in manufacturerDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var manufacturer = new Manufacturer()
                {
                    ManufacturerName = dto.ManufacturerName,
                    Founded = dto.Founded
                };

                manufacturers.Add(manufacturer);
                var founded = string.Join(", ", dto.Founded.Split(' '));
                sb.AppendLine(String.Format(SuccessfulImportManufacturer, dto.ManufacturerName, founded));
            }

            context.AddRange(manufacturers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            var shellDtos = Deserialize<ImportXmlShell[]>(xmlString, "Shells");
            var sb = new StringBuilder();
            var shells = new List<Shell>();

            foreach (var dto in shellDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var shell = new Shell()
                {
                    ShellWeight = dto.ShellWeight,
                    Caliber = dto.Caliber
                };

                shells.Add(shell);
                sb.AppendLine(String.Format(SuccessfulImportShell, dto.Caliber, dto.ShellWeight));
            }

            context.AddRange(shells);
            context.SaveChanges();


            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            throw new NotImplementedException();
        }

        private static T Deserialize<T>(string inputXml, string rootName)
        {
            var root = new XmlRootAttribute(rootName);
            var serializer = new XmlSerializer(typeof(T), root);

            using StringReader reader = new StringReader(inputXml);

            T dtos = (T)serializer.Deserialize(reader);
            return dtos;
        }

        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}