using System;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;
using System.Security.Permissions;

namespace AOPFramework
{
    /// <summary>
    /// Defines an attribute that will decorate those classes that can be intercepted in order to process its behaviour, represented by
    /// the invokation of its methods.
    /// </summary>
    /// <remarks>
    /// Class extends from <c>ContextAttribute</c>, which provides the default implementations of the <c>IContextAttribute</c> and <c>IContextProperty</c> interfaces.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    internal sealed class InterceptContextAttribute : ContextAttribute
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        public InterceptContextAttribute() : base(Guid.NewGuid().ToString()) { }

        /// <summary>
        /// Called when the context is frozen.
        /// </summary>
        /// <param name="newContext">The context to freeze.</param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override void Freeze(Context newContext) { }

        /// <summary>
        /// Adds the current context property to the given message.
        /// </summary>
        /// <param name="ctorMsg">The <c>IConstructionCallMessage</c> to which to add the context property.</param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override void GetPropertiesForNewContext(IConstructionCallMessage ctorMsg)
        {
            if (ctorMsg != null)
            {
                ctorMsg.ContextProperties.Add(new InterceptProperty(this.Name));
            }
        }

        /// <summary>
        /// Returns a Boolean value indicating whether the context parameter meets the context attribute's requirements.
        /// </summary>
        /// <param name="ctx">The context in which to check.</param>
        /// <param name="ctorMsg">The <c>IConstructionCallMessage</c> to which to add the context property.</param>
        /// <returns>
        /// This method will return <c>true</c> if the passed in context is okay; otherwise, it will return <c>false</c>.
        /// </returns>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override bool IsContextOK(Context ctx, IConstructionCallMessage ctorMsg)
        {
            return (ctx != null) && (ctx.GetProperty(this.Name) as InterceptProperty) != null;
        }

        /// <summary>
        /// Returns a Boolean value indicating whether the context property is compatible with the new context.
        /// </summary>
        /// <param name="newCtx">The new context in which the property has been created.</param>
        /// <returns>
        /// This method will return <c>true</c> if the context property is okay with the new context; otherwise, it will return <c>false</c>.
        /// </returns>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override bool IsNewContextOK(Context newCtx)
        {
            return (newCtx != null) && (newCtx.GetProperty(this.Name) as InterceptProperty) != null;
        }
    }
}
