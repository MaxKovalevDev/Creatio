namespace Terrasoft.Configuration.DsnPokemonIntegrationService
{
    using System;
    using Terrasoft.Core.ImageAPI;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.ServiceModel.Activation;
    using Terrasoft.Core;
    using Terrasoft.Web.Common;
    using Terrasoft.Core.Entities;
    using Newtonsoft.Json;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Terrasoft.Core.Factories;
    using Terrasoft.Core.DB;
    using System.Data;
    using global::Common.Logging;





    public class DsnPokemonApiClient
    {
        /// <summary>
        /// ���������� ������ �� api ���������
        /// </summary>
        /// <param name="uri">������ ��� ������� � api</param>
        /// <param name="name">��� ��������</param>
        /// <returns>���������� ��������� ������� � API</returns>
        public HttpResponseMessage GetResponse(string uri, string name)
        {
            HttpClient httpClient = new HttpClient();
            var result = httpClient.GetAsync(uri + name).Result;
            return result;

        }
        


    }   
}