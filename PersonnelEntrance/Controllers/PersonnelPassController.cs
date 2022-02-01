using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using PersonnelEntrance.DTO;
using System.Linq;
using System.Threading.Tasks;
using PersonnelEntrance.Service;
using System.Net;
using System.Data.SqlClient;

namespace PersonnelEntrance.Controllers
{
    [Route("api/PersonnelPass")]
    [ApiController]
    public class PersonnelPassController : ControllerBase
    {
        private readonly IPersonnelPassRepository _passRepository;

        public PersonnelPassController(IPersonnelPassRepository passRepository)
        {
            _passRepository = passRepository;
        }

        // GET: api/<PersonnelPassController>
        [HttpGet]
        public ActionResult<IEnumerable<PersonnelPassDTO>> GetAll()
        {
            IEnumerable<PersonnelPassDTO> passes = _passRepository.RetrieveAllPersonnelPass();
            if (passes != null)
            {
                if (passes.Any())
                    return Ok(passes);
                else
                    return NoContent();
            }
            else
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    "No employee pass records were found");
        }

        // GET api/<PersonnelPassController>/5
        [HttpGet("{id}")]
        public ActionResult<PersonnelPassDTO> Get(Guid id)
        {
            var PassResult= _passRepository.RetrievePersonnelPass(id);
            if (PassResult != null)
                return Ok(PassResult);
            else 
                return NotFound("Item not found");
        }

        // POST api/<PersonnelPassController>
        [HttpPost]
        public ActionResult<PersonnelPassDTO> Post([FromBody] PersonnelPassDTO personnelPass)
        {
            try {
                Validator<PersonnelPassDTO> validator = 
                    _passRepository.CreatePersonnelPass(personnelPass);
                if (validator.isValid)
                    return Ok(validator.item);
                else
                    return BadRequest(validator.errorMessage);
            }
            catch (SqlException e)
            {
                switch (e.Number)
                {
                    // Unique constraint error
                    case 2627:
                        return BadRequest($"A record for {personnelPass.personnelName} at" +
                            $" time {personnelPass.passTime} already exists.");

                    // Duplicated key row error
                    case 2601:
                        return BadRequest("Duplicate primary key...");

                    case 50000:
                        return BadRequest(e.Message);

                    default:
                        throw ;
                }
            }
        }

        // PUT api/<PersonnelPassController>/5
        [HttpPut("{id}")]
        public ActionResult<PersonnelPassDTO> Put(Guid? id, [FromBody] PersonnelPassDTO personnelPass)
        {
            try
            {
                Guid id2 = id.GetValueOrDefault();
                Validator<PersonnelPassDTO> validator =
                    _passRepository.UpdatePersonnelPass(id2, personnelPass);
                if (validator.isValid)
                    return Ok(validator.item);
                else
                    return BadRequest(validator.errorMessage);
            }
            catch (SqlException e)
            {
                switch (e.Number)
                {
                    // Unique constraint error
                    case 2627:
                        return BadRequest($"A record for {personnelPass.personnelName} at" +
                            $" time {personnelPass.passTime} already exists.");

                    // Duplicated key row error
                    case 2601:
                        return BadRequest("Duplicate primary key...");

                    case 50000:
                        return BadRequest(e.Message);

                    default:
                        throw;
                }
            }
        }

        // DELETE api/<PersonnelPassController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            int result = _passRepository.DeletePersonnelPass(id);
            if (result == 1)
            {
                return Ok();
            }
            else if (result == 0)
                return NotFound();

            return BadRequest();
        }
    }
}
