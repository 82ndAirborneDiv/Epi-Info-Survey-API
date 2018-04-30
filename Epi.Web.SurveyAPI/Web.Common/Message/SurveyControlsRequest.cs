using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using Epi.Web.SurveyAPI.Web.Common.MessageBase;

namespace Epi.Web.SurveyAPI.Web.Common.Message
    {
     [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class SurveyControlsRequest : ResponseBase 
        {
         [DataMember]
         public string SurveyId;
        }
    }
