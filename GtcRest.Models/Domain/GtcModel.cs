using System;
using System.Collections.Generic;
using System.Text;

namespace GtcRest.Models.Domain
{
    /// <summary>
    /// The GTC Object
    /// </summary>
    /// <remarks>This represents a Gtc object</remarks>
    public class GtcModel
    {
        /// <summary>
        /// The object's Id
        /// </summary>
        /// <remarks></remarks>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// The object's Description 
        /// </summary>
        /// <remarks></remarks>
        /// <example>This is a description</example>
        public string Description { get; set; }

    }
}
