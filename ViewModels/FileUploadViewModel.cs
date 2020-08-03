using CricketFavourites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CricketFavourites.ViewModels
{
    public class FileUploadViewModel
    {
        public Favourite CurrentFavourite { get; set; }
        public List<FileModel> Files { get; set; }
    }
}
