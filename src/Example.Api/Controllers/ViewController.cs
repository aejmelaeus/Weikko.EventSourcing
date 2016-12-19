using System.Web.Http;
using Example.Events;
using Library.Interfaces;

namespace Example.Api.Controllers
{
    public class ViewController : ApiController
    {
        private readonly IProjections<EventBase> _projections;

        public ViewController(IProjections<EventBase> projections)
        {
            _projections = projections;
        }

        [Route("company/{id}")]
        [HttpGet]
        public IHttpActionResult GetCompany(string id)
        {
            return Ok(_projections.Read<CompanyView>(id));
        }

        [Route("company/{id}/names")]
        [HttpGet]
        public IHttpActionResult GetCompanyNames(string id)
        {
            return Ok(_projections.Read<CompanyNamesView>(id));
        }

        [Route("company/{id}/names/rebuild")]
        [HttpGet]
        public IHttpActionResult RebuildCompanyNames(string id)
        {
            _projections.Rebuild<CompanyNamesView>(id);
            return Ok();
        }
    }
}
