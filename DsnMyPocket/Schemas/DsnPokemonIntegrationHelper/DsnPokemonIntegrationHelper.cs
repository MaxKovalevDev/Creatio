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
    using System.IO;



    public class DsnPokemonIntegrationHelper

    {
       //���� ����������//

       private readonly DsnDataBaseClient _dbClient;
       private readonly DsnPokemonApiClient _apiClient;
       public ILog log;


        //�����������
        public DsnPokemonIntegrationHelper (UserConnection userConnection)
        {

            _dbClient = new DsnDataBaseClient(userConnection);
            _apiClient = new DsnPokemonApiClient();

        }


        /// <summary>
        /// ��������������� ������, �������� ��������� ������ ��� ������ ��������
        /// </summary>
        /// <param name="name">��� ��������</param>

        /// <returns>���������� ��������� ���������� � �������� ��������</returns>
        public string HelperFunc(string name)
        {

            HttpResponseMessage result;
            DsnPokemonDTO ability;
            MemoryStream imgStream;
            log = LogManager.GetLogger("API Pokemon.co");

            

            if (String.IsNullOrEmpty(name) || (name.Contains("/") == true) || (name.Contains("\\") == true))
            {
                return _dbClient.GetLocallizableString("DsnPokemonsSection", "DsnEnterCorrectName");
            };

            if (_dbClient.GetPokemonId(name) != Guid.Empty)
            {
                return _dbClient.GetLocallizableString("DsnPokemonsSection", "DsnPokemonAlreadyHas");
            };

            var uri = _dbClient.getUriApi();

            try
            {
                result = _apiClient.GetResponse(uri, name);
                log.Info(uri + name);
            }
            catch (Exception)
            {
                return "������ ���������� � API";
                log.Error(result.StatusCode);
                throw;
            }

            if (result.IsSuccessStatusCode)
            {
                var body = result.Content;
                       string responseString = body.ReadAsStringAsync().Result;
                       ability = JsonConvert.DeserializeObject<DsnPokemonDTO>(responseString);
                       
                try
                {
                    imgStream = _dbClient.GetProfilePhotoIdByUrl(ability.sprites.front_default);
                }
                catch (Exception)
                {
                    log.Error("�� ������� �������, ����������� ����������");
                    throw;
                }

                var imgID = _dbClient.SaveImage(imgStream, name);
                _dbClient.CreatePokemon(name, ability.height, ability.weight, imgID);
                return ("������� ������ " + name);
            }

            return _dbClient.GetLocallizableString("DsnPokemonsSection", "DsnPokemonDoenstExist");
        }
        


    }


}