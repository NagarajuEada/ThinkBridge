using AutoMapper;
using ShopBridge.DTO;
using ShopBridge.Interfaces;
using ShopBridge.Interfaces.Repository;
using ShopBridge.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Net.Http.Headers;
using System.Transactions;

namespace ShopBridge.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public InventoryService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDto> GetProductAsync(int id)
        {
            var product = await _productRepository.FetchAsync(filter: x => x.Id == id).ConfigureAwait(false);
            if (product != null)
            {
                var productDto = _mapper.Map<ProductDto>(product.Single());
                productDto.ProductImage = ImageToBase64(product.Single().ProductImage);
                return productDto;
            }
            return null;
        }
        public async Task<List<ProductDto>> GetProductsAsync()
        {
            var products = await _productRepository.FetchAsync().ConfigureAwait(false);
            var productsDto = _mapper.Map<List<ProductDto>>(products);
            foreach (var itemDto in productsDto)
            {
                foreach (var item in products)
                {
                    itemDto.Thumbnail = ImageToBase64(item.Thumbnail);
                }
            }
            return productsDto;
        }
        public string ImageToBase64(string path)
        {

            using (System.Drawing.Image image = System.Drawing.Image.FromFile(path))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }
        public async Task<bool> DeleteProductAsync(int id)
        {

            var product = await _productRepository.FetchAsync(filter: x => x.Id == id).ConfigureAwait(false);
            File.Delete(product.Single().Thumbnail);
            File.Delete(product.Single().ProductImage);
            var res = await _productRepository.DeleteEntityAsync(id);
            return res;



        }
        public async Task<int> AddProductAsync(ProductDto productDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);
                string directorypath = Path.Combine(Directory.GetCurrentDirectory(), "Images");
                if (!Directory.Exists(directorypath))
                {
                    Directory.CreateDirectory(directorypath);
                }
                Guid name = Guid.NewGuid();
                string filePath = Path.Combine(directorypath, name.ToString() + ".jpeg");
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    productDto.ImageFile.CopyTo(stream);
                    product.ProductImage = filePath;
                }
                product.Thumbnail = ConvertThumb(filePath);
                return await _productRepository.AddEntityAsync(product);
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public bool ThumbnailCallback()
        {
            return false;
        }
        public string ConvertThumb(string imgbyte)
        {

            System.Drawing.Image OriginalImage;

            OriginalImage = System.Drawing.Image.FromFile(imgbyte);

            var imThumbnailImage = OriginalImage.GetThumbnailImage(75, 75, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
            MemoryStream myMS = new MemoryStream();
            imThumbnailImage.Save(myMS, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] test_imge = myMS.ToArray();
            string directorypath = Path.Combine(Directory.GetCurrentDirectory(), "Thumbnails");
            if (!Directory.Exists(directorypath))
            {
                Directory.CreateDirectory(directorypath);
            }
            Guid name = Guid.NewGuid();
            string filePath = Path.Combine(directorypath, name.ToString() + ".jpeg");
            using (Image image = Image.FromStream(new MemoryStream(test_imge)))
            {


                image.Save(filePath, ImageFormat.Jpeg);
            }
            imThumbnailImage.Dispose();
            OriginalImage.Dispose();
            return filePath;



        }
    }
}




