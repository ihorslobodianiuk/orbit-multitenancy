// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Orbit.Multitenant.Api.Models
{
    /// <summary>
    /// Customer.
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// First Name.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Last Name.
        /// </summary>
        public string? LastName { get; set; }
    }
}
