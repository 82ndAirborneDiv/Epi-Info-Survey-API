﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.SurveyAPI.Web.Common.DTO;
using System.Runtime.Serialization;

namespace Epi.Web.SurveyAPI.Web.Common.Message
{
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class OrganizationRequest : MessageBase.RequestBase
    {

        public OrganizationRequest()
        {
            this.Organization = new OrganizationDTO();
        }

        /// <summary>
        /// Organization object.
        /// </summary>
        [DataMember]
        public OrganizationDTO Organization;
        [DataMember]
        public Guid AdminSecurityKey;
    }
}
