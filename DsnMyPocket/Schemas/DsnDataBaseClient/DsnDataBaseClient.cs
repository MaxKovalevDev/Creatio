using System;
using Terrasoft.Core;
using System.Net;
using Terrasoft.Core.DB;
using System.IO;
using Terrasoft.Core.ImageAPI;

namespace Terrasoft.Configuration.DsnPokemonIntegrationService
{
    
    public class DsnDataBaseClient
    {
        UserConnection UserConnection;
        private ImageAPI _imageApi;
        string Type = "d7e5ab09-b2ed-45f5-8966-b69fad0c0b87";

        //�����������
        public DsnDataBaseClient(UserConnection userConnection)
        {
            UserConnection = userConnection;
            _imageApi = new ImageAPI(userConnection);
        }
        /// <summary>
        /// ������ ����� �������� � �� �� �����
        /// </summary>
        /// <param name="name">��� ��������</param>
        /// <returns>���������� id ��������</returns>
        public Guid GetPokemonId(string name)
        {

            var select = new Select(UserConnection)
                .Column("Id")
                .From("DsnPokemons")
                .Where("DsnName").IsEqual(Column.Parameter(name)) as Select;
            return select.ExecuteScalar<Guid>();
        }
        /// <summary>
        /// ������ ����� �������� � �� �� �����
        /// </summary>
        /// 
        /// <returns>���������� ������ �� API ������� ���������</returns>
        public string getUriApi()
        {
            var uri = (string)Terrasoft.Core.Configuration.SysSettings.GetValue(UserConnection, "DsnUriPokemon");
            return uri;
        }
        /// <summary>
        /// ������ ����� �������� � �� �� �����
        /// </summary>
        /// <param name="name">��� ��������</param>
        /// <param name="height">������ ��������</param>
        /// <param name="weight">��� ��������</param>
        /// <param name="imgPokemon">id �������� � ������� sysImage</param>
        public void CreatePokemon(string name, int height, int weight, Guid imgPokemon)
        {
            var pokemonSchema = UserConnection.EntitySchemaManager.GetInstanceByName("DsnPokemons");
            var pokemon = pokemonSchema.CreateEntity(UserConnection);
            pokemon.SetDefColumnValues();
            pokemon.SetColumnValue("Id", Guid.NewGuid());
            pokemon.SetColumnValue("DsnName", name);
            pokemon.SetColumnValue("DsnWeight", weight);
            pokemon.SetColumnValue("DsnHeight", height);
            pokemon.SetColumnValue("DsnLookupTypeId", Type);
            pokemon.SetColumnValue("DsnPokemonPhotoId", imgPokemon);
            pokemon.Save();
        }
        /// <summary>
        /// �������� ����������� �� ������ 
        /// </summary>
        /// <param name="url">������ �� ��������</param>
        /// <returns>���������� ����� ������ � ������� ���������� �������� � ��������� ��������</returns>
        public MemoryStream GetProfilePhotoIdByUrl(string url)
        {
            MemoryStream imageMemoryStream;
            WebRequest imageRequest = WebRequest.Create(url);
            WebResponse webResponse = imageRequest.GetResponse();
            Stream webResponseStream = webResponse.GetResponseStream();
            imageMemoryStream = new MemoryStream();
            webResponseStream.CopyTo(imageMemoryStream);

            return imageMemoryStream;
        }
        /// <summary>
        /// ��������� � ��(sysImage) ��������
        /// </summary>
        /// <param name="imageMemoryStream">���������� ����� �����������</param>
        /// <param name ="imageName">������������ �������� � ��</param>
        /// <returns>���������� guid ����������� �������� </returns>
        public Guid SaveImage(Stream imageMemoryStream, string imageName)
        {
            var imageId = Guid.NewGuid();
            _imageApi.Save(imageMemoryStream, "image/png", imageName, imageId);
            return imageId;
        }
        /// <summary>
        /// ��������� � ��(sysImage) ��������
        /// </summary>
        /// <param  name="nameShema">��� ����� ������</param>
        /// <param  name="namestring">������������ �������������� ������</param>
        /// <returns>���������� �������������� ������ </returns>
        public string GetLocallizableString(string nameShema, string namestring) {

            string title = UserConnection.GetLocalizableString(nameShema, namestring);

            return title;
        }

    }

}