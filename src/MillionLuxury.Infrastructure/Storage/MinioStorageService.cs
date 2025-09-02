namespace MillionLuxury.Infrastructure.Storage;

#region Usings
using Microsoft.Extensions.Logging;
using MillionLuxury.Application.Common.Abstractions.Storage;
using MillionLuxury.Domain.File;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
#endregion

public class MinioStorageService : IStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioStorageService> _logger;

    public MinioStorageService(IMinioClient minioClient, ILogger<MinioStorageService> logger )
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene el contenido de un archivo del bucket indicado.
    /// Retorna el contenido en bytes.
    /// </summary>
    public async Task<byte[]> GetFile(File file)
    {
        if (file is null) throw new ArgumentNullException(nameof(file));
        if (string.IsNullOrWhiteSpace(file.NameBucket)) throw new ArgumentException("NameBucket es requerido.");

        // Si no viene Filename (como tu GetFile factory), asumimos que Path ya es la clave completa
        var key = string.IsNullOrWhiteSpace(file.Filename)
            ? NormalizePath(file.Path)
            : BuildObjectKey(file.Path, file.Filename);

        try
        {
            using var memoryStream = new MemoryStream();

            var getArgs = new GetObjectArgs()
                .WithBucket(file.NameBucket)
                .WithObject(key)
                .WithCallbackStream(stream => stream.CopyTo(memoryStream));

            await _minioClient.GetObjectAsync(getArgs);

            _logger.LogInformation("Objeto leído de MinIO. Bucket: {Bucket}, Key: {Key}, Size: {Size} bytes",
                file.NameBucket, key, memoryStream.Length);

            return memoryStream.ToArray();
        }
        catch (ObjectNotFoundException)
        {
            _logger.LogWarning("Objeto no encontrado. Bucket: {Bucket}, Key: {Key}", file.NameBucket, key);
            return [];
        }
        catch (MinioException ex)
        {
            _logger.LogError(ex, "Error de MinIO al obtener objeto. Bucket: {Bucket}, Key: {Key}", file.NameBucket, key);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado al obtener objeto. Bucket: {Bucket}, Key: {Key}", file.NameBucket, key);
            throw;
        }
    }

    //public async Task<byte[]> GetFile(File file)
    //{
    //    try
    //    {
    //        string filename = file.Filename;
    //        string objectName = file.Path;

    //        // Confirm object exists before attemt to get
    //        StatObjectArgs statObjectArgs = new StatObjectArgs()
    //            .WithBucket(file.NameBucket)
    //            .WithObject(objectName);

    //        await _minioClient.
    //          StatObjectAsync(statObjectArgs)
    //          .ConfigureAwait(false);

    //        var tcs = new TaskCompletionSource<byte[]>();

    //        GetObjectArgs getObjectArgs = new GetObjectArgs()
    //            .WithBucket(file.NameBucket)
    //            .WithObject(objectName)
    //            .WithCallbackStream((stream) =>
    //            {
    //                using (var ms = new MemoryStream())
    //                {
    //                    stream.CopyTo(ms);
    //                    tcs.SetResult(ms.ToArray());
    //                }
    //            });

    //        await _minioClient
    //          .GetObjectAsync(getObjectArgs)
    //          .ConfigureAwait(false);

    //        return await tcs.Task.ConfigureAwait(false);

    //    }
    //    catch (InvalidBucketNameException ex)
    //    {
    //        throw new ArgumentException("Invalid bucket name", ex);
    //    }
    //    catch (ConnectionException ex)
    //    {
    //        throw new InvalidOperationException("Connection error", ex);
    //    }
    //    catch (InternalClientException ex)
    //    {
    //        throw new InvalidOperationException("Internal library error", ex);
    //    }
    //    catch (Minio.Exceptions.MinioException ex)
    //    {
    //        throw new KeyNotFoundException("File not found", ex);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception("An error occurred", ex);
    //    }
    //}

    /// <summary>
    /// Guarda un archivo en el bucket indicado.
    /// Se asume que File.Content viene en Base64 y que Mimetype es el Content-Type.
    /// </summary>
    public async Task SaveFile(File file)
    {
        if (file is null) throw new ArgumentNullException(nameof(file));
        if (string.IsNullOrWhiteSpace(file.NameBucket)) throw new ArgumentException("NameBucket es requerido.");
        if (string.IsNullOrWhiteSpace(file.Path) && string.IsNullOrWhiteSpace(file.Filename))
            throw new ArgumentException("Se requiere Path o Filename para construir la clave del objeto.");
        if (string.IsNullOrWhiteSpace(file.Content))
            throw new ArgumentException("Content (Base64) es requerido para guardar el archivo.");

        var key = BuildObjectKey(file.Path, file.Filename);

        try
        {
            await EnsureBucketExistsAsync(file.NameBucket);

            // Si llega con encabezado data URL, córtalo: "data:...;base64,<payload>"
            var base64 = ExtractBase64Payload(file.Content);
            var bytes = Convert.FromBase64String(base64);

            using var ms = new MemoryStream(bytes);

            var putArgs = new PutObjectArgs()
                .WithBucket(file.NameBucket)
                .WithObject(key)
                .WithStreamData(ms)
                .WithObjectSize(ms.Length)
                .WithContentType(string.IsNullOrWhiteSpace(file.Mimetype)
                    ? "application/octet-stream"
                    : file.Mimetype);

            await _minioClient.PutObjectAsync(putArgs);

            _logger.LogInformation("Objeto guardado en MinIO. Bucket: {Bucket}, Key: {Key}, Size: {Size} bytes",
                file.NameBucket, key, ms.Length);
        }
        catch (MinioException ex)
        {
            _logger.LogError(ex, "Error de MinIO al guardar objeto. Bucket: {Bucket}, Key: {Key}", file.NameBucket, key);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado al guardar objeto. Bucket: {Bucket}, Key: {Key}", file.NameBucket, key);
            throw;
        }
    }

    //public async Task SaveFile(File file)
    //{
    //    string mimetype = "application/octet-stream";

    //    try
    //    {
    //        //Create bucket if it doesn't exist.
    //        var beArgs = new BucketExistsArgs().WithBucket(file.NameBucket);
    //        bool found = await _minioClient.BucketExistsAsync(beArgs).ConfigureAwait(false);
    //        if (!found)
    //        {
    //            var mbArgs = new MakeBucketArgs().WithBucket(file.NameBucket);
    //            await _minioClient.MakeBucketAsync(mbArgs).ConfigureAwait(false);
    //        }

    //        string filename = file.Filename;
    //        string objectName = file.Path;

    //        byte[] contentBytes = Convert.FromBase64String(file.Content);
    //        var memoryStringFile = new System.IO.MemoryStream(contentBytes);

    //        // Upload a file to bucket.
    //        var putObjectArgs = new PutObjectArgs()
    //          .WithBucket(file.NameBucket)
    //          .WithObject(objectName)
    //          .WithContentType(mimetype)
    //          .WithStreamData(memoryStringFile)
    //          .WithObjectSize(memoryStringFile.Length);

    //        var data = await _minioClient
    //          .PutObjectAsync(putObjectArgs)
    //          .ConfigureAwait(false);

    //    }
    //    catch (InvalidBucketNameException ex)
    //    {
    //        throw new ArgumentException("Invalid bucket name", ex);
    //    }
    //    catch (ConnectionException ex)
    //    {
    //        throw new InvalidOperationException("Connection error", ex);
    //    }
    //    catch (InternalClientException ex)
    //    {
    //        throw new InvalidOperationException("Internal library error", ex);
    //    }
    //    catch (Minio.Exceptions.MinioException ex)
    //    {
    //        throw new KeyNotFoundException("File not found", ex);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception("An error occurred", ex);
    //    }
    //}


    /// <summary>
    /// Elimina un archivo del bucket indicado.
    /// Si no viene Filename (como en tu DeleteFile factory), se asume que Path es la clave completa.
    /// </summary>
    public async Task DeleteFile(File file)
    {
        if (file is null) throw new ArgumentNullException(nameof(file));
        if (string.IsNullOrWhiteSpace(file.NameBucket)) throw new ArgumentException("NameBucket es requerido.");

        var key = string.IsNullOrWhiteSpace(file.Filename)
            ? NormalizePath(file.Path) // tu factory DeleteFile sólo trae Path y Bucket
            : BuildObjectKey(file.Path, file.Filename);

        try
        {
            var removeArgs = new RemoveObjectArgs()
                .WithBucket(file.NameBucket)
                .WithObject(key);

            await _minioClient.RemoveObjectAsync(removeArgs);

            _logger.LogInformation("Objeto eliminado de MinIO. Bucket: {Bucket}, Key: {Key}", file.NameBucket, key);
        }
        catch (ObjectNotFoundException)
        {
            _logger.LogWarning("Intento de eliminar objeto inexistente. Bucket: {Bucket}, Key: {Key}", file.NameBucket, key);
        }
        catch (MinioException ex)
        {
            _logger.LogError(ex, "Error de MinIO al eliminar objeto. Bucket: {Bucket}, Key: {Key}", file.NameBucket, key);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado al eliminar objeto. Bucket: {Bucket}, Key: {Key}", file.NameBucket, key);
            throw;
        }
    }

    #region Helpers
    private static string BuildObjectKey(string path, string filename)
    {
        var normPath = NormalizePath(path);
        if (string.IsNullOrWhiteSpace(filename))
            return normPath;

        return string.IsNullOrWhiteSpace(normPath)
            ? filename.Trim().Replace("\\", "/")
            : $"{normPath}/{filename.Trim()}".Replace("//", "/");
    }

    private static string NormalizePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) return string.Empty;
        var p = path.Replace("\\", "/").Trim();
        p = p.TrimStart('/'); // no queremos leading slash en la key
        p = p.TrimEnd('/');   // ni trailing slash
        return p;
    }

    private static string ExtractBase64Payload(string content)
    {
        // Permite soportar formatos con encabezado data URL
        var idx = content.IndexOf("base64,", StringComparison.OrdinalIgnoreCase);
        return idx >= 0 ? content[(idx + "base64,".Length)..] : content;
    }

    private async Task EnsureBucketExistsAsync(string bucket)
    {
        try
        {
            var exists = await _minioClient.BucketExistsAsync(
                new BucketExistsArgs().WithBucket(bucket));

            if (!exists)
            {
                await _minioClient.MakeBucketAsync(
                    new MakeBucketArgs().WithBucket(bucket));
                _logger.LogInformation("Bucket creado en MinIO: {Bucket}", bucket);
            }
        }
        catch (MinioException ex)
        {
            _logger.LogError(ex, "Error verificando/creando bucket {Bucket}", bucket);
            throw;
        }
    }
    #endregion
}
