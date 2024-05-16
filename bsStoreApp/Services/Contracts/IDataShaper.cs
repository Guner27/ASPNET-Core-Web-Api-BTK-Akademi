using Entities.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IDataShaper<T>
    {
        //Data Shapping; Her API için şart değil. İhtiyaca göre tanımlanmalıdır.
        IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string fieldString);
        ShapedEntity ShapeData(T entity, string fieldString);
    }
}
