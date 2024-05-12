using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    //public record BookDtoForUpdate
    //{
    //    public int Id { get; init; } //init anahtar sözcüğü Readonly  özellik kazandırır.
    //    public String Title { get; init; }
    //    public decimal Price { get; init; }
    //}
    //Üstteki ile alttaki eşdeğer 
    public record BookDtoForUpdate(int Id, String Title, decimal Price);
}
