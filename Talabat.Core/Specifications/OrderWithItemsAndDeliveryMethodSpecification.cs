using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Core.Specifications
{
    public  class OrderWithItemsAndDeliveryMethodSpecification:BaseSpecification<Order>
    {

        public OrderWithItemsAndDeliveryMethodSpecification(string buyerEmail)
            : base(
                  O => O.BuyerEmail == buyerEmail        
                 )
        {
            AddIncludes();
            AddOrderByDesc(O => O.OrderDate);
        }

        public OrderWithItemsAndDeliveryMethodSpecification(string buyerEmail,int orderId)
            :base(O=>
                    O.BuyerEmail == buyerEmail&&
                    O.Id==orderId
                 )
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(O => O.Items);
            Includes.Add(O => O.DeliveryMethod);
        }
    }
}
