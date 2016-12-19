using System.Web.Http;
using Example.Events;
using Library.Interfaces;

namespace Example.Api.Controllers
{
    public class ViewController : ApiController
    {
        private readonly IProjectionRepository<EventBase> _projectionRepository;

        public ViewController(IProjectionRepository<EventBase> projectionRepository)
        {
            _projectionRepository = projectionRepository;
        }

        [Route("company/{id}")]
        [HttpGet]
        public IHttpActionResult GetCompany(string id)
        {
            return Ok(_projectionRepository.Read<CompanyView>(id));
        }

        [Route("company/{id}/names")]
        [HttpGet]
        public IHttpActionResult GetCompanyNames(string id)
        {
            return Ok(_projectionRepository.Read<CompanyNamesView>(id));
        }

        [Route("company/{id}/names/rebuild")]
        [HttpGet]
        public IHttpActionResult RebuildCompanyNames(string id)
        {
            _projectionRepository.Rebuild<CompanyNamesView>(id);
            return Ok();
        }
    }
}
