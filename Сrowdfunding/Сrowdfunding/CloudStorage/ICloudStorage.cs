using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Сrowdfunding.CloudStorage
{
    public interface ICloudStorage
    {
        Uri UploadImage(string file);
    }
}
