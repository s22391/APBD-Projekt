using System.Collections.Generic;

namespace StocksApplication.Server.Dtos
{
    public class WrapperDto<T>
    {
        public T Results { get; set; }
    }
}