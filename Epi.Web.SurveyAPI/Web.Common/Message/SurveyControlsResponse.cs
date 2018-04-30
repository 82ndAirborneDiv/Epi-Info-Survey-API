using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using Epi.Web.SurveyAPI.Web.Common.DTO;

namespace Epi.Web.SurveyAPI.Web.Common.Message
    {
     [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class SurveyControlsResponse : MessageBase.ResponseBase 
        {
         [DataMember]
         public string Message;
         [DataMember]
         public List<SurveyControlDTO> SurveyControlList;

        }
    }
