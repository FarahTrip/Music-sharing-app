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
        public AmazonReturnState UserFolder(string UserId)
        {
            var path = "Users-Files";
            Client = new AmazonS3Client(AccessId, SecretKey, RegionEndpoint.EUNorth1);
            S3DirectoryInfo di = new S3DirectoryInfo(Client, BucketName, path);
            S3DirectoryInfo userFolder = new S3DirectoryInfo(Client, BucketName, path + "/" + UserId);
            var returnState = new AmazonReturnState();

            if (UserId == null)
            {
                returnState.error = "User id-ul este null";
                return returnState;
            }
            else
            {
                if (!userFolder.Exists)

                {
                    try
                    {
                        di.CreateSubdirectory(UserId);
                        returnState.success = path + "/" + UserId;
                        return returnState;
                    }
                    catch (AmazonS3Exception error)
                    {

                        returnState.error = $@"A aparut o eroare! Vezi detalii aici : Error code {error.ErrorCode} 
                       Mesaj : {error.Message}. Daca eroarea persista te rog contacteaza suportul.";

                        return returnState;
                    }
                }
                returnState.success = path + "/" + UserId;
                return returnState;
            }
        }

    }
}

