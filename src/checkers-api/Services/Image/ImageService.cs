using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace checkers_api.Services;

public class ImageService : IImageService
{
    private readonly string bucketName;
    private readonly string bucketDirectory;
    private readonly AmazonS3Client s3Client;
    private readonly ILogger<ImageService> logger;

    public ImageService(IConfiguration configuration, ILogger<ImageService> logger)
    {
        bucketName = configuration["AWS-BUCKET-NAME"];
        bucketDirectory = configuration["AWS-BUCKET-DIRECTORY"];
        s3Client = new AmazonS3Client(configuration["AWS-ACCESS-KEY-ID"], configuration["AWS-SECRET-ACCESS-KEY"], RegionEndpoint.USWest2);
        this.logger = logger;
    }

    public async Task<string> SaveImageAsync(IFormFile image)
    {
        var request = new PutObjectRequest()
        {
            BucketName = bucketName,
            Key =  bucketDirectory + "/" + Guid.NewGuid().ToString() + image.FileName,
            InputStream = image.OpenReadStream(),
            CannedACL = S3CannedACL.PublicRead
        };

        request.Metadata.Add("Content-Type", image.ContentType);
        await s3Client.PutObjectAsync(request);
        return $"https://{bucketName}.s3.us-west-2.amazonaws.com/" + request.Key;
    }
}
