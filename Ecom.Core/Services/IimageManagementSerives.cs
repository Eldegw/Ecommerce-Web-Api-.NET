using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Ecom.Core.Services
{
    public interface IimageManagementSerives
    {
        Task<List<string>> AddImageAsync(IFormFileCollection file, string src);
        public void DeleteImageAsync(string imageName, string folderName);

      
        
        //void DeleteImageAsync(string src);

        //public void DeleteImage(string src, string imageName);

    }
}
