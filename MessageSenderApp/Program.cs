using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using MessageSenderApp;
using Newtonsoft.Json;
using System;

namespace CGI.Training.Applications
{
    public static class MainClass
    {
        public static void Main(string[] args)
        {
            var awsSecretId = @"AKIAJ4N5T4GYXQCU4Z3A";
            var awsSecretKey = @"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";
            var region = RegionEndpoint.APSouth1;

            try
            {
                var credentials = new BasicAWSCredentials(awsSecretId, awsSecretKey);
                var queueUrl = @"https://sqs.ap-south-1.amazonaws.com/142198642907/onlineorders";

                using (var awsClient = new AmazonSQSClient(credentials, region))
                {
                    while (true)
                    {
                        Console.WriteLine("Press [ENTER] to send an order message ... or type exit to break !");

                        if (Console.ReadLine().Equals("exit", StringComparison.OrdinalIgnoreCase))
                            break;

                        var orderObject = new Order
                        {
                            OrderId = 1,
                            OrderDate = DateTime.Now,
                            Customer = "Northwind Traders",
                            ShippingAddress = "Bangalore",
                            OrderAmount = new Random().Next(10000, 50000)
                        };

                        var messageRequest = new SendMessageRequest
                        {
                            MessageBody = JsonConvert.SerializeObject(orderObject),
                            QueueUrl = queueUrl
                        };

                        var response = awsClient.SendMessageAsync(messageRequest).Result;

                        Console.WriteLine("Message Sent Successfully ... " +
                            response.HttpStatusCode.ToString());
                    }
                }
            }
            catch (Exception exceptionObject)
            {
                Console.WriteLine("Error Occurred, Details : " + exceptionObject.Message);
            }

            Console.WriteLine("End of Application!");
            Console.ReadLine();
        }
    }
}