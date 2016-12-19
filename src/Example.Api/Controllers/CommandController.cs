using Example.Events;
using System.Web.Http;
using Example.Commands;
using Library.Interfaces;

namespace Example.Api.Controllers
{
    public class CommandController : ApiController
    {
        private readonly IAggregates<EventBase> _aggregates;

        public CommandController(IAggregates<EventBase> aggregates)
        {
            _aggregates = aggregates;
        }

        [Route("company/commands/createcompany")]
        [HttpPost]
        public IHttpActionResult CreateCompany(CreateCompany command)
        {
            var company = _aggregates.Read<CompanyAggregate>(command.Id);

            if (!string.IsNullOrEmpty(company.Id))
            {
                return BadRequest($"Company with id {command.Id} already exists");
            }

            company = new CompanyAggregate();
            company.CreateCompany(command.Id, command.Name, command.Category);

            _aggregates.Commit(company);

            return Ok();
        }

        [Route("company/commands/updatecompanyname")]
        [HttpPost]
        public IHttpActionResult UpdateCompanyName(UpdateCompanyName command)
        {
            var company = _aggregates.Read<CompanyAggregate>(command.Id);

            company.UpdateName(command.Id, command.NewName);

            _aggregates.Commit(company);

            return Ok();
        }
    }
}
