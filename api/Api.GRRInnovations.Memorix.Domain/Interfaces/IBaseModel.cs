﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Domain.Interfaces
{
    public interface IBaseModel
    {
        Guid Uid { get; set; }

        Guid UpdatedBy { get; set; }

        DateTime CreatedAt { get; set; }

        DateTime UpdatedAt { get; set; }
    }
}
