using System;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;

namespace AOPFramework
{
    /// <summary>
    /// This class implements the necessary methods to represent a property that can be intercepted and provide information as well as
    /// data to the context sink.
    /// </summary>
    internal class InterceptProperty : IContextProperty, IContributeClientContextSink, IContributeServerContextSink
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        public InterceptProperty(string name) : base() { this.Name = name; }

        /// <summary>
        /// Gets the name of the property under which it will be added to the context.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Called when the context is frozen.
        /// </summary>
        /// <param name="newContext">The context to freeze.</param>
        public void Freeze(Context newContext) { }

        /// <summary>
        /// Returns a Boolean value indicating whether the context property is compatible with the new context.
        /// </summary>
        /// <param name="newCtx">The new context in which the ContextProperty has been created.</param>
        /// <returns>
        /// <c>True</c> if the context property can coexist with the other context properties in the given context; otherwise, <c>false</c>.
        /// </returns>
        public bool IsNewContextOK(Context newCtx)
        {
            return (newCtx != null) && (newCtx.GetProperty(this.Name) as InterceptProperty) != null;
        }

        /// <summary>
        /// Chains the message sink of the provided server object in front of the given sink chain.
        /// </summary>
        /// <param name="nextSink">The chain of sinks composed so far.</param>
        /// <returns>The composite sink chain.</returns>
        public IMessageSink GetClientContextSink(IMessageSink nextSink)
        {
            return new InterceptSink(nextSink);
        }

        /// <summary>
        /// Chains the message sink of the provided server object in front of the given sink chain.
        /// </summary>
        /// <param name="nextSink">The chain of sinks composed so far.</param>
        /// <returns>The composite sink chain.</returns>
        public IMessageSink GetServerContextSink(IMessageSink nextSink)
        {
            return new InterceptSink(nextSink);
        }
    }
}
