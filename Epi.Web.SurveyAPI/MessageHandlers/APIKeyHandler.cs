using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Epi.Web.SurveyAPI.Repository;
using System.IO;
using System.Reflection;
using System.Configuration;
using System.Web.Http;

namespace Epi.Web.SurveyAPI.MessageHandlers
{
    public class APIKeyHandler : DelegatingHandler
    {
        private const string yourApiKey = "2fc8635e-9f69-4c28-ba03-94834be72fbd";     


        public APIKeyHandler()
        {
            
        }



        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            bool isValidAPIRequest = false;
            IEnumerable<string> lsHeaders;
            string  SurveyId;
            //Validate that the api keys exists in Client request headers.
            var checkApiKeyExists = request.Headers.TryGetValues("SurveyId", out lsHeaders);
            SurveyId = lsHeaders.FirstOrDefault();          
            if (!string.IsNullOrEmpty(SurveyId))
            {
                try
                {
                    //using Service Locator pattern instead of Dependency injection in the DelegateHandler
                     var requestScopedService = request.GetDependencyScope().GetService(typeof(ISurveyResponseRepository)) as ISurveyResponseRepository;
                    //checking if the keys are valid GUIDS
                   requestScopedService.SurveyId = new Guid(SurveyId); 
                    var surveyInfo = requestScopedService.GetSurveyInfoById(SurveyId);
                    if(surveyInfo!=null)
                    {
                        isValidAPIRequest = true;
                        requestScopedService.PublisherKey = surveyInfo.UserPublishKey;
                        requestScopedService.OrgKey = surveyInfo.OrganizationKey;
                    }
                }
                catch(Exception ex)
                {
                    return request.CreateResponse(HttpStatusCode.Unauthorized, "Authentication failed. "+ ex.Message.ToString());//401 Unauthorized The user is not authorized to use the API.
                }
            }
            else
            {
                return request.CreateResponse(HttpStatusCode.BadRequest, "Access denied");//400 Bad Request The request URI does not match the APIs in the system, or the operation failed for unknown reasons. Invalid headers can also cause this error. 
            }
            //If the key is not valid, return an http status code.
            if (!isValidAPIRequest)
            {
                return request.CreateResponse(HttpStatusCode.Unauthorized, "Authentication failed."); //401 Unauthorized The user is not authorized to use the API.
            }
            else
            {                
                var response = await base.SendAsync(request, cancellationToken);
                //Return the response back up the chain
                return response;
            }
            //Allow the request to process further down the pipeline
        }
    }
}