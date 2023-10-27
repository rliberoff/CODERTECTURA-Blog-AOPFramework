using System;
using System.Security.Permissions;

namespace AOPFramework
{
    /// <summary>
    /// Represents a context-bound object whose execution must be intercepted.
    /// </summary>
    [InterceptContextAttribute]
    public abstract class InterceptableObject : ContextBoundObject
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        protected InterceptableObject() : base() { }

        /// <summary>
        /// Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns>Returns <c>null</c> since a lifetime service is not defined for interceptable objects.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
