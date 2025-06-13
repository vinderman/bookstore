using System.Security.Cryptography;
using Bookstore.BL.Dto.File;
using Bookstore.BL.Interfaces;
using Bookstore.DAL.Interfaces;
using Bookstore.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using File = Bookstore.DAL.Entities.File;

namespace Bookstore.BL.Services;

public class FileService: IFileService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDocumentRepository _documentRepository;
    private readonly IFileRepository _fileRepository;

    public FileService(IUnitOfWork unitOfWork, IDocumentRepository documentRepository, IFileRepository fileRepository)
    {

        _unitOfWork = unitOfWork;
        _documentRepository = documentRepository;
        _fileRepository = fileRepository;
    }

    public async Task<bool> UploadFile(IFormFile file, Guid bookId)
    {
        var fileId = Guid.NewGuid();
        var fileStream = file.OpenReadStream();
        var fileHash = CalculateFileHash(fileStream);
        fileStream.Position = 0;
        var uploadResult = await _documentRepository.UploadDocument(fileStream, fileId.ToString());

        if (!uploadResult)
        {
            throw new Exception($"File {fileId} could not be uploaded.");
        }


        var parsedFile = ParseFileName(file.FileName);

        await _fileRepository.AddAsync(new File
        {
            Id = fileId,
            Name = parsedFile.FileName,
            FileSize = file.Length,
            FileType = parsedFile.FileExtension,
            BookId = bookId,
            S3Url = fileId.ToString(),
            FileHash = fileHash,
            UploadedAt = DateTime.UtcNow
        });
        await _unitOfWork.SaveChangesAsync();


        return true;
    }

    public async Task DeleteFile(Guid fileId)
    {
        await _fileRepository.Delete(new File { Id = fileId });
    }

    public async Task<DownloadFileDto> DownloadFile(Guid id)
    {
        // var file = await _documentRepository.DownloadDocument(id);
        //
        // return new DownloadFileDto
        // {
        //     FileContent = file,
        //     Name = book.Name
        // };
        throw new NotImplementedException();
    }

    // public FileDto? GetBookFileContent(Book book)
    // {
    //     FileDto? fileContent = null;
    //     if (book.FileId != null && book.FileName != null)
    //     {
    //         fileContent = new FileDto { FileId = book.FileId, FileName = book.FileName, };
    //     }
    //
    //     return fileContent;
    // }

    private ParsedFileName ParseFileName(string fileName)
    {
        var lastDotIndex = fileName.LastIndexOf(".", StringComparison.InvariantCulture);

        if (lastDotIndex.Equals(-1))
        {
            throw new BadRequestException("Невозможно получить расширение файла. Укажите корректное название");
        }

        return new ParsedFileName
        {
            FileName = fileName.Substring(0, lastDotIndex),
            FileExtension = fileName.Substring(lastDotIndex + 1)
        };
    }

    private string CalculateFileHash(Stream file)
    {
        var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(file);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }
}
