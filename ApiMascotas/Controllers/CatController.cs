using ApiMascotas.Models;
using ApiMascotas.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ApiMascotas.Controllers
{
    public class CatController : ApiController
    {
        private static HttpClient client = new HttpClient();

        // Obtiene todas las razas
        // Ejemplo: /api/breed/all
        [HttpGet]
        [Route("api/breed/all")]
        public async Task<IHttpActionResult> GetBreeds()
        {
            var urlApi = $"{Settings.Default.UrlApiCat}/breeds";
            using (var response = await client.GetAsync(urlApi))
            {
                var breeds = await response.Content.ReadAsAsync<List<Breed>>();
                foreach (var breed in breeds)
                {
                    var urlApi2 = $"{Settings.Default.UrlApiCat}/images/search?breed_ids={breed.Id}";
                    using (var response2 = await client.GetAsync(urlApi2))
                    {
                        var images = await response2.Content.ReadAsAsync<List<ImageSearch>>();
                        breed.Image = images[0].Url;
                    }
                }

                return Ok(breeds.OrderBy(x => x.Name));
            }
        }

        // Obtiene una raza en particular por nombre
        // Ejemplo: /api/breed/search?name=Abyssinian
        [HttpGet]
        [Route("api/breed/search")]
        public async Task<IHttpActionResult> GetBreedByName(string name)
        {
            var urlApi = $"{Settings.Default.UrlApiCat}/breeds/search?q={name}";
            using (var response = await client.GetAsync(urlApi))
            {
                var breeds = await response.Content.ReadAsAsync<List<Breed>>();
                var breed = breeds[0];
                var urlApi2 = $"{Settings.Default.UrlApiCat}/images/search?breed_ids={breed.Id}";

                using (var response2 = await client.GetAsync(urlApi2))
                {
                    var images = await response2.Content.ReadAsAsync<List<ImageSearch>>();
                    breed.Image = images[0].Url;
                }

                return Ok(breed);
            }
        }
    }
}
