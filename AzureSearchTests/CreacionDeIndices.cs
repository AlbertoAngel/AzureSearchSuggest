using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Reflection.Emit;
using Comun.Modelos;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Rest.Azure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzureSearchTests
{
    [TestClass]
    public class CreacionDeIndices
    {
        [TestMethod]
        public void CreacionDeIndiceUser()
        {
            #region creacion de indice
            // Definimos el indice y creamos la instancia del servicio de busqueda
            Index index = new Index(
                name: "userindex",
                fields: Microsoft.Azure.Search.FieldBuilder.BuildForType<User>());

            string searchServiceName = ConfigurationManager.AppSettings["SearchServiceName"];
            string apiKey = ConfigurationManager.AppSettings["SearchServiceApiKey"];

            var serviceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(apiKey));

            // Creamos el indice
            bool exists =  serviceClient.Indexes.ExistsAsync(index.Name).GetAwaiter().GetResult();
            if (!exists)
            {               
                serviceClient.Indexes.CreateAsync(index).GetAwaiter().GetResult();
            }
            else
            {
                serviceClient.Indexes.DeleteAsync(index.Name).GetAwaiter().GetResult();
                serviceClient.Indexes.CreateAsync(index).GetAwaiter().GetResult();
            }

            #endregion          

            Assert.AreEqual(true, true);
        }

        [TestMethod]
        public void AddFuenteDeDatosUser()
        {
            Index index = new Index(
               name: "userindex",
               fields: Microsoft.Azure.Search.FieldBuilder.BuildForType<User>());

            string searchServiceName = ConfigurationManager.AppSettings["SearchServiceName"];
            string apiKey = ConfigurationManager.AppSettings["SearchServiceApiKey"];

            var serviceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(apiKey));

            #region añadimos fuente de datos

            // configuramos la fuente de datos
            DataSource cosmosDataSource = DataSource.DocumentDb(
               name: "cosmos-db-user",
               documentDbConnectionString: @"AccountEndpoint={};AccountKey={};Database={}",
               collectionName: "User",
               useChangeDetection: true);

            // Asociamos la fuente de datos
            serviceClient.DataSources.CreateOrUpdateAsync(cosmosDataSource).GetAwaiter().GetResult();

            #endregion

            #region creacion del indexer y ejecucion inicial

            // Creamos el indexer
            Indexer indexer = new Indexer(
               name: "cosmos-sql-indexer",
               dataSourceName: cosmosDataSource.Name,
               targetIndexName: index.Name,
               schedule: new IndexingSchedule(TimeSpan.FromHours(1)));

            bool exists = serviceClient.Indexers.ExistsAsync(indexer.Name).GetAwaiter().GetResult();
            if (exists)
            {
                serviceClient.Indexers.ResetAsync(indexer.Name).GetAwaiter().GetResult();
            }

            serviceClient.Indexers.CreateOrUpdateAsync(indexer).GetAwaiter().GetResult();

            // Ejecutamos el indexer inmediatamente y despues se ejecutara cada hora
            try
            {
                serviceClient.Indexers.RunAsync(indexer.Name).GetAwaiter().GetResult();
            }
            catch (CloudException e) when (e.Response.StatusCode == (HttpStatusCode)429)
            {
                Console.WriteLine("Failed to run indexer: {0}", e.Response.Content);
            }

            #endregion
        }


        [TestMethod]
        public void CreacionDeIndiceProduct()
        {
            #region creacion de indice
            // Definimos el indice y creamos la instancia del servicio de busqueda
            Index index = new Index(
               name: "audiencechannelindex",
               fields: Microsoft.Azure.Search.FieldBuilder.BuildForType<User>());

            string searchServiceName = ConfigurationManager.AppSettings["SearchServiceName"];
            string apiKey = ConfigurationManager.AppSettings["SearchServiceApiKey"];

            var serviceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(apiKey));

            // Creamos el indice
            bool exists = serviceClient.Indexes.ExistsAsync(index.Name).GetAwaiter().GetResult();
            if (exists)
            {
                serviceClient.Indexes.DeleteAsync(index.Name).GetAwaiter().GetResult();
            }
            serviceClient.Indexes.CreateAsync(index).GetAwaiter().GetResult();

            #endregion

            #region añadimos fuente de datos
            // configuramos la fuente de datos
            DataSource cosmosDataSource = DataSource.DocumentDb(
               name: "cosmos-db-product",
               documentDbConnectionString: @"AccountEndpoint={};AccountKey={};Database={}",
               collectionName: "AudienceChannel",
               useChangeDetection: true);

            // Asociamos la fuente de datos
            serviceClient.DataSources.CreateOrUpdateAsync(cosmosDataSource).GetAwaiter().GetResult();

            #endregion

            #region creacion del indexer y ejecucion inicial
            // Creamos el indexer
            Indexer indexer = new Indexer(
               name: "cosmos-sql-indexer",
               dataSourceName: cosmosDataSource.Name,
               targetIndexName: index.Name,
               schedule: new IndexingSchedule(TimeSpan.FromHours(1)));

            exists = serviceClient.Indexers.ExistsAsync(indexer.Name).GetAwaiter().GetResult();
            if (exists)
            {
                serviceClient.Indexers.ResetAsync(indexer.Name).GetAwaiter().GetResult();
            }

            serviceClient.Indexers.CreateOrUpdateAsync(indexer).GetAwaiter().GetResult();

            // Ejecutamos el indexer inmediatamente y despues se ejecutara cada hora
            try
            {
                serviceClient.Indexers.RunAsync(indexer.Name).GetAwaiter().GetResult();
            }
            catch (CloudException e) when (e.Response.StatusCode == (HttpStatusCode)429)
            {
                Console.WriteLine("Failed to run indexer: {0}", e.Response.Content);
            }

            #endregion

            Assert.AreEqual(true, true);
        }

        
    }
}
