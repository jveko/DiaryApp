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
    
    private static IActionResult NoteOwnerNotMatch()
    {
        return new BadRequestObjectResult(new
        {
            message = "Note is not yours"
        });
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetNotes([FromQuery] PaginatedParamModel<int> paramModel)
    {
        var user = (User) HttpContext.Items["User"]!;
        var pagedNote = await _service.GetPagedNote(paramModel, user.Id);
        return new OkObjectResult(new
        {
            totalCount = pagedNote.TotalCount,
            filteredCount = pagedNote.FilteredCount,
            pageData = _mapper.Map<List<NoteGetAllDto>>(pagedNote.PageData)
        });
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> PostNote(NoteParamPostModel model)
    {
        var user = (User) HttpContext.Items["User"]!;
        var note = _mapper.Map<Note>(model);
        await _service.AddNote(note, user.Id);
        return new ObjectCreatedResult(new
        {
            note.Id
        });
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetNoteById(int id)
    {
        var user = (User) HttpContext.Items["User"]!;
        var note = await _service.GetNote(id, new Expression<Func<Note, object>>[] {x => x.User});
        if (note == null) return NoteNotFound();
        if (!note.OwnerId.Equals(user.Id)) return NoteOwnerNotMatch();
        return new OkObjectResult(new
        {
            message = "Success",
            data = _mapper.Map<NoteGetByIdDto>(note)
        });
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> PutNoteById([FromBody] NoteParamPutModel model, int id)
    {
        var user = (User) HttpContext.Items["User"]!;
        var note = await _service.GetNote(id);
        if (note == null) return NoteNotFound();
        if (!note.OwnerId.Equals(user.Id)) return NoteOwnerNotMatch();
        await _service.UpdateNote(note, model);
        return new OkObjectResult(new
        {
            message = "Success Update Note",
            data = _mapper.Map<NotePutByIdDto>(note)
        });
    }

    [HttpPut]
    [Route("archive/{id:int}")]
    public async Task<IActionResult> PutArchiveNoteById(int id)
    {
        var user = (User) HttpContext.Items["User"]!;
        var note = await _service.GetNote(id);
        if (note == null) return NoteNotFound();
        if (!note.OwnerId.Equals(user.Id)) return NoteOwnerNotMatch();
        await _service.ArchiveNote(note);
        return new OkObjectResult(new
        {
            message = $"Success Note {(note.IsArchive ? "Archived" : "UnArchived")}"
        });
    }
}