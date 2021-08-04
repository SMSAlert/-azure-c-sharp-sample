using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static Smsalert;

namespace SendSMS.Function
{
    public static class SendSMS
    {
        [FunctionName("SendSMS")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string username = "YOUR_SMSALERT_USERNAME"; //Add your SMS Alert username here.
            string password = "YOUR_SMSALERT_PASSWORD"; //Add your SMS Alert password here.

            //If you want to use API key uncomment below line.
            //string apikey = "API_KEY_HERE";

            string sender = req.Query["sender"];
            string mobileno = req.Query["mobileno"];
            string message = req.Query["message"];
            string route = req.Query["route"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            sender = sender ?? data?.sender;
            mobileno = mobileno ?? data?.mobileno;
            message = message ?? data?.message;
            route = route ?? data?.route;

            if (sender == null || mobileno == null || message == null)
            {
                string responseMessage = "Pass sender, mobileno, message and route(optional) in the query string or in the request body to send the SMS.";
                return new OkObjectResult(responseMessage);
            }
            else
            {

                Smsalert obj = new Smsalert(username, password);

                //If you want to authenticate using API key uncomment below line.
                //Smsalert obj = new Smsalert(apikey);

                String res = obj.sendsms(sender, mobileno, message, route);

                string responseMessage = $"SMS sent! {res}";
                return new OkObjectResult(responseMessage);
            }
        }
    }
}
