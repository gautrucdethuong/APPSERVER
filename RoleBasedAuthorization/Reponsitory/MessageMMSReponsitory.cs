using RoleBasedAuthorization.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using Twilio;
using Twilio.Rest.Api.V2010.Account;


namespace RoleBasedAuthorization.Reponsitory
{
    public class MessageMMSReponsitory : IMessageService
    {

        public bool SendMMSMessage()
        {
            //Account SID and Auth Token at twilio.com / console
            const string accountSid = "AC10b661551dca73f562a3a047523b3217";
            const string authToken = "b6cf00ac221fa7b4a203d205ef250396";

            TwilioClient.Init(accountSid, authToken);

            // create message send user
            var mediaUrl = new[] {
            new Uri("https://duytom.com/images/Thank-You-Picture.jpg")
        }.ToList();

            MessageResource.Create(
                body: "Thanks you sign up!",
                from: new Twilio.Types.PhoneNumber("+17015994809"),
                mediaUrl: mediaUrl,
                to: new Twilio.Types.PhoneNumber("+840832511369")
            );
            return true;
        }
    }
}
