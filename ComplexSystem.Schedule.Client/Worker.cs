using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ComplexSystem.Schedule.Client.Models.GoogleApi.Docs;
using ComplexSystem.Schedule.Client.Models.ResourceApi.Gallery;
using ComplexSystem.Schedule.Client.RefitClients.GoogleApi;
using ComplexSystem.Schedule.Client.RefitClients.ResourceApi;
using Newtonsoft.Json;

namespace ComplexSystem.Schedule.Client
{
    internal class Worker
    {
        private readonly IGalleryApiClient _galleryApiClient;
        private readonly IUserApiClient _userApiClient;
        private readonly IGoogleDocsApiService _googleDocsApiService;

        public Worker(IGalleryApiClient galleryApiClient,
                      IUserApiClient userApiClient,
                      IGoogleDocsApiService googleDocsApiService)
        {
            _galleryApiClient = galleryApiClient;
            _userApiClient = userApiClient;
            _googleDocsApiService = googleDocsApiService;
        }

        public async Task Start(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await CreateGallery();
                await CreateGoogleDoc();

                await Task.Delay(3000, cancellationToken);
            }
        }

        private async Task CreateGallery()
        {
            var galleryId = Guid.NewGuid().ToString();
            var userId = "198fc671-d84d-42e4-88d3-913af4290b11";

            var galleryResponse = await _galleryApiClient.CreateGallery(new Gallery
            {
                Id = galleryId,
                Name = "Name_" + DateTime.UtcNow.ToString("o"),
                UserId = userId
            });

            var userResponse = await _userApiClient.GetUser(userId);

            var isGalleryCreated = userResponse.Content.Galleries.Any(g => g.Id == galleryId);

            Console.WriteLine($"Gallery is added: {isGalleryCreated}");
            Console.WriteLine($"Gallery: {JsonConvert.SerializeObject(galleryResponse.Content, Formatting.Indented)}");
        }

        private async Task CreateGoogleDoc()
        {
            var docId = Guid.NewGuid().ToString();

            var createdResponse = await _googleDocsApiService.CreateDocument(new Document
            {
                Id = docId,
                Title = $"Title_{DateTime.UtcNow}"
            });

            var documentResponse = await _googleDocsApiService.GetDocument(docId);

            var isDocumentCreated = documentResponse.IsSuccessStatusCode && documentResponse.Content.Id == docId;

            Console.WriteLine($"Document is created: {isDocumentCreated}");
            Console.WriteLine($"Document: {JsonConvert.SerializeObject(documentResponse.Content, Formatting.Indented)}");
        }
    }
}
