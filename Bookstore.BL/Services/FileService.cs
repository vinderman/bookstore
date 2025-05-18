using Bookstore.BL.Dto.File;
using Bookstore.BL.Interfaces;
using Bookstore.DAL.Entities;
using Bookstore.DAL.Interfaces;
using Bookstore.Shared.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Bookstore.BL.Services;

public class FileService: IFileService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDocumentRepository _documentRepository;
    public FileService(IUnitOfWork unitOfWork, IDocumentRepository documentRepository)
    {

        _unitOfWork = unitOfWork;
        _documentRepository = documentRepository;


    }
    public async Task<bool> UploadFile(IFormFile file, Guid id)
    {
        var fileId = Guid.NewGuid().ToString();
        var fileStream = file.OpenReadStream();

        await _documentRepository.UploadDocument(fileStream, fileId);


        // TODO: добавить работу с fileRepository
        // await _unitOfWork.BeginTransactionAsync();

        // await _unitOfWork.CommitTransactionAsync();


        return true;
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
}
