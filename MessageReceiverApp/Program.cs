using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using MessageReceiverApp;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

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

                Task.Factory.StartNew(() =>
                {
                    using (var awsClient = new AmazonSQSClient(credentials, region))
                    {
                        while (true)
                        {
                            Console.WriteLine("Waiting for a Message ...");

                            var receiveReqeust = new ReceiveMessageRequest(queueUrl);
                            var response = awsClient.ReceiveMessageAsync(receiveReqeust).Result;
                            var receivedMessages = response.Messages;
                            foreach (var receivedMessage in receivedMessages)
                            {
                                if (receivedMessage != default(Message))
                                {
                                    var messageBody = JsonConvert.DeserializeObject<Order>(
                                            receivedMessage.Body);

                                    Console.WriteLine($"Order Processing Completed ... {messageBody.ToString()}");

                                    var deleteMessageRequest = new DeleteMessageRequest
                                    {
                                        QueueUrl = queueUrl,
                                        ReceiptHandle = receivedMessage.ReceiptHandle
                                    };

                                    awsClient.DeleteMessageAsync(deleteMessageRequest).Wait();

                                    Console.WriteLine("Message has been removed successfully!");
                                }
                            }

                            Thread.Sleep(2000);
                        }
                    };
                });
            }
            catch (Exception exceptionObject)
            {
                Console.WriteLine("Error Occurred, Details : " + exceptionObject.Message);
            }

            Console.WriteLine("Press [ENTER] to exit the application, it automatically stops polling ... ");
            Console.ReadLine();
        }
    }
}