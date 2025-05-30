// JaveragesLibrary/Services/Features/Mangas/MangaService.cs
using JaveragesLibrary.Domain.Entities; // ¡Necesitamos acceso a nuestra entidad Manga!
using System.Collections.Generic;       // Para usar List<T>
using System.Linq;                      // Para usar LINQ (FirstOrDefault, etc.)

namespace JaveragesLibrary.Services.Features.Mangas;

public class MangaService
{
    // Nuestra "base de datos" en memoria por ahora.
    // La hacemos 'static' para que sea compartida entre instancias si el servicio es Scoped,
    // o simplemente una por instancia si es Singleton (lo veremos luego).
    // Para un ejemplo inicial simple, vamos a hacerla no-static y veremos el efecto de los tiempos de vida del servicio.
    private readonly List<Manga> _mangas;
    private int _nextId = 1; // Para generar IDs simples

    public MangaService()
    {
        _mangas = new List<Manga>();
        // Podríamos añadir algunos mangas de ejemplo aquí para pruebas iniciales
        // Add(new Manga { Title = "Naruto", Author = "Masashi Kishimoto", PublicationDate = new DateTime(1999, 9, 21), Volumes = 72, IsOngoing = false, Genre = "Shonen" });
        // Add(new Manga { Title = "One Piece", Author = "Eiichiro Oda", PublicationDate = new DateTime(1997, 7, 22), Volumes = 100, IsOngoing = true, Genre = "Shonen" }); // ¡Añade el Id al crear!
    }

    // Operaciones CRUD (Create, Read, Update, Delete)

    // READ All
    public IEnumerable<Manga> GetAll()
    {
        return _mangas;
    }

    // READ by Id
    public Manga? GetById(int id) // Devolvemos Manga? para indicar que podría no encontrarse
    {
        return _mangas.FirstOrDefault(manga => manga.Id == id);
    }

    // CREATE
    public Manga Add(Manga manga)
    {
        manga.Id = _nextId++; // Asignamos un nuevo ID
        _mangas.Add(manga);
        return manga; // Devolvemos el manga creado (con su ID)
    }

    // UPDATE
    public bool Update(Manga mangaToUpdate)
    {
        var existingManga = _mangas.FirstOrDefault(m => m.Id == mangaToUpdate.Id);
        if (existingManga != null)
        {
            // Actualizamos las propiedades del manga existente
            existingManga.Title = mangaToUpdate.Title;
            existingManga.Author = mangaToUpdate.Author;
            existingManga.Genre = mangaToUpdate.Genre;
            existingManga.PublicationDate = mangaToUpdate.PublicationDate;
            existingManga.Volumes = mangaToUpdate.Volumes;
            existingManga.IsOngoing = mangaToUpdate.IsOngoing;
            return true; // Indicamos que la actualización fue exitosa
        }
        return false; // Indicamos que no se encontró el manga para actualizar
    }

    // DELETE
    public bool Delete(int id)
    {
        var mangaToRemove = _mangas.FirstOrDefault(manga => manga.Id == id);
        if (mangaToRemove != null)
        {
            _mangas.Remove(mangaToRemove);
            return true; // Indicamos que la eliminación fue exitosa
        }
        return false; // Indicamos que no se encontró el manga para eliminar
    }
}