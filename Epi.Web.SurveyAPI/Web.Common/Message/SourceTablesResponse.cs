
using System.Collections.Generic;
using System.Runtime.Serialization;

using Epi.Web.SurveyAPI.Web.Common.MessageBase;
using Epi.Web.SurveyAPI.Web.Common.DTO;


namespace Epi.Web.SurveyAPI.Web.Common.Message
{
     [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class SourceTablesResponse : RequestBase
    {
         public SourceTablesResponse() {
             this.List = new List<SourceTableDTO>();
         }

          
         public List<SourceTableDTO> List{get;set;}
    }
}
