namespace MillionLuxury.Application.Common.Abstractions.Storage;

using MillionLuxury.Domain.File;

public interface IStorageService
{
    Task<string> SaveFile(File file);
    Task<byte[]> GetFile(File file);
    Task DeleteFile(File file);
}
