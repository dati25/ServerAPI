﻿using Microsoft.AspNetCore.Mvc;
using SeverAPI.Commands;
using SeverAPI.Commands.ConfigCommands;
using SeverAPI.Results.ConfigResults;
using SeverAPI.Database.Models;
using ZstdNet;
using SeverAPI.Commands.AdminCommands;
using SeverAPI.Results.TaskResults;

namespace SeverAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConfigController : ControllerBase
{
    private MyContext context = new MyContext();
    
    [HttpGet]
    public IActionResult Get(int? count, int offset = 0)
    {
        CommandsGetDelete command = new CommandsGetDelete();
        List<ConfigResultGet> list = new List<ConfigResultGet>();

        context.Configs!.ToList().ForEach(x => list.Add(new ConfigResultGet(x)));

        List<ConfigResultGet> results = command.Get(list, count, offset);

        return Ok(results);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        Config? config = context.Configs!.Find(id);

        if (config == null)
            return NotFound("Object doesn't exist.");

        ConfigResultGet result = new ConfigResultGet(config);

        return Ok(result);
    }

    [HttpGet("/api/tasks/{idPC}")]
    public IActionResult GetConfigsFromPCid(int idPC)
    {
        CommandsGetDelete command = new CommandsGetDelete();
        List<int> results = command.GetConfigsFromidPC(idPC);
        if (results == null)
            return NotFound("No configs found.");

        return Ok(results);
    }

    [HttpPost]
    public IActionResult Post([FromBody] ConfigCommandPost command)
    {
        ConfigTestCommands testCommands = new ConfigTestCommands();

        var exceptions = testCommands.CheckConfig(command);

        if (exceptions != null)
            return BadRequest(exceptions);

        command.Execute();
        return Ok("Task completed succesfully.");
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] ConfigCommandPut command)
    {
        Config config = command.Execute(id);
        ConfigTestCommands testCommands = new ConfigTestCommands();

        var exceptions = testCommands.CheckConfig(config);
        if (exceptions.Count > 0)
            return BadRequest(exceptions);

        context.SaveChanges();
        return Ok("Task completed succesfully");
    }

    [HttpPut("/api/{idConfig}/{idPC}")]
    public IActionResult UploadSnapshot(int idConfig, int idPC, [FromBody] TaskResultPut Snapshot)
    {
        Tasks task = this.context.Tasks!.Where(x => x.IdConfig == idConfig && x.IdPc == idPC).First();
        task.Snapshot = Snapshot.Snapshot;
        this.context.SaveChanges();
        return Ok("Task completed succesfully");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        CommandsGetDelete command = new CommandsGetDelete();
        if (!command.Delete(this.context.Configs!.Find(id)!))
            return BadRequest("Object doesn't exist");

        return Ok("Task completed succesfully");
    }

    [HttpDelete("/api/sources/{id}")]
    public IActionResult DeleteSource(int id)
    {
        CommandsGetDelete command = new CommandsGetDelete();
        if (!command.Delete(this.context.Sources!.Find(id)!))
            return BadRequest("Object doesn't exist");

        return Ok("Task completed succesfully");
    }

    [HttpDelete("/api/destinations/{id}")]
    public IActionResult DeleteDestination(int id)
    {
        CommandsGetDelete command = new CommandsGetDelete();
        if (!command.Delete(this.context.Destinations!.Find(id)!))
            return BadRequest("Object doesn't exist");

        return Ok("Task completed succesfully");
    }

    [HttpDelete("/api/{idConfig}/{idPC}")]
    public IActionResult DeleteTask(int idConfig, int idPC)
    {
        CommandsGetDelete command = new CommandsGetDelete();

        command.Delete(context.Tasks!.Where(x => x.IdConfig == idConfig && x.IdPc == idPC).First()!);

        return Ok("Task completed succesfully");
    }
}