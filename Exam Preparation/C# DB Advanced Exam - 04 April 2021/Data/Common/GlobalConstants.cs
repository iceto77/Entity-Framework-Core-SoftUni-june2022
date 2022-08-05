namespace TeisterMask.Data.Common
{
    public class GlobalConstants
    {
        //Employee
        public const int EmployeeUsernameMinLength = 3;
        public const int EmployeeUsernameMaxLength = 40;
        public const int EmployeePhoneMaxLength = 12;
        public const string EmployeeUsernameRegex = @"^(\w*\d*)$";
        public const string EmployeePhoneRegex = @"^\d{3}-\d{3}-\d{4}$";

        //Project
        public const int ProjectNameMinLength = 2;
        public const int ProjectNameMaxLength = 40;

        //Task
        public const int TasktNameMinLength = 2;
        public const int TasktNameMaxLength = 40;

    }
}
