using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.SurveyAPI.Web.Common.BusinessObject;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;
using Epi.Web.SurveyAPI.Web.Common.Message;
using Epi.Web.SurveyAPI.Web.Common.ObjectMapping;
using Epi.Web.SurveyAPI.Web.Common.Xml;
using Epi.Web.SurveyAPI.Web.Common.DTO;
using Epi.Web.SurveyAPI.Web.Common.BusinessRule;
using Epi.Web.SurveyAPI.Web.Commom.BusinessObject;
using Epi.Web.SurveyAPI.Web.Common.Criteria;
using Epi.Web.SurveyAPI.Interfaces.DataInterfaces;
using Epi.Web.SurveyAPI.Web.Common.Security;

namespace Epi.Web.SurveyAPI.BLL
{
    public class SurveyResponse
    {

        public enum Message
            {
              Failed = 1,
              Success = 2,
           
            }
        private Interfaces.DataInterfaces.ISurveyResponseDao SurveyResponseDao;

        public SurveyResponse(Interfaces.DataInterfaces.ISurveyResponseDao pSurveyResponseDao)
        {
            this.SurveyResponseDao = pSurveyResponseDao;
        }

        public List<SurveyResponseBO> GetSurveyResponseById(List<String> pId, Guid UserPublishKey)
        {
            List<SurveyResponseBO> result = this.SurveyResponseDao.GetSurveyResponse(pId, UserPublishKey);
            return result;
        }

        //Validate User
        public bool ValidateUser(UserAuthenticationRequestBO PassCodeBoObj)
        {
            string PassCode = PassCodeBoObj.PassCode;
            string ResponseId = PassCodeBoObj.ResponseId;
            List<string> ResponseIdList = new List<string> ();
            ResponseIdList.Add(PassCodeBoObj.ResponseId);
 
            UserAuthenticationResponseBO results = this.SurveyResponseDao.GetAuthenticationResponse(PassCodeBoObj);
                
             

            bool ISValidUser = false;

            if (results != null && !string.IsNullOrEmpty(PassCode))
            {

                if (results.PassCode == PassCode)
                {
                    ISValidUser = true;


                }
                else
                {
                    ISValidUser = false;
                }
            }
            return ISValidUser;
        }
        //Save Pass code 
        public void SavePassCode( UserAuthenticationRequestBO pValue)
        {
            UserAuthenticationRequestBO result = pValue;
            this.SurveyResponseDao.UpdatePassCode(pValue);
          

           
        }
        // Get Authentication Response
        public UserAuthenticationResponseBO GetAuthenticationResponse(UserAuthenticationRequestBO pValue)
        {
            UserAuthenticationResponseBO result = this.SurveyResponseDao.GetAuthenticationResponse(pValue);

            return result; 

        }
        public List<SurveyResponseBO> GetSurveyResponseBySurveyId(List<String> pSurveyIdList, Guid UserPublishKey)
        {
            List<SurveyResponseBO> result = this.SurveyResponseDao.GetSurveyResponseBySurveyId(pSurveyIdList, UserPublishKey);
            return result;
        }

        public List<SurveyResponseBO> GetSurveyResponse(List<string> SurveyAnswerIdList, string pSurveyId, DateTime pDateCompleted,bool pIsDraftMode  ,int pStatusId,int PageNumber,int PageSize )
        {
            List<SurveyResponseBO> result = this.SurveyResponseDao.GetSurveyResponse(SurveyAnswerIdList, pSurveyId, pDateCompleted,pIsDraftMode, pStatusId,PageNumber,PageSize);
            return result;
        }

        public SurveyResponseBO InsertSurveyResponse(SurveyResponseBO pValue)
        {
            SurveyResponseBO result = pValue;
            this.SurveyResponseDao.InsertSurveyResponse(pValue);
            return result;
        }


        public SurveyResponseBO UpdateSurveyResponse(SurveyResponseBO pValue)
        {
            SurveyResponseBO result = pValue;
            this.SurveyResponseDao.UpdateSurveyResponse(pValue);
            return result;
        }

        public bool DeleteSurveyResponse(SurveyResponseBO pValue)
        {
            bool result = false;

            this.SurveyResponseDao.DeleteSurveyResponse(pValue);
            result = true;

            return result;
        }

        public PageInfoBO GetResponseSurveySize(List<string> SurveyResponseIdList, string SurveyId, DateTime pClosingDate, int BandwidthUsageFactor, bool pIsDraftMode = false, int pSurveyType = -1, int pPageNumber = -1, int pPageSize = -1, int pResponseMaxSize = -1)
        {
            List<SurveyResponseBO> SurveyResponseBOList = this.SurveyResponseDao.GetSurveyResponseSize(SurveyResponseIdList, SurveyId, pClosingDate,pIsDraftMode, pSurveyType, pPageNumber, pPageSize, pResponseMaxSize);
            PageInfoBO result = new PageInfoBO();

            result = Epi.Web.SurveyAPI.BLL.Common.GetSurveySize(SurveyResponseBOList, BandwidthUsageFactor, pResponseMaxSize);
            return result;
        }

        public PageInfoBO GetSurveyResponseBySurveyIdSize(List<string> SurveyIdList, Guid UserPublishKey, int BandwidthUsageFactor, int PageNumber = -1, int PageSize = -1, int ResponseMaxSize = -1)
        {
            List<SurveyResponseBO> SurveyResponseBOList = this.SurveyResponseDao.GetSurveyResponseBySurveyIdSize(SurveyIdList, UserPublishKey, PageNumber, PageSize, ResponseMaxSize);

            PageInfoBO result = new PageInfoBO();

            result = Epi.Web.SurveyAPI.BLL.Common.GetSurveySize(SurveyResponseBOList, BandwidthUsageFactor, ResponseMaxSize);
            return result;
         
        }
        public PageInfoBO GetSurveyResponseSize(List<string> SurveyResponseIdList, Guid UserPublishKey, int BandwidthUsageFactor, int PageNumber = -1, int PageSize = -1, int ResponseMaxSize = -1)
        {

            List<SurveyResponseBO> SurveyResponseBOList = this.SurveyResponseDao.GetSurveyResponseSize(SurveyResponseIdList, UserPublishKey, PageNumber, PageSize, ResponseMaxSize);

            PageInfoBO result = new PageInfoBO();

            result = Epi.Web.SurveyAPI.BLL.Common.GetSurveySize(SurveyResponseBOList, BandwidthUsageFactor, ResponseMaxSize);
            return result;
        }

        public string CreateResponseXml(PreFilledAnswerRequest request, List<SurveyInfoBO> SurveyBOList)
            
            {
           
            string ResponseXml;
          
            XDocument SurveyXml = new XDocument();

            foreach (var item in SurveyBOList)
                {
                SurveyXml = XDocument.Parse(item.XML);
                }
             SurveyResponseXML Implementation = new  SurveyResponseXML(request, SurveyXml);
            ResponseXml = Implementation.CreateResponseDocument(SurveyXml).ToString();


            return ResponseXml;
            }
        public Dictionary<string, string> ValidateResponse(List<SurveyInfoBO> SurveyBOList,  PreFilledAnswerRequest request)
            {
             
            XDocument SurveyXml = new XDocument();
            foreach (var item in SurveyBOList)
                {
                SurveyXml = XDocument.Parse(item.XML);
                }
              Dictionary<string, string> MessageList = new Dictionary<string, string>();
              Dictionary<string, string> FieldNotFoundList = new Dictionary<string, string>();
              Dictionary<string, string> WrongFieldTypeList = new Dictionary<string, string>();
            SurveyResponseXML Implementation = new  SurveyResponseXML(request, SurveyXml);
            FieldNotFoundList = Implementation.ValidateResponseFileds();
            WrongFieldTypeList = Implementation.ValidateResponseFiledTypes();
            MessageList = MessageList.Union(FieldNotFoundList).Union(WrongFieldTypeList).ToDictionary(k => k.Key, v => v.Value);
            return MessageList;


            }
        public List<SurveyInfoBO> GetSurveyInfo( PreFilledAnswerRequest request)
            {
            
            List<string> SurveyIdList = new List<string>();
            string SurveyId =request.AnswerInfo.SurveyId.ToString();
            string OrganizationId = request.AnswerInfo.OrganizationKey.ToString();
            Guid UserPublishKey = request.AnswerInfo.UserPublishKey;
            List<SurveyInfoBO> SurveyBOList = new List<SurveyInfoBO>();


           
            SurveyIdList.Add(SurveyId);
 
            SurveyInfoRequest pRequest = new SurveyInfoRequest();
            var criteria = pRequest.Criteria as SurveyInfoCriteria;

             IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
             ISurveyInfoDao surveyInfoDao = entityDaoFactory.SurveyInfoDao;
             SurveyInfo implementation = new  SurveyInfo(surveyInfoDao);

            SurveyBOList = implementation.GetSurveyInfo(SurveyIdList, criteria.ClosingDate, OrganizationId, criteria.SurveyType, criteria.PageNumber, criteria.PageSize);//Default 
               
            return SurveyBOList;

            }
        public  PreFilledAnswerResponse SetSurveyAnswer( PreFilledAnswerRequest request)
            {
            string SurveyId = request.AnswerInfo.SurveyId.ToString();
            string OrganizationId = request.AnswerInfo.OrganizationKey.ToString();
            Guid UserPublishKey = request.AnswerInfo.UserPublishKey;
            Dictionary<string, string> ErrorMessageList = new Dictionary<string, string>();
            PreFilledAnswerResponse response;
            

            SurveyResponseBO SurveyResponse = new SurveyResponseBO();
            UserAuthenticationRequestBO UserAuthenticationRequestBO = new  UserAuthenticationRequestBO();

            bool IsValidOrgKeyAndPublishKey = IsSurveyInfoValidByOrgKeyAndPublishKey(SurveyId, OrganizationId, UserPublishKey);


            if (IsValidOrgKeyAndPublishKey)
                {
                //Get Survey Info (MetaData)
                List<SurveyInfoBO> SurveyBOList = GetSurveyInfo(request);
                //Build Survey Response Xml

                string Xml = CreateResponseXml(request, SurveyBOList);
                //Validate Response values
                var responseid = request.AnswerInfo.SurveyQuestionAnswerList.Where(x => x.Key.ToLower() == "responseid" || x.Key.ToLower() == "id").FirstOrDefault();
                if (responseid.Key != null)
                {
                    request.AnswerInfo.SurveyQuestionAnswerList.Remove(responseid.Key);
                }

                var updatedtime = request.AnswerInfo.SurveyQuestionAnswerList.Where(x => x.Key.ToLower() == "_updatestamp").FirstOrDefault();
                 if (updatedtime.Key != null)
                 {
                  request.AnswerInfo.SurveyQuestionAnswerList.Remove(updatedtime.Key);
                 }
                ErrorMessageList = ValidateResponse(SurveyBOList, request);

                if (ErrorMessageList.Count() > 0)
                    {
                        response = new PreFilledAnswerResponse();
                        response.ErrorMessageList = ErrorMessageList;
                        response.ErrorMessageList.Add("SurveyId", SurveyId);
                        if (responseid.Value != null)
                        {
                            response.ErrorMessageList.Add("ResponseId", responseid.Value);                        
                        }
                        response.Status = ((Message)1).ToString();
                        InsertErrorLog(response.ErrorMessageList);
                       
                    }               
                    //Insert Survey Response                   
                    if (responseid.Value == null && updatedtime.Value==null)
                    {
                        SurveyResponse = InsertSurveyResponse(Mapper.ToBusinessObject(Xml, request.AnswerInfo.SurveyId.ToString()));
                    }
                    else
                    {//Insert Survey Response
                        SurveyResponseBO surveyresponseBO = new SurveyResponseBO();
                        surveyresponseBO.SurveyId = request.AnswerInfo.SurveyId.ToString();
                        surveyresponseBO.ResponseId = responseid.Value.ToString();
                        surveyresponseBO.XML = Xml;
                        surveyresponseBO.Status = 2;
                        surveyresponseBO.RecordSourceId = (int)ValidationRecordSourceId.MA;
                        System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                        if (updatedtime.Value != null)
                        {
                            surveyresponseBO.DateUpdated = dateTime.AddMilliseconds(Convert.ToDouble(updatedtime.Value.ToString())).ToLocalTime();                           
                        }
                        else
                        {
                            surveyresponseBO.DateUpdated = DateTime.Now;
                        }
                        surveyresponseBO.DateCreated = surveyresponseBO.DateUpdated;
                        surveyresponseBO.DateCompleted = surveyresponseBO.DateUpdated;
                        SurveyResponse = InsertSurveyResponse(surveyresponseBO);
                    }

                    //Save PassCode

                    UserAuthenticationRequestBO =  Mapper.ToBusinessObject(SurveyResponse.ResponseId);
                    SavePassCode(UserAuthenticationRequestBO);

                    //return Response
                    string ResponseUrl = ConfigurationManager.AppSettings["ResponseURL"];
                    response = new  PreFilledAnswerResponse( Mapper.ToDataTransferObjects(UserAuthenticationRequestBO));
                    response.SurveyResponseUrl = ResponseUrl + UserAuthenticationRequestBO.ResponseId;
                    response.Status = ((Message)2).ToString(); 
                   
                }
            else
                {
                PassCodeDTO DTOList = new   PassCodeDTO();
                response = new PreFilledAnswerResponse(DTOList);
                ErrorMessageList.Add("Keys", "Organization key and/or User publish key are invalid.");
                response.ErrorMessageList = ErrorMessageList;
                response.Status = ((Message)1).ToString();
                InsertErrorLog(response.ErrorMessageList);
            }
            return response;
            }
        private bool IsSurveyInfoValidByOrgKeyAndPublishKey(string SurveyId, string Orgkey, Guid publishKey)
            {
            IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
            ISurveyInfoDao surveyInfoDao = entityDaoFactory.SurveyInfoDao;

            string EncryptedKey = Cryptography.Encrypt(Orgkey.ToString());
            List<SurveyInfoBO> result = surveyInfoDao.GetSurveyInfoByOrgKeyAndPublishKey(SurveyId.ToString(), EncryptedKey, publishKey);


            if (result != null && result.Count > 0)
                {
                return true;
                }
            else
                {
                return false;
                }
            }

        public void UpdateRecordStatus(SurveyResponseBO pValue)
        {
            SurveyResponseBO result = pValue;
            this.SurveyResponseDao.UpdateRecordStatus(pValue);
            
        }

        //SurveyResponseBO
        public static PageInfoBO GetSurveySize(List<SurveyResponseBO> resultRows, int BandwidthUsageFactor, int ResponseMaxSize = -1)
        {

            PageInfoBO result = new PageInfoBO();

            int NumberOfRows = 0;
            int ResponsesTotalsize = 0;
            decimal AvgResponseSize = 0;
            decimal NumberOfResponsPerPage = 0;

            if (resultRows.Count > 0)
            {
                NumberOfRows = resultRows.Count;
                ResponsesTotalsize = (int)resultRows.Select(x => x.TemplateXMLSize).Sum();

                AvgResponseSize = (int)resultRows.Select(x => x.TemplateXMLSize).Average();
                // NumberOfResponsPerPage = (int)Math.Ceiling((ResponseMaxSize * (BandwidthUsageFactor/100)) / AvgResponseSize);
                if (AvgResponseSize == 0)
                {
                    AvgResponseSize = 1;
                }
                NumberOfResponsPerPage = (int)Math.Ceiling((int)(ResponseMaxSize * (BandwidthUsageFactor * 0.01)) / AvgResponseSize);
                result.PageSize = (int)Math.Ceiling(NumberOfResponsPerPage);
                result.NumberOfPages = (int)Math.Ceiling(NumberOfRows / NumberOfResponsPerPage);
            }



            return result;
        }

        //SurveyInfoBO
        public static PageInfoBO GetSurveySize(List<SurveyInfoBO> resultRows, int BandwidthUsageFactor, int ResponseMaxSize = -1)
        {

            PageInfoBO result = new PageInfoBO();

            int NumberOfRows = 0;
            int ResponsesTotalsize = 0;
            decimal AvgResponseSize = 0;
            decimal NumberOfResponsPerPage = 0;

            if (resultRows.Count > 0)
            {
                NumberOfRows = resultRows.Count;
                ResponsesTotalsize = (int)resultRows.Select(x => x.TemplateXMLSize).Sum();

                AvgResponseSize = (int)resultRows.Select(x => x.TemplateXMLSize).Average();
                // NumberOfResponsPerPage = (int)Math.Ceiling((ResponseMaxSize * (BandwidthUsageFactor / 100)) / AvgResponseSize);

                NumberOfResponsPerPage = (int)Math.Ceiling((int)(ResponseMaxSize * (BandwidthUsageFactor * 0.01)) / AvgResponseSize);

                result.PageSize = (int)Math.Ceiling(NumberOfResponsPerPage);
                result.NumberOfPages = (int)Math.Ceiling(NumberOfRows / NumberOfResponsPerPage);
            }



            return result;
        }


        public void InsertErrorLog(Dictionary<string, string> pValue)
        {            
            this.SurveyResponseDao.InsertErrorLog(pValue);           
        }
    }
}
