using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Trippin_Website.Logic_classes;

namespace Trippin_Website.Controllers
{
    public class ToolsController : Controller
    {

        public HttpClient client { get; set; }

        public ToolsController()
        {
            client = new HttpClient();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ObtineUrlTemporar(TemporaryLinkResponse model, HttpPostedFileBase file)
        {
            var fileStream = file.InputStream;
            var fileInfo = new FileInfo(file.FileName);

            try
            {
                using (var form = new MultipartFormDataContent())
                {
                    form.Add(new StreamContent(fileStream), "file", fileInfo.Name);
                    form.Add(new StringContent(model.expires + "d"), "expires");
                    form.Add(new StringContent("1"), "maxDownloads");
                    form.Add(new StringContent("true"), "autoDelete");

                    using (var response = await client.PostAsync("https://file.io", form))
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonConvert.DeserializeObject<TemporaryLinkResponse>(responseBody);

                        ViewBag.MaxDownloads = responseObject.maxDownloads;
                        ViewBag.Link = responseObject.link;
                        ViewBag.Expires = responseObject.expires;
                        ViewBag.AutoDelete = responseObject.autoDelete;

                        return View("LinkTemporar");
                    }
                }
            }

            catch (Exception ex)
            {
                ViewBag.Eroare = $@"A aparut o eroare, daca eroarea persista va rog contactati suportul. Eroare : {ex.Message}
stack tracke : {ex.StackTrace}";
                return View("LinkTemporar");
            }
        }

        public ActionResult LinkTemporar()
        {
            var model = new TemporaryLinkResponse();

            return View(model);
        }
        public ActionResult YoutubeDownload()
        {

            return View();
        }

        public ActionResult AnalizeazaSunet()
        {
            return View();
        }
    }
}