﻿using Tutorial2TareasMVC.Models;

namespace Tutorial2TareasMVC.Services
{
    public class LocalStoreService : IStoreService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocalStoreService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            this._env = env;
            this._httpContextAccessor = httpContextAccessor;
        }
        public async Task<AlmacenarArchivoResultado[]> Almacenar(string contenedor, IEnumerable<IFormFile> archivos)
        {
            var tareas = archivos.Select(async archivo =>
            {
                var nombreArchivoOriginal = Path.GetFileName(archivo.FileName);
                var extension = Path.GetExtension(archivo.FileName);
                var nombreArchivo = $"{Guid.NewGuid()}{extension}";
                string folder = Path.Combine(_env.WebRootPath, contenedor);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string ruta = Path.Combine(folder, nombreArchivo);
                using (var ms = new MemoryStream())
                {
                    await archivo.CopyToAsync(ms);
                    var contenido = ms.ToArray();
                    await File.WriteAllBytesAsync(ruta, contenido);
                }
                var url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
                var urlArchivo = Path.Combine(url, contenedor, nombreArchivo).Replace("\\","/");
                return new AlmacenarArchivoResultado
                {
                    URL = urlArchivo,
                    Titulo = nombreArchivoOriginal
                };
            });
            var resultados = await Task.WhenAll(tareas);
            return resultados;
        }

        public Task Borrar(string ruta, string contenedor)
        {
            if (string.IsNullOrWhiteSpace(ruta))
            {
                return Task.CompletedTask;
            }
            var nombreArchivo = Path.GetFileName(ruta);
            var directorioArchivo = Path.Combine(_env.WebRootPath, contenedor, nombreArchivo);
            if (File.Exists(directorioArchivo))
            {
                File.Delete(directorioArchivo);
            }
            return Task.CompletedTask;
        }
    }
}
