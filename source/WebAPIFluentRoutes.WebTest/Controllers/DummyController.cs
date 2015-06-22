using System;
using System.Collections.Generic;
using System.Web.Http;
using WebAPIFluentRoutes.WebDummy.Model;

namespace WebAPIFluentRoutes.WebDummy.Controllers
{
    [RoutePrefix("")]
    public class DummyController : ApiController
    {
        private readonly IRouteFinder routeFinder = new RouteFinder();

        [HttpGet, Route("")]
        public IDictionary<string, Uri> Get()
        {
            return new Dictionary<string, Uri>
            {
                {"Simple number value from call", routeFinder.Link<DummyController>(ControllerContext, c => c.Get(10))},
                {"Simple number value from anonymous object", routeFinder.Link<DummyController>(ControllerContext, c => c.Get(-1), new { number = 10 })},
                {"Simple string value from call", routeFinder.Link<DummyController>(ControllerContext, c => c.Get("test value"))},
                {"Simple string value from anonymous object", routeFinder.Link<DummyController>(ControllerContext, c => c.Get((string)null), new { value = "test value" })},
                {"Complex object value from call", routeFinder.Link<DummyController>(ControllerContext, c => c.Get(new DummyModel { Age = 42, Created = DateTime.UtcNow, Name = "Donald" }))},
                {"Complex object value from anonymous object", routeFinder.Link<DummyController>(ControllerContext, c => c.Get((DummyModel)null), new DummyModel { Age = 42, Created = DateTime.UtcNow, Name = "Donald" })}
            };
        }

        [HttpGet, Route("simple-number")]
        public string Get(int number)
        {
            return Request.RequestUri.ToString();
        }

        [HttpGet, Route("simple-string")]
        public string Get(string value)
        {
            return Request.RequestUri.ToString();
        }

        [HttpGet, Route("complex-object")]
        public string Get([FromUri] DummyModel model)
        {
            return Request.RequestUri.ToString();
        }
    }
}
