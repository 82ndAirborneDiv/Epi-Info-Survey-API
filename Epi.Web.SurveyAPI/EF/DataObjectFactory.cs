using System.Configuration;
using System;
using Epi.Web.SurveyAPI.Web.Common.Security;
using Epi.Web.SurveyAPI.EF;

namespace Epi.Web.SurveyAPI.EF
{
    /// <summary>
    /// DataObjectFactory caches the connectionstring so that the context can be created quickly.
    /// </summary>
    public static class DataObjectFactory
    {
        private static readonly string _connectionString;
        public static readonly string _ADOConnectionString;
        /// <summary>
        /// Static constructor. Reads the connectionstring from web.config just once.
        /// </summary>
        static DataObjectFactory()
        {
            try
            {
                //  string connectionStringName = ConfigurationManager.AppSettings.Get("ConnectionStringName");
                string connectionStringName = "SurveyAPIEntities";// "SurveyAPIEntities";
                string AdoConnectionStringName = "EIWSADO";
                //Decrypt connection string here
                _connectionString = Cryptography.Decrypt(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);
                _ADOConnectionString = Cryptography.Decrypt(ConfigurationManager.ConnectionStrings[AdoConnectionStringName].ConnectionString);
            }
            catch (Exception ex) 
                {
                    throw (ex);
                }
        }

        /// <summary>
        /// Creates the Context using the current connectionstring.
        /// </summary>
        /// <remarks>
        /// Gof pattern: Factory method. 
        /// </remarks>
        /// <returns>Action Entities context.</returns>
        public static Epi.Web.SurveyAPI.EF.SurveyAPIEntities CreateContext()
        {
            return new SurveyAPIEntities(_connectionString);
        }
        public static SurveyMetaData CreateSurveyMetaData()
        {
            return new SurveyMetaData();
        }

        public static SurveyResponse CreateSurveyResponse()
        {
            return new SurveyResponse();
        }
    }
}
