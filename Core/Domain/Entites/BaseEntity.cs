using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class BaseEntity<Tkey>
    {  
            public Tkey Id { get; set; } = default!;    
            public DateTime CreatedAt { get; set; }

    }
}
