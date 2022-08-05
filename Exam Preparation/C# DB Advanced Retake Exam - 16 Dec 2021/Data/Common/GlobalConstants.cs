namespace Artillery.Data.Common
{
    public class GlobalConstants
    {
        //Country
        public const int CountryNameMinLength = 4;
        public const int CountryNameMaxLength = 60;
        public const int CountryArmySizeMin = 50000;
        public const int CountryArmySizeMax = 10000000;

        //Manufacturer
        public const int ManufacturerNameMinLength = 4;
        public const int ManufacturerNameMaxLength = 40;
        public const int ManufacturerFoundedMinLength = 10;
        public const int ManufacturerFoundedMaxLength = 100;

        //Shell
        public const double ShelShellWeightMin = 2.0;
        public const double ShelShellWeightMax = 1680.0;
        public const int ShellCaliberMinLength = 4;
        public const int ShellCaliberMaxLength = 30;

        //Gun
        public const int GunGunWeightMin = 100;
        public const int GunGunWeightMax = 1350000;
        public const double GunBarrelLengthMin = 2.0;
        public const double GunBarrelLengthMax = 35.00;
        public const int GunRangeMin = 1;
        public const int GunRangeMax = 100000;
    }
}
