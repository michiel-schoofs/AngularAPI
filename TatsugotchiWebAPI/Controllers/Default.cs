using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TatsugotchiWebAPI.Controllers {
    [Route("/")]
    [ApiController]
    public class Default : Controller {
        [HttpGet]
        public HttpResponse Get() {
            Response.Redirect("/swagger/index.html");
            return Response;
        }
    }
}
