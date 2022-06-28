using ContosoRest.Interfaces.Repository;

using System;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bricklink_API_Client;

namespace ContosoRest.Repository.Repos
{
    public class BricklinkRepo : IBricklinkRepo
    {
        private readonly ILogger<ContosoRepo> _logger;
        private readonly IBricklinkAdapter _bricklinkAdapter;

        public BricklinkRepo(IBricklinkAdapter bricklinkAdapter, ILogger<BricklinkRepo> logger)
        {
            _logger = logger;
            _bricklinkAdapter = bricklinkAdapter;
        }
    }
}
