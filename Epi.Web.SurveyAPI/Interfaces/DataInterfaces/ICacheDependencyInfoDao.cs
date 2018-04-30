using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.SurveyAPI.Web.Common.BusinessObject;

namespace Epi.Web.SurveyAPI.Interfaces.DataInterfaces
{
    public interface ICacheDependencyInfoDao
    {
        List<CacheDependencyBO> GetCacheDependencyInfo();
        List<CacheDependencyBO> GetCacheDependencyInfo(List<string> surveyKeys);
    }
}
