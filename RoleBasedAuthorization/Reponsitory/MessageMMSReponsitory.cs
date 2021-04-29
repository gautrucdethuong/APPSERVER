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
            TwilioClient.Init(Constant.accountSid, Constant.authToken);

            // create message send user
            var mediaUrl = new[] {
            new Uri("https://duytom.com/images/Thank-You-Picture.jpg")
        }.ToList();

            MessageResource.Create(
                body: "Thanks you sign up!",
                from: new Twilio.Types.PhoneNumber(Constant.contactSystems),
                mediaUrl: mediaUrl,
                to: new Twilio.Types.PhoneNumber(Constant.contactCustomer)
            );
            return true;
        }
    }
}
