using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace BusinessLayer.Concrete
{
    public  class ImageFileManager : IImageFileService
    {
        IImageFileDal _imageFileDal;

        public ImageFileManager(IImageFileDal imageFileDal)
        {
            _imageFileDal = imageFileDal;
        }

        public ImageFile GetByID(int id)
        {
         return _imageFileDal.Get(x => x.ImageID == id);
        }

        public List<ImageFile> GetList()
        {
           return _imageFileDal.List();
        }

        public void TAdd(ImageFile t)
        {
            _imageFileDal.Insert(t);
        }

        public void TDelete(ImageFile t)
        {
            _imageFileDal.Delete(t);
        }
    }
}
