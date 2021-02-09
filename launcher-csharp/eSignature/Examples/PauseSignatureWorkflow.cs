using DocuSign.CodeExamples.Models;
using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;
using System;
using System.Collections.Generic;

namespace eSignature.Examples
{
    public class PauseSignatureWorkflow
    {
        /// <summary>
        /// Applies a brand to the envelope
        /// </summary>
        /// <param name="brandId">The brand ID</param>
        /// <param name="accessToken">Access Token for API call (OAuth)</param>
        /// <param name="basePath">BasePath for API calls (URI)</param>
        /// <param name="accountId">The DocuSign Account ID (GUID or short version) for which the APIs call would be made</param>
        /// <param name="signerEmail">Email address for the signer</param>
        /// <param name="signerName">Full name of the signer</param>
        /// <param name="docPdf">String of bytes representing the document (pdf)</param>
        /// <returns>The summary of the envelopes</returns>
        public static EnvelopeSummary PauseWorkflow(RecipientModel recipient1, RecipientModel recipient2, string accessToken, string basePath, string accountId)
        {
            // Construct your API headers
            var apiClient = new ApiClient(basePath);
            apiClient.Configuration.DefaultHeader.Add("Authorization", "Bearer " + accessToken);

            // Construct request body
            var envelope = MakeEnvelope(recipient1, recipient2);

            // Call the eSignature REST API
            EnvelopesApi envelopesApi = new EnvelopesApi(apiClient);

            return envelopesApi.CreateEnvelope(accountId, envelope);
        }

        private static EnvelopeDefinition MakeEnvelope(RecipientModel recipient1, RecipientModel recipient2)
        {
            var document = new Document()
            {
                DocumentBase64 = "DQoNCg0KDQoJCVdlbGNvbWUgdG8gdGhlIERvY3VTaWduIFJlY3J1aXRpbmcgRXZlbnQNCgkJDQoJCQ0KCQlQbGVhc2UgU2lnbiBpbiENCgkJDQoJCQ0KCQk=",
                DocumentId = "1",
                FileExtension = "txt",
                Name = "Welcome"
            };

            var workflowStep = new WorkflowStep()
            {
                Action = "pause_before",
                TriggerOnItem = "routing_order",
                ItemId = "2"
            };

            var signer1 = new Signer()
            {
                Email = recipient1.Email,
                Name = recipient1.Name,
                RecipientId = "1",
                RoutingOrder = "1",
                Tabs = new Tabs
                {
                    SignHereTabs = new List<SignHere>
                    {
                        new SignHere()
                        {
                            DocumentId = "1",
                            PageNumber = "1",
                            TabLabel = "Sign Here",
                            XPosition = "200",
                            YPosition = "200"
                        }
                    }
                }
            };

            var signer2 = new Signer()
            {
                Email = recipient2.Email,
                Name = recipient2.Name,
                RecipientId = "2",
                RoutingOrder = "2",
                Tabs = new Tabs
                {
                    SignHereTabs = new List<SignHere>
                    {
                        new SignHere()
                        {
                            DocumentId = "1",
                            PageNumber = "1",
                            TabLabel = "Sign Here",
                            XPosition = "300",
                            YPosition = "200"
                        }
                    }
                }
            };

            var envelopeDefinition = new EnvelopeDefinition()
            {
                Documents = new List<Document> { document },
                EmailSubject = "EnvelopeWorkflowTest",
                Workflow = new Workflow { WorkflowSteps = new List<WorkflowStep> { workflowStep } },
                Recipients = new Recipients { Signers = new List<Signer> { signer1, signer2 } },
                Status = "Sent"
            };

            return envelopeDefinition;
        }
    }
}
