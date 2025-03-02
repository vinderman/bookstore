namespace Bookstore.DAL.Interfaces;

public interface IDocumentRepository
{
    Task<bool> UploadDocument(Stream stream, string key);

    Task<bool> DeleteDocument(string key);

    Task<Stream> DownloadDocument(string key);
}
