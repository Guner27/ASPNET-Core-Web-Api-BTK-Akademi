using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore.Extensions
{
    public static class OrderQueryBuilder
    {
        public static String CreateOrderQuery<T>(String orderByQueryString)
        {
            //orderParams: String disizi Split ile virgüllü olan yerlerden parametreleri kesip diziye aktar.
            //title,id,price => [[title],[id],[price]]
            var orderParams = orderByQueryString.Trim().Split(',');


            var propertyInfos = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);
            //Public veya new'lenebilen üyeleri al Book için: id,title,price


            //Sorgu ifadesi
            var orderQueryBuilder = new StringBuilder();


            //title ascending, price descending, id ascending,[,]
            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                //dizideki her bir parametreyi bu defa buşluğa göre ayır ilk elemanını al.
                //price desc ise [[price],[desc]] --> [price] elemanını al bu, queryden gelen parametre ismidir.
                var propertyFromQueryName = param.Split(' ')[0];

                //Query String ile nesneyi eşleştir. (StringComparison... = büyük küçük harf ayrımını dikkate alma.
                var objectProperty = propertyInfos
                .FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName,
                StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty is null)
                    continue;

                //OrderBy yönü
                var direction = param.EndsWith(" desc") ? "descending" : "ascending";

                //title ascending, price descending, id ascending,[,] 
                //bu ifadeyi oluştur.
                orderQueryBuilder.Append($"{objectProperty.Name.ToString()}  {direction},");
            }


            //title ascending, price descending, id ascending,[,]
            //TrimEnd ile sondaki virgülü at yerine boşluk ekle.
            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            return orderQuery;

        }
    }
}
