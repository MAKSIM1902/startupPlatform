using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Сrowdfunding.CloudStorage
{
    public class CloudinaryStorage : ICloudStorage
    {
        static Account account = new Account(
            "dwivxsl5s",
            "112216621386628",
            "ktwW_1UFvkjoG_DI5zBxW9DXp3Q");
        Cloudinary cloudinary = new Cloudinary(account);

        public Uri UploadImage(string file)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(@file)
            };
            var uploadResult = cloudinary.Upload(uploadParams);
            return uploadResult.Url;
        }

    }
}
