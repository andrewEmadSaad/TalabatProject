using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithBrandAndTypeSpecifications : BaseSpecification<Product>
    {

        // This Ctor Is Using To Get All Product
        public ProductWithBrandAndTypeSpecifications(ProductSpecParams productParams)
            : base(P =>
                     (string.IsNullOrEmpty(productParams.Search)|| P.Name.ToLower().Contains( productParams.Search))&&
                     (!productParams.BrandId.HasValue || P.ProductBrandId == productParams.BrandId.Value) &&
                     (!productParams.TypeId.HasValue || P.ProductTypeId == productParams.TypeId.Value)

                  )
        {
            // Add Includes To Query
            AddProductIncludes();

            ApplyPagination(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);

            // Add OrderBy To Query
            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }



        }

        // This Ctor Is Using To Get Specific Product
        public ProductWithBrandAndTypeSpecifications(int Id) : base(P => P.Id == Id)
        {
            AddProductIncludes();
        }


        private void AddProductIncludes()
        {

            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);

        }
    }
}
