namespace ttnm.Helpers
{
    public static class PhoneNumberValidator
    {
        public static bool IsPhoneNumberValid(string phoneNumber)
        {
            if (phoneNumber[0] != '0')
                return false;

            if(phoneNumber.Length < 10 || phoneNumber.Length > 10)
                return false;

            return true;
        }
    }
}
