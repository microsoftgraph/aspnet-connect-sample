/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft_Graph_SDK_ASPNET_Connect.Models;
using Microsoft.Graph;

namespace UnitTests
{
    [TestClass]
    public class Tests
    {
        private static string accessToken = null;
        private static GraphServiceClient client = null;
        private static string clientId = Environment.GetEnvironmentVariable("test_client_id");
        private static string clientSecret = Environment.GetEnvironmentVariable("test_client_secret");
        private static string userName = Environment.GetEnvironmentVariable("test_user_name");
        private static string password = Environment.GetEnvironmentVariable("test_password");

        [TestMethod]
        // Get an access token to use for testing the sample calls.
        public void GetAccessTokenUsingPasswordGrant()
        {
            JObject jResult = null;
            String urlParameters = String.Format(
                    "grant_type={0}&resource={1}&client_id={2}&client_secret={3}&username={4}&password={5}",
                    "password",
                    "https%3A%2F%2Fgraph.microsoft.com%2F",
                    clientId,
                    clientSecret,
                    userName,
                    password
            );

            HttpClient client = new HttpClient();
            var body = new StringContent(urlParameters, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
            Task<HttpResponseMessage> requestTask = client.PostAsync("https://login.microsoftonline.com/common/oauth2/token", body);
            requestTask.Wait();
            HttpResponseMessage response = requestTask.Result;

            if (response.IsSuccessStatusCode)
            {
                Task<string> responseTask = response.Content.ReadAsStringAsync();
                responseTask.Wait();
                string responseContent = responseTask.Result;
                jResult = JObject.Parse(responseContent);
                accessToken = (string)jResult["access_token"];
            }

            Assert.IsNotNull(accessToken, accessToken.Substring(0, 12));
        }

        [TestMethod]
        // Initialize a Graph Service Client to use for testing the sample calls.
        public void GetAuthenticatedClient()
        {
            try
            {
                client = new GraphServiceClient(
                    new DelegateAuthenticationProvider(
                        async (requestMessage) =>
                        {
                            // Append the access token to the request.
                            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
                        }));
            }
            catch (ServiceException se)
            {
                Assert.Fail("Threw " + se.Error.Message);
            }

            Assert.IsNotNull(client, client.BaseUrl);
        }

        [TestMethod]
        // Test GraphService.GetMyEmailAddress method. 
        // Gets the email address of the test account.
        // Success: Retrieved email address matches test account's email address.
        public async Task GetMyEmailAddress()
        {
            // Arrange
            string emailAddress = null;
            GraphService graphService = new GraphService();

            // Act
            emailAddress = await graphService.GetMyEmailAddress(client);

            // Assert
            Assert.IsTrue(emailAddress.ToLower() == userName.ToLower(), emailAddress.ToString());
        }

        [TestMethod]
        // Test GraphService.SendEmail method. 
        // Sends an email to the test account from the test account.
        // Success: Task completes without throwing an exception.
        public async Task SendEmail()
        {
            // Arrange.
            GraphService graphService = new GraphService();
            string subject = "Test email from ASP.NET 4.6 Connect sample";
            string bodyContent = "<html><body>The body of the test email.</body></html>";
            List<Recipient> recipientList = new List<Recipient>();
            recipientList.Add(new Recipient
            {
                EmailAddress = new EmailAddress
                {
                    Address = userName
                }
            });
            Message message = new Message
            {
                Body = new ItemBody
                {
                    Content = bodyContent,
                    ContentType = BodyType.Html,
                },
                Subject = subject,
                ToRecipients = recipientList
            };

            // Act
            Task task = graphService.SendEmail(client, message);

            // Assert
            Task.WaitAll(task);
            Assert.IsTrue(task.IsCompleted, task.Exception?.Message);
        }
    }
}
