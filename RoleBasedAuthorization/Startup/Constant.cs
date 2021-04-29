
namespace RoleBasedAuthorization
{
    public static class Constant
    {
        //config JWT token
        public const string Issuer = Audiance;
        public const string Audiance = "https://localhost:5001/";
        public const string Secret = "this_is_our_supper_long_my_name_is_huynh_nhat_minh";

        //Account Twilio
        public const string accountSid = "AC10b661551dca73f562a3a047523b3217";
        public const string authToken = "6f2868a56b6c1134af5a5ee1998063df";

        //
        public const string randomString = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        public const string contactCustomer = "+840832511369";
        public const string contactSystems = "+17015994809";

        public const string SMSMessageLogin = " la ma OTP cho Dang nhapvao tai khoan Shop, ma OTP se het han sau 5 phut. Luu y: Tuyet doi khong cung cap ma OTP cua ban vi bat cu ly do gi.";
        public const string OPTMessage = " (ma OTP Application se het han sau 5 phut). Luu y: Tuyet doi khong cung cap ma OTP cua ban vi bat cu ly do gi.";
    }

}
