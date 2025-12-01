using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Domain.Interfaces
{
    public interface IDeck : IBaseModel
    {
        string Name { get; set; }
        IUser User { get; set; }
        List<ICard>? Cards { get; set; }
    }
}
