using Microsoft.AspNetCore.Mvc;
using SeverAPI.Commands.ComputerCommands;
using SeverAPI.Results.ComputerResults;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SeverAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComputerController : ControllerBase
    {
        // GET: api/<ComputerController>
        [HttpGet]
        public IActionResult Get()
        {
            ComputerCommandGet command = new ComputerCommandGet();
            List<ComputerResultGet> results = command.Execute();

            if (results == null)
                return NotFound("No objects found.");

            return Ok(results);
        }

        // GET api/<ComputerController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            ComputerCommandSearchedGet command = new ComputerCommandSearchedGet();
            ComputerResultGet result = command.Execute(id);

            if (result == null)
                return NotFound("Searched object doesn't exist.");

            return Ok(result);
        }

        // POST api/<ComputerController>
        [HttpPost]
        public IActionResult Post([FromBody] ComputerResultPost computerResult)
        {
            ComputerCommandPost command = new ComputerCommandPost();

            if (command.Execute(computerResult) == null)
                return NotFound("Searched object doesn't exist.");

            return Ok();
        }

        // PUT api/<ComputerController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ComputerCommandPut command)
        {
            command.Execute(id);
            return Ok("Task completed succesfully");
        }

        // DELETE api/<ComputerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            ComputerCommandDelete command = new ComputerCommandDelete();

            command.Execute(id);
        }
    }
}
