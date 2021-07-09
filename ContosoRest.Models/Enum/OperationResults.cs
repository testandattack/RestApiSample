using System;
using System.Collections.Generic;
using System.Text;

namespace ContosoRest.Models.Enum
{
    public enum OperationResult
    {
        Created,

        Updated,

        Deleted,

        NotFound,

        BadRequest,

        Exception
    }
}
