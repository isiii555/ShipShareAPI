using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Application.Interfaces.Services
{
    public interface IUploadImageToStorageService
    {
        string UploadImageToStorage(IFormFile formFile);
    }
}
