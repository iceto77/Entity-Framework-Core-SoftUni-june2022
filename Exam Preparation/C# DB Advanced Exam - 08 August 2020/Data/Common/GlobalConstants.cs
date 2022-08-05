namespace VaporStore.Data.Common
{
    public class GlobalConstants
    {
        //User
        public const int UserUsernameMinLength = 3;
        public const int UserUsernameMaxLength = 20;
        public const string UserFullNameRegex = @"^([A-Z][a-z]*)\s([A-Z][a-z]*)$";
        public const int UserAgeMinValue = 3;
        public const int UserAgeMaxValue = 103;

        //Card
        public const string CardNumberRegex = @"^(\d{4})\s(\d{4})\s(\d{4})\s(\d{4})$";
        public const string CardCvcRegex = @"^\d{3}$";
        public const int CardCvcMaxLength = 3;

        //Purchase
        public const int PurchaseProductKeyMaxLength = 14;
        public const string PurchaseProductKeyRegex = @"^([A-Z0-9]{4})-([A-Z0-9]{4})-([A-Z0-9]{4})$";

        //Game
        public const string GamePriceMinValue = "0";
        public const string GamePriceMaxValue = "79228162514264337593543950335";

    }
}
