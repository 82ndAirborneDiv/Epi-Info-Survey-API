
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Epi.Web.SurveyAPI.Models;
using Epi.Web.SurveyAPI.Web.Common.Message;
using Epi.Web.SurveyAPI.Web.Common.BusinessObject;

namespace Epi.Web.SurveyAPI.Repository
{
    public interface ISurveyResponseRepository
    {
         Guid SurveyId
        {
            get; set;
        }

         Guid OrgKey
        {
            get; set;
        }

         Guid PublisherKey
        {
            get; set;
        }        
        PreFilledAnswerResponse SetSurveyAnswer(SurveyAnswerModel item);
        void Remove(string id);
        PreFilledAnswerResponse Update(SurveyAnswerModel item, string ResponseId);
        SurveyInfoBO GetSurveyInfoById(string SurveyId);
    }
}