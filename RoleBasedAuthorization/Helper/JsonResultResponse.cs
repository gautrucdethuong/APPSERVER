
namespace RoleBasedAuthorization.Helper
{
    public class JsonResultResponse
    {

        public static object ResponseSuccess(object response = null)
        {
            return new { Result = "Successed.", Data = response };
        }

        public static object ResponseChange(string message = null)
        {
            return new { Result = "Successed.", Message = message };
        }


        public static object ResponseFail(string message = null)
        {
            return new { Result = "Failed.", Message = message};
        }


        public static object ReponseError(string message = null, object reponse = null)
        {
            return new { Result = "Error.", Message = message, Data = reponse };
        }

    }
}

