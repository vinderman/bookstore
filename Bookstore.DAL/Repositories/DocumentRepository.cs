using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Bookstore.DAL.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Bookstore.DAL.Repositories;

public class DocumentRepository: IDocumentRepository
{
    private readonly string _bucketName = "bookstore-s3";
    private readonly string _serviceUrl = "https://hb.ru-msk.vkcloud-storage.ru";
    private readonly IConfiguration _config;
    private readonly AmazonS3Client _s3Client;

    public DocumentRepository(IConfiguration config)
    {
        _config = config;
        _s3Client = new AmazonS3Client(
            new BasicAWSCredentials(
                "6Pw1wS683wySu2HQsS4SnK",
                "6CS1s7mHUJDmffgCHvAKKpsG36dhsSegE9EGZGr2B5iX"
            ),
            new AmazonS3Config
            {
                ForcePathStyle =  true,
                ServiceURL = _serviceUrl
            }
        );
    }

    public async Task<bool> UploadDocument(Stream file, string key)
    {
        var result = await _s3Client.PutObjectAsync(new PutObjectRequest
        {
            BucketName = _bucketName, InputStream = file, Key = key, DisableDefaultChecksumValidation = true
        });

        if (result == null)
        {
            throw new Exception("Could not upload document.");
        }

        return result.HttpStatusCode == System.Net.HttpStatusCode.OK;
    }

    public async Task<Stream> DownloadDocument(string key)
    {
        var getRequest = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = key
        };

        var response = await _s3Client.GetObjectAsync(getRequest);
        return response.ResponseStream;
    }

    public async Task<bool> DeleteDocument(string key)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = key
        };

        var response = await _s3Client.DeleteObjectAsync(deleteRequest);
        return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
    }
}
