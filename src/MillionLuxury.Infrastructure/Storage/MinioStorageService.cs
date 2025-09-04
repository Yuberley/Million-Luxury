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

    public MinioStorageService(IMinioClient minioClient, ILogger<MinioStorageService> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    /// <summary>
    /// Gets the contents of a file from the specified bucket.
    /// Returns the content in bytes.
    /// </summary>
    public async Task<byte[]> GetFile(File file)
    {
        if (file is null) throw new ArgumentNullException(nameof(file));
        if (string.IsNullOrWhiteSpace(file.NameBucket)) throw new ArgumentException("NameBucket es requerido.");

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

    /// <summary>
    /// Saves a file to the specified bucket.
    /// It is assumed that File.Content is in Base64 and that Mimetype is the Content-Type.
    /// </summary>
    public async Task<string> SaveFile(File file)
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

            var base64 = ExtractBase64Payload(file.Content);
            var bytes = Convert.FromBase64String(base64);

            using var memoryStream = new MemoryStream(bytes);

            var putArgs = new PutObjectArgs()
                .WithBucket(file.NameBucket)
                .WithObject(key)
                .WithStreamData(memoryStream)
                .WithObjectSize(memoryStream.Length)
                .WithContentType(string.IsNullOrWhiteSpace(file.Mimetype)
                    ? "application/octet-stream"
                    : file.Mimetype);

            await _minioClient.PutObjectAsync(putArgs);

            _logger.LogInformation("Objeto guardado en MinIO. Bucket: {Bucket}, Key: {Key}, Size: {Size} bytes",
                file.NameBucket, key, memoryStream.Length);

            return $"{_minioClient.Config.Endpoint}/{file.NameBucket}/{key}";
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

    /// <summary>
    /// Deletes a file from the specified bucket.
    /// If Filename is not provided (as in your DeleteFile factory), Path is assumed to be the full key.
    /// </summary>
    public async Task DeleteFile(File file)
    {
        if (file is null) throw new ArgumentNullException(nameof(file));
        if (string.IsNullOrWhiteSpace(file.NameBucket)) throw new ArgumentException("NameBucket es requerido.");

        var key = string.IsNullOrWhiteSpace(file.Filename)
            ? NormalizePath(file.Path)
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
        p = p.TrimStart('/');
        p = p.TrimEnd('/');
        return p;
    }

    private static string ExtractBase64Payload(string content)
    {
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

                await EnsureBucketPublicReadAsync(bucket);

                _logger.LogInformation("Bucket creado en MinIO: {Bucket}", bucket);
            }
        }
        catch (MinioException ex)
        {
            _logger.LogError(ex, "Error verificando/creando bucket {Bucket}", bucket);
            throw;
        }
    }

    private async Task EnsureBucketPublicReadAsync(string bucket, string? prefix = null)
    {
        var resource = string.IsNullOrWhiteSpace(prefix)
            ? $"arn:aws:s3:::{bucket}/*"
            : $"arn:aws:s3:::{bucket}/{NormalizePath(prefix)}/*";

        var policyJson = $$"""
            {
              "Version": "2012-10-17",
              "Statement": [
                {
                  "Sid": "AllowAnonymousRead",
                  "Effect": "Allow",
                  "Principal": "*",
                  "Action": ["s3:GetObject"],
                  "Resource": ["{{resource}}"]
                }
              ]
            }
            """;

        await _minioClient.SetPolicyAsync(
            new SetPolicyArgs()
                .WithBucket(bucket)
                .WithPolicy(policyJson)
        );
    }
    #endregion
}
