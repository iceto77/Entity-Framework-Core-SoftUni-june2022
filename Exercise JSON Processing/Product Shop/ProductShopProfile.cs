namespace ProductShop
{
    using ProductShop.Models;
    using ProductShop.DTOs.User;
    using ProductShop.DTOs.Product;
    using ProductShop.DTOs.Category;
    using ProductShop.DTOs.CategoryProduct;
    
    using AutoMapper;
    using System.Linq;
    using System;

    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<ImportUserDto, User>();
            this.CreateMap<ImportProductDto, Product>();
            this.CreateMap<ImportCategoriesDto, Category>();
            this.CreateMap<ImportCategoryProductDto, CategoryProduct>();

            this.CreateMap<Product, ExportProductsInRangeDto>()
                .ForMember(d => d.SellerFullName, mo => mo.MapFrom(s => $"{s.Seller.FirstName} {s.Seller.LastName}"));
            this.CreateMap<Product, ExportUserSoldProductsDto>()
                .ForMember(d => d.BuyerFirstName, mo => mo.MapFrom(s => s.Buyer.FirstName))
                .ForMember(d => d.BuyerLastName, mo => mo.MapFrom(s => s.Buyer.LastName));
            this.CreateMap<User, ExportUsersWithSoldProductsDto>()
                .ForMember(d => d.SoldProducts, mo => mo.MapFrom(s => s.ProductsSold.Where(p => p.BuyerId.HasValue)));

            this.CreateMap<Category, ExportCategoryByProductsInfoDto>()
                .ForMember(d => d.Category, mo => mo.MapFrom(s => s.Name))
                .ForMember(d => d.ProductsCount, mo => mo.MapFrom(s => s.CategoryProducts.Count))
                .ForMember(d => d.AveragePrice, mo => mo.MapFrom(s => Math.Round(s.CategoryProducts.Average(p => p.Product.Price), 2, MidpointRounding.AwayFromZero).ToString()))
                .ForMember(d => d.TotalRevenue, mo => mo.MapFrom(s => Math.Round(s.CategoryProducts.Sum(p => p.Product.Price), 2, MidpointRounding.AwayFromZero).ToString()));


            //this.CreateMap<Product, ExportSoldProductShortInfoDto>();
            //this.CreateMap<User, ExportSoldProductsFullInfoDto>()
            //    .ForMember(d => d.SoldProducts, mo => mo.MapFrom(s => s.ProductsSold.Where(p => p.BuyerId.HasValue)));
            //this.CreateMap<User, ExportUsersWithFullProductInfoDto>()
            //    .ForMember(d => d.SoldProductsInfo, mo => mo.MapFrom(s => s));
        }
    }
}
