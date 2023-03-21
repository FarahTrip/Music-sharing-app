using Amazon;
using Amazon.S3;
using Amazon.S3.IO;

namespace Trippin_Website.Logic_classes
{
    public class AmazonHelper
    {
        public readonly string AccessId;
        public readonly string SecretKey;
        public readonly string BucketName;
        private AmazonS3Client Client { get; set; }

        public AmazonHelper()
        {
            AccessId = "AKIAT5UTNOTECKGQZCOK";
            SecretKey = "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";
            BucketName = "trippin-website";
        }

        public string CreateFolder(string UserId)
        {
            var path = "Users-Files";
            Client = new AmazonS3Client(AccessId, SecretKey, RegionEndpoint.EUNorth1);
            S3DirectoryInfo di = new S3DirectoryInfo(Client, BucketName, path);


            di.CreateSubdirectory(UserId);


            return path + "/" + UserId;
        }
    }
}