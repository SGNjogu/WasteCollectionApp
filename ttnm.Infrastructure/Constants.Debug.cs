namespace ttnm.Infrastructure
{
    public static class Constants
    {
        public static readonly string BaseUrl = "https://app.takanimali.org";
        public static readonly string LoginUrl = "api/v1/mobile/login";
        public static readonly string CheckUserUrl = "api/v1/mobile/check-user";
        public static readonly string RegistrationUrl = "api/v1/mobile/register";
        public static readonly string SaccoUrl = "api/v1/mobile/update-sacco";
        public static readonly string NhifUrl = "api/v1/mobile/update-nhif";
        public static readonly string ZoneUrl = "api/v1/mobile/update-zone";
        public static readonly string ProfileUrl = "api/v1/mobile/update-profile";
        public static readonly string AddressUrl = "api/v1/mobile/update-address";
        public static readonly string GetCollectionRequestsUrl = "api/v1/mobile/h/get-requests";
        public static readonly string AcceptCollectionRequestsUrl = "api/v1/mobile/accept-request";
        public static readonly string CollectRequestUrl = "api/v1/mobile/collect-request";
        public static readonly string CancelCollectionRequestsUrl = "api/v1/mobile/cancel-request";
        public static readonly string SupportUrl = "api/v1/mobile/submit-support-query";
        public static readonly string VerificationUrl = "api/v1/mobile/verify";
        public static readonly string ResendVerificationUrl = "api/v1/mobile/resend-verification-code";
        public static readonly string AggCollectionHistoryUrl = "api2/collectionorder";
        public static readonly string FetchCollectorsUrl = "api2/collectors";
        public static readonly string AddCollectionOrdersListUrl = "api2/collectionorder";
        public static readonly string GetAggregatorsListUrl = "api/v1/mobile/waste-type/aggregators";
        public static readonly string NewCollectionOrderUrl = "api/v1/mobile/collection-order";
        public static readonly string RequestCodeUrl = "api/v1/mobile/verify-password-request";
        public static readonly string ChangePasswordUrl = "api/v1/mobile/change-password";
    }
}
