//using Epi.Web.Common.BusinessObject;
//using Epi.Web.Common.DTO;
//using Epi.Web.Common.Message;
//using Epi.Web.Common.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Epi.Web.SurveyAPI.Models;
//using Epi.Web.Common.ObjectMapping;
//using static Epi.Web.BLL.SurveyResponse;
using System.Configuration;
using Epi.Web.SurveyAPI.Web.Common.Message;
using Epi.Web.SurveyAPI.BLL;
using Epi.Web.SurveyAPI.Web.Common.DTO;
using Epi.Web.SurveyAPI.EF;
using Epi.Web.SurveyAPI.Web.Common.BusinessObject;
using Epi.Web.SurveyAPI.Web.Common.ObjectMapping;
using static Epi.Web.SurveyAPI.BLL.SurveyResponse;

namespace Epi.Web.SurveyAPI.Repository
{
    public class SurveyResponseRepository : ISurveyResponseRepository
    {
        private List<SurveyAnswerModel> SurveyAnswerModelList = new List<SurveyAnswerModel>();

       public Guid SurveyId
        {
            get; set;
        }

        public Guid OrgKey
        {
            get; set;
        }

        public Guid PublisherKey
        {
            get; set;
        }

        public SurveyResponseRepository()
        {
        }
       
        public SurveyControlsResponse GetSurveyControlsList(SurveyControlsRequest pRequestMessage)
        {
            SurveyControlsResponse SurveyControlsResponse = new SurveyControlsResponse();
            try
            {
                Interfaces.DataInterfaces.ISurveyInfoDao ISurveyInfoDao = new EntitySurveyInfoDao();
                SurveyInfo Implementation = new BLL.SurveyInfo(ISurveyInfoDao);
                SurveyControlsResponse = Implementation.GetSurveyControlList(pRequestMessage.SurveyId);
            }
            catch (Exception ex)
            {
                SurveyControlsResponse.Message = "Error";
                throw ex;
            }
            return SurveyControlsResponse;
        }
      
        public Dictionary<string,string> ProcessModforRadioControls(IEnumerable<SurveyControlDTO> surveyControlList,Dictionary<string,string>SurveyQuestionAnswerList)
        {            
            foreach (SurveyControlDTO s in surveyControlList)
            {
                if (SurveyQuestionAnswerList.Keys.Contains(s.ControlId))
                {
                    int val = Convert.ToInt32(SurveyQuestionAnswerList[s.ControlId]);
                    SurveyQuestionAnswerList[s.ControlId] = (val % 100).ToString();
                }
            }
            return SurveyQuestionAnswerList;
        }

        public Dictionary<string, string> ProcessValforCheckBoxControls(IEnumerable<SurveyControlDTO> surveyControlList, Dictionary<string, string> SurveyQuestionAnswerList)
        {
            foreach (SurveyControlDTO s in surveyControlList)
            {
                if (SurveyQuestionAnswerList.Keys.Contains(s.ControlId))
                {
                    string val =SurveyQuestionAnswerList[s.ControlId];
                    if (val != null && val.ToLower() == "false")
                        SurveyQuestionAnswerList[s.ControlId] = "No";
                    else if(val!=null && val.ToLower()=="true")
                     SurveyQuestionAnswerList[s.ControlId] = "Yes";
                }
            }
            return SurveyQuestionAnswerList;
        }

        public Dictionary<string, string> ProcessValforYesNoControls(IEnumerable<SurveyControlDTO> surveyControlList, Dictionary<string, string> SurveyQuestionAnswerList)
        {
            foreach (SurveyControlDTO s in surveyControlList)
            {
                if (SurveyQuestionAnswerList.Keys.Contains(s.ControlId))
                {
                    string val = SurveyQuestionAnswerList[s.ControlId];
                    if (val != null && val == "2")
                        SurveyQuestionAnswerList[s.ControlId] = "1";
                    else if (val != null && val== "1")
                        SurveyQuestionAnswerList[s.ControlId] = "0";
                }
            }
            return SurveyQuestionAnswerList;
        }

        /// <summary>
        /// Inserts SurveyResponse 
        /// </summary>
        /// <param name="SurveyAnswerModel"></param>
        /// <returns>response </returns>
        public PreFilledAnswerResponse SetSurveyAnswer(SurveyAnswerModel request)
        {
            PreFilledAnswerResponse response;
            SurveyControlsResponse SurveyControlsResponse = new SurveyControlsResponse();
            SurveyControlsRequest surveyControlsRequest = new SurveyControlsRequest();
            surveyControlsRequest.SurveyId= request.SurveyId.ToString();
            SurveyControlsResponse = GetSurveyControlsList(surveyControlsRequest);
            Dictionary<string, string> FilteredAnswerList = new Dictionary<string, string>();
            var radiolist = SurveyControlsResponse.SurveyControlList.Where(x => x.ControlType == "GroupBoxRadioList");
            FilteredAnswerList = ProcessModforRadioControls(radiolist, request.SurveyQuestionAnswerListField);
            var checkboxLsit = SurveyControlsResponse.SurveyControlList.Where(x => x.ControlType == "CheckBox");
            FilteredAnswerList = ProcessValforCheckBoxControls(checkboxLsit, FilteredAnswerList);
            var yesNoList = SurveyControlsResponse.SurveyControlList.Where(x => x.ControlType == "YesNo");
            FilteredAnswerList = ProcessValforYesNoControls(yesNoList, FilteredAnswerList);
            try
            {                                  
                    Interfaces.DataInterfaces.ISurveyResponseDao SurveyResponseDao = new EntitySurveyResponseDao();
                BLL.SurveyResponse Implementation = new BLL.SurveyResponse(SurveyResponseDao);
                    PreFilledAnswerRequest prefilledanswerRequest = new PreFilledAnswerRequest();
                    Dictionary<string, string> Values = new Dictionary<string, string>();
                    prefilledanswerRequest.AnswerInfo.UserPublishKey = request.PublisherKey;
                    prefilledanswerRequest.AnswerInfo.OrganizationKey = request.OrgKey;
                    prefilledanswerRequest.AnswerInfo.SurveyId = request.SurveyId;
                    prefilledanswerRequest.AnswerInfo.UserPublishKey = request.PublisherKey;                  
                    foreach (KeyValuePair<string, string> entry in FilteredAnswerList)
                    {
                        Values.Add(entry.Key, entry.Value);
                    }
                    prefilledanswerRequest.AnswerInfo.SurveyQuestionAnswerList = Values;
                    response = Implementation.SetSurveyAnswer(prefilledanswerRequest);
                    return response;               
            }
            catch (Exception ex)
            {
                PassCodeDTO DTOList = new PassCodeDTO();
                 response = new PreFilledAnswerResponse(DTOList);

                response.ErrorMessageList.Add("Failed", "Failed to insert Response");
                response.Status = ((BLL.SurveyResponse.Message)1).ToString();
                return response;
            }
        }

        /// <summary>
        /// Updates SurveyResponse 
        /// </summary>
        /// <param name="SurveyAnswerModel",name="ResponseId"></param>
        /// <returns>response </returns>
        public PreFilledAnswerResponse Update(SurveyAnswerModel request,string ResponseId)
        {
            PreFilledAnswerResponse response;
            SurveyControlsResponse SurveyControlsResponse = new SurveyControlsResponse();
            SurveyControlsRequest surveyControlsRequest = new SurveyControlsRequest();
            surveyControlsRequest.SurveyId = request.SurveyId.ToString();
            SurveyControlsResponse = GetSurveyControlsList(surveyControlsRequest);
            Dictionary<string, string> FilteredAnswerList = new Dictionary<string, string>();
            var radiolist = SurveyControlsResponse.SurveyControlList.Where(x => x.ControlType == "GroupBoxRadioList");
            FilteredAnswerList = ProcessModforRadioControls(radiolist, request.SurveyQuestionAnswerListField);
            var checkboxLsit = SurveyControlsResponse.SurveyControlList.Where(x => x.ControlType == "CheckBox");
            FilteredAnswerList = ProcessValforCheckBoxControls(checkboxLsit, FilteredAnswerList);
            var yesNoList = SurveyControlsResponse.SurveyControlList.Where(x => x.ControlType == "YesNo");
            FilteredAnswerList = ProcessValforYesNoControls(yesNoList, FilteredAnswerList);
            try
            {
                Interfaces.DataInterfaces.ISurveyResponseDao SurveyResponseDao = new EntitySurveyResponseDao();
                BLL.SurveyResponse Implementation = new BLL.SurveyResponse(SurveyResponseDao);
                PreFilledAnswerRequest prefilledanswerRequest = new PreFilledAnswerRequest();
                Dictionary<string, string> Values = new Dictionary<string, string>();
                prefilledanswerRequest.AnswerInfo.UserPublishKey = request.PublisherKey;
                prefilledanswerRequest.AnswerInfo.OrganizationKey = request.OrgKey;
                prefilledanswerRequest.AnswerInfo.SurveyId = request.SurveyId;
                prefilledanswerRequest.AnswerInfo.UserPublishKey = request.PublisherKey;
                var updatedtime = FilteredAnswerList.Where(x => x.Key.ToLower() == "_updatestamp").FirstOrDefault();
                var Responsekey = FilteredAnswerList.Where(x => x.Key.ToLower() == "responseid" || x.Key.ToLower() == "id").FirstOrDefault().Key;
                FilteredAnswerList.Remove(Responsekey);
                FilteredAnswerList.Remove(updatedtime.Key);
                foreach (KeyValuePair<string, string> entry in FilteredAnswerList)
                {
                    Values.Add(entry.Key, entry.Value);
                }
                prefilledanswerRequest.AnswerInfo.SurveyQuestionAnswerList = Values;

                Dictionary<string, string> ErrorMessageList = new Dictionary<string, string>();
                List<SurveyInfoBO> SurveyBOList = Implementation.GetSurveyInfo(prefilledanswerRequest);//
                string Xml = Implementation.CreateResponseXml(prefilledanswerRequest, SurveyBOList);//
                ErrorMessageList = Implementation.ValidateResponse(SurveyBOList, prefilledanswerRequest);//

                if (ErrorMessageList.Count() > 0)
                {
                    response = new PreFilledAnswerResponse();
                    response.ErrorMessageList = ErrorMessageList;
                    response.Status = ((Message)1).ToString();
                }
                else
                {
                    SurveyResponseBO surveyresponseBO = new SurveyResponseBO(); SurveyResponseBO SurveyResponse = new SurveyResponseBO();
                    UserAuthenticationRequestBO UserAuthenticationRequestBO = new UserAuthenticationRequestBO();
                    surveyresponseBO.SurveyId = request.SurveyId.ToString();
                    surveyresponseBO.ResponseId = ResponseId.ToString();
                    surveyresponseBO.XML = Xml;
                    System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    surveyresponseBO.DateUpdated = dateTime.AddMilliseconds(Convert.ToDouble(updatedtime.Value.ToString())).ToLocalTime();
                    surveyresponseBO.DateCompleted = dateTime.AddMilliseconds(Convert.ToDouble(updatedtime.Value.ToString())).ToLocalTime();

                    surveyresponseBO.Status = 2;                   
                    SurveyResponse = Implementation.UpdateSurveyResponse(surveyresponseBO);
                    UserAuthenticationRequestBO = Web.Common.ObjectMapping.Mapper.ToBusinessObject(ResponseId);
                    Implementation.SavePassCode(UserAuthenticationRequestBO);

                    //return Response
                    string ResponseUrl = ConfigurationManager.AppSettings["ResponseURL"];
                    response = new PreFilledAnswerResponse(Web.Common.ObjectMapping.Mapper.ToDataTransferObjects(UserAuthenticationRequestBO));
                    response.SurveyResponseUrl = ResponseUrl + UserAuthenticationRequestBO.ResponseId;
                    response.Status = ((Message)2).ToString(); 
                }
                return response;
            }
            catch (Exception ex)
            {
                PassCodeDTO DTOList = new PassCodeDTO();
                response = new PreFilledAnswerResponse(DTOList);

                response.ErrorMessageList.Add("Failed", "Failed to insert Response");
                response.Status = ((BLL.SurveyResponse.Message)1).ToString();
                return response;
            }
        }

        /// <summary>
        /// Validate Header information coming from the request
        /// </summary>
        /// <param name="SurveyId",name="PublisherKey",name="OrgKey"></param>
        /// <returns>response </returns>
        public bool IsSurveyInfoValidByOrgKeyAndPublishKey(string SurveyId,string PublisherKey,string OrgKey) 
        {
            Interfaces.DataInterfaces.ISurveyInfoDao surveyInfoDao = new EF.EntitySurveyInfoDao();
            BLL.SurveyInfo SurveyInfo = new BLL.SurveyInfo(surveyInfoDao);
            bool IsValidOrgKeyAndPublishKey=SurveyInfo.IsSurveyInfoValidByOrgKeyAndPublishKey(SurveyId, OrgKey, new Guid(PublisherKey));            
            return IsValidOrgKeyAndPublishKey;
        }

        /// <summary>
        /// Updates SurveyResponse 
        /// </summary>
        /// <param name="id"></param>
        public void Remove(string id)
        {
            Interfaces.DataInterfaces.ISurveyResponseDao SurveyResponseDao = new EntitySurveyResponseDao();
            BLL.SurveyResponse Implementation = new BLL.SurveyResponse(SurveyResponseDao);
            SurveyResponseBO surveyresponseBO = new SurveyResponseBO();
            surveyresponseBO.ResponseId = id;
            surveyresponseBO.Status = 0;          
            Implementation.UpdateRecordStatus(surveyresponseBO);
        }

      

    }
}