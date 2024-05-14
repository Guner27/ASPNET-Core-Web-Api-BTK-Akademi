using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class MetaData
    {
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious => CurrentPage > 1; //Önceki sayfa var mı?

        /// <summary>
        /// HasNext = HasPage
        /// </summary>
        public bool HasNext => CurrentPage < TotalPage; //Sonrasında sayfa var mı?
    }
}
