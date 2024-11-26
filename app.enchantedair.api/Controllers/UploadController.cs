using Amazon.S3.Model;
using Amazon.S3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace app.enchantedair.api.Controllers
{
    [EnableCors("AllowReactApp")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {

        IAmazonS3 client { get; }
        public string IdentityPath() =>
            User
            .FindFirst(ClaimTypes.NameIdentifier)
            ?.Value
            ?.Replace("-", @"/") ?? "";
        public UploadController(IAmazonS3 client,IHttpContextAccessor context)
        {
            this.client = client;
        }
        
        [HttpPost]
        public async Task<IActionResult> GetPresignedUploadUrl()
        {
            // Generate a unique key for the object (e.g., using a GUID)
            var objectKey = $"{IdentityPath()}/{Guid.NewGuid().ToString("N")}"; // Or any file extension/type you expect
            // Define the request for a presigned URL with PUT method for uploading
            var request = new GetPreSignedUrlRequest
            {
                BucketName = "treesofnyc",
                Key = objectKey,
                Expires = DateTime.UtcNow.AddMinutes(15), // Set URL expiration time
                Verb = HttpVerb.PUT // HTTP PUT for uploading files
            };
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // get user ID from claims
            var userName = User.Identity.Name; // get username
            // Generate the presigned URL
            string presignedUrl = client.GetPreSignedURL(request);
            return Ok(new { uploadUrl = presignedUrl });
        }
    }
}
