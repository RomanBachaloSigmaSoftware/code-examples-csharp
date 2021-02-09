﻿using DocuSign.CodeExamples.Controllers;
using DocuSign.CodeExamples.Models;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;
using eSignature.Examples;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DocuSign.CodeExamples.eSignature.Controllers
{
    [Area("eSignature")]
    [Route("Eg034")]
    public class Eg034ConditionalRecipientsWorkflowController : EgController
    {
        public Eg034ConditionalRecipientsWorkflowController(DSConfiguration config, IRequestItemsService requestItemsService) 
            : base(config, requestItemsService)
        {
        }

        public override string EgName => "Eg034";

        [HttpPost]
        public IActionResult Create(RecipientModel recipient1, RecipientModel conditionalRecipient1, RecipientModel conditionalRecipient2)
        {
            // Check the token with minimal buffer time.
            bool tokenOk = CheckToken(3);

            if (!tokenOk)
            {
                // We could store the parameters of the requested operation 
                // so it could be restarted automatically.
                // But since it should be rare to have a token issue here,
                // we'll make the user re-enter the form data after 
                // authentication.
                RequestItemsService.EgName = EgName;
                return Redirect("/ds/mustAuthenticate");
            }

            string basePath = RequestItemsService.Session.BasePath + "/restapi";

            // Obtain your OAuth token
            string accessToken = RequestItemsService.User.AccessToken;
            string accountId = RequestItemsService.Session.AccountId;

            EnvelopeSummary results;

            try
            {
                // Call the eSignature API
                results = ConditionalRecipientsWorkflow.SendEnvelope(accessToken, basePath, accountId, recipient1,
                                                                     conditionalRecipient1, conditionalRecipient2);
            }
            catch (ApiException apiException)
            {
                ViewBag.errorCode = apiException.ErrorCode;

                if (apiException.Message.Contains("WORKFLOW_UPDATE_RECIPIENTROUTING_NOT_ALLOWED"))
                {
                    ViewBag.errorMessage = "Update to the workflow with recipient routing is not allowed for your account!";
                    ViewBag.errorInformation = "Please contact with our <a href='https://developers.docusign.com/support/' target='_blank'>support team</a> to resolve this issue.";
                }
                else
                {
                    ViewBag.errorMessage = apiException.Message;
                }

                return View("Error");
            }

            // Process results
            ViewBag.h1 = "The envelope was created successfully!";
            ViewBag.message = "Results from the Envelopes::create method:";
            ViewBag.Locals.Json = JsonConvert.SerializeObject(results, Formatting.Indented);
            return View("example_done");
        }
    }
}