using Entities.Models;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    //Data Shapping; Her API için şart değil. İhtiyaca göre tanımlanmalıdır.
    public class DataShapper<T> : IDataShaper<T> where T : class
    {
        //Property'leri elde edecek, Book için: Id, Title, Price
        public PropertyInfo[] Properties { get; set; }
        public DataShapper()
        {
            Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        //ExpandoObject: RunTime'da dinamik olarak ürettiğimiz herhangi bir nesneye karşılık gelebiliyor.
        public IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string fieldString)
        {
            var requiredFields = GetRequiredProperties(fieldString);
            return FetchData(entities, requiredFields);
        }

        public ShapedEntity ShapeData(T entity, string fieldString)
        {
            var requiredProperties = GetRequiredProperties(fieldString);
            return FetchDataForEntity(entity, requiredProperties);
        }

        private IEnumerable<PropertyInfo> GetRequiredProperties(string fieldString)
        {
            //Eşleşen, gerekli olan property'leri(QueryString'de verilen) sistemdeki modelden(classdan) seçen fonksiyon..

            var requiredFields = new List<PropertyInfo>();

            if (!string.IsNullOrWhiteSpace(fieldString))
            {
                var fields = fieldString.Split(',', StringSplitOptions.RemoveEmptyEntries);

                foreach (var field in fields)
                {
                    var property = Properties
                        .FirstOrDefault(pi => pi.Name.Equals(field.Trim(), StringComparison.InvariantCultureIgnoreCase));

                    if (property is null)
                        continue;
                    requiredFields.Add(property);
                }
            }
            else
            {
                //Herhangi bir fields tanımı yoksa tüm property'leri listele
                requiredFields = Properties.ToList();
            }
            return requiredFields;
        }

        private ShapedEntity FetchDataForEntity(T entity, IEnumerable<PropertyInfo> requiredProperties)
        {
            //Hangi property'lere ihtiyaç varsa, ilgili property'lerin değerlerini üretip [Key], [Value] şeklindeki ifadelere oluşturup döndüren fonksiyon.

            //Şekillendirdiğimiz nesne = ilgili nesne RunTime(Çalışma zamanın)da üretilecek
            var shapedObject = new ShapedEntity();

            foreach (var property in requiredProperties)
            {
                var objectPropertyValue = property.GetValue(entity);
                //Property'nin adı ve içerisindeki değeri objeye ekle. (ÖR: {Id: 32, Title:"Another Love"})
                shapedObject.Entity.TryAdd(property.Name, objectPropertyValue);
            }
            var objectProperty = entity.GetType().GetProperty("Id");
            shapedObject.Id = (int)objectProperty.GetValue(entity);
            return shapedObject;
        }

        private IEnumerable<ShapedEntity> FetchData(IEnumerable<T> entities, IEnumerable<PropertyInfo> requiredProperties)
        {
            //Yukarıdaki fonksiyon ile aynı, Yukarıda tek bir nesne vardı burada bir nesne listesi
            var shapedData = new List<ShapedEntity>();
            foreach (var entity in entities)
            {
                var shapedObject = FetchDataForEntity(entity, requiredProperties);
                shapedData.Add(shapedObject);
            }
            return shapedData;
        }

    }
}
