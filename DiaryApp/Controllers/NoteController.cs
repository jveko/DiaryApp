using System.Linq.Expressions;
using AutoMapper;
using DiaryApp.Entities;
using DiaryApp.Interfaces;
using DiaryApp.Models;
using DiaryApp.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DiaryApp.Controllers;

[ApiController]
[Route("notes")]
public class NoteController : ControllerBase
{
    private readonly INoteService _service;
    private readonly IMapper _mapper;

    public NoteController(INoteService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    private static IActionResult NoteNotFound()
    {
        return new NotFoundObjectResult(new
        {
            message = "Note Doesn't Exist"
        });
    }
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Search([FromQuery] PaginatedParamModel<int> paramModel)
    {
        try
        {
            var notes = await _service.GetPagedNote(paramModel, 1);
            return new OkObjectResult(notes);
        }
        catch (Exception e)
        {
            return new InternalServerErrorResult("Failed to Get Notes");
        }
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> Create(NoteParamCreateModel model)
    {
        try
        {
            var note = _mapper.Map<Note>(model);
            await _service.AddNote(note, 1);
            return new ObjectCreatedResult(new
            {
                note.Id
            });
        }
        catch (Exception e)
        {
            return new InternalServerErrorResult("Failed to Create Note");
        }
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> Detail(int id)
    {
        try
        {
            var note = await _service.GetNote(id, new Expression<Func<Note, object>>[] {x => x.User});
            return new OkObjectResult(new
            {
                message = "Success",
                data = _mapper.Map<NoteDto>(note)
            });
        }
        catch (Exception e)
        {
            return new InternalServerErrorResult("Failed to Get Detail Note");
        }
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromBody] NoteParamUpdateModel model, int id)
    {
        try
        {
            var note = await _service.GetNote(id);
            if (note == null) return NoteNotFound();
            note = _mapper.Map<Note>(model);
            await _service.UpdateNote(note);
            return new OkObjectResult(new
            {
                message = "Success Update Note",
                data = _mapper.Map<NoteDto>(note)
            });
        }
        catch (Exception e)
        {
            return new InternalServerErrorResult("Failed to Update Note");
        }
    }
    
    [HttpPut]
    [Route("/archive/{id:int}")]
    public async Task<IActionResult> ArchiveNote(int id)
    {
        try
        {
            var note = await _service.GetNote(id);
            if (note == null) return NoteNotFound();
            await _service.ArchiveNote(note);
            return new OkObjectResult(new
            {
                message = $"Success Note {(note.IsArchive ? "Archived" : "UnArchived")}"
            });
        }
        catch (Exception e)
        {
            return new InternalServerErrorResult("Failed to Archive Note");
        }
    }
}