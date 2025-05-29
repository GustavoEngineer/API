// JaveragesLibrary/Controllers/V1/MangaController.cs
using JaveragesLibrary.Domain.Entities;
using JaveragesLibrary.Services.Features.Mangas; // ¡Necesitamos nuestro servicio!
using Microsoft.AspNetCore.Mvc;                 // Para [ApiController], [Route], IActionResult, etc.

namespace JaveragesLibrary.Controllers.V1;

[ApiController] // Indica que esta clase es un controlador de API
[Route("api/v1/[controller]")] // Define la ruta base: "api/v1/manga"
public class MangaController : ControllerBase // Base para controladores de API
{
    private readonly MangaService _mangaService;

    // ¡Inyección de Dependencias en acción! ASP.NET Core nos dará una instancia de MangaService.
    public MangaController(MangaService mangaService)
    {
        _mangaService = mangaService;
    }

    // GET api/v1/manga
    [HttpGet]
    public IActionResult GetAll()
    {
        var mangas = _mangaService.GetAll();
        return Ok(mangas); // 200 OK con la lista de mangas
    }

    // GET api/v1/manga/{id}
    [HttpGet("{id:int}")] // Restricción de ruta: id debe ser un entero
    public IActionResult GetById([FromRoute] int id) // [FromRoute] es opcional aquí, pero explícito
    {
        var manga = _mangaService.GetById(id);
        if (manga == null)
        {
            return NotFound(new { Message = $"Manga con ID {id} no encontrado." }); // 404 Not Found
        }
        return Ok(manga); // 200 OK con el manga
    }

    // POST api/v1/manga
    [HttpPost]
    public IActionResult Add([FromBody] Manga manga) // [FromBody] indica que el manga viene en el cuerpo de la petición
    {
        if (!ModelState.IsValid) // Validaciones básicas (ej: si 'required' no se cumple)
        {
            return BadRequest(ModelState);
        }
        var newManga = _mangaService.Add(manga);
        // 201 Created. Devuelve la URL para obtener el nuevo recurso y el recurso mismo.
        return CreatedAtAction(nameof(GetById), new { id = newManga.Id }, newManga);
    }

    // PUT api/v1/manga/{id}
    [HttpPut("{id:int}")]
    public IActionResult Update([FromRoute] int id, [FromBody] Manga mangaToUpdate)
    {
        if (id != mangaToUpdate.Id)
        {
            return BadRequest(new { Message = "El ID de la ruta no coincide con el ID del cuerpo." }); // 400 Bad Request
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var success = _mangaService.Update(mangaToUpdate);
        if (!success)
        {
            return NotFound(new { Message = $"Manga con ID {id} no encontrado para actualizar." }); // 404 Not Found
        }
        return NoContent(); // 204 No Content (éxito, pero no hay nada que devolver en el cuerpo)
    }

    // DELETE api/v1/manga/{id}
    [HttpDelete("{id:int}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var success = _mangaService.Delete(id);
        if (!success)
        {
            return NotFound(new { Message = $"Manga con ID {id} no encontrado para eliminar." }); // 404 Not Found
        }
        return NoContent(); // 204 No Content
    }
}