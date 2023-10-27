using System;
using System.Runtime.Remoting.Messaging;

namespace AOPFramework
{
    /// <summary>
    /// Represents an intercept sink.
    /// </summary>
    internal class InterceptSink : IMessageSink
    {
        /// <summary>
        /// Stores the next message sink in the sink chain.
        /// </summary>
        private IMessageSink nextSink;

        /// <summary>
        /// A generic instance of an object to lock on, rather than locking on the type itself, to avoid deadlocks.
        /// </summary>
        /// <remarks>
        /// The initialization of this variable uses the <i>static initialization</i> approach, where the instance is created 
        /// the first time any member of the class is referenced. The common language runtime (CLR) takes care of the variable 
        /// initialization.
        /// </remarks>
        private static object syncRoot = new Object();

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="nextSink">The next message sink in the sink chain.</param>
        public InterceptSink(IMessageSink nextSink)
        {
            lock (syncRoot)
            {
                this.nextSink = nextSink;
            }
        }

        /// <summary>
        /// Gets the next message sink in the sink chain.
        /// </summary>
        public IMessageSink NextSink
        {
            get { return this.nextSink; }
        }

        /// <summary>
        /// Asynchronously processes the given message.
        /// </summary>
        /// <param name="msg">The message to process.</param>
        /// <param name="replySink">The reply sink for the reply message.</param>
        /// <returns>
        /// Returns an <c>IMessageCtrl</c> interface that provides a way to control asynchronous messages after they have been dispatched.
        /// </returns>
        public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
        {
            return nextSink.AsyncProcessMessage(msg, replySink);
        }

        /// <summary>
        /// Synchronously processes the given message.
        /// </summary>
        /// <param name="msg">The message to process.</param>
        /// <returns>A reply message in response to the request.</returns>
        public IMessage SyncProcessMessage(IMessage msg)
        {
            IMethodCallMessage callMessage = (msg as IMethodCallMessage);

            if (callMessage != null)
            {
                InterceptableAttribute[] processables = (InterceptableAttribute[])(msg as IMethodMessage).MethodBase.GetCustomAttributes(typeof(InterceptableAttribute), true);

                Array.Sort<InterceptableAttribute>(processables, new Comparison<InterceptableAttribute>(delegate(InterceptableAttribute a, InterceptableAttribute b) { return a.Priority.CompareTo(b.Priority); }));

                // Do pre-processing: process the method call before its body execution. The calling parameters are going to be available.
                for (int i = 0; i < processables.Length; i++)
                {
                    processables[i].Processor.ProcessCallMessage(callMessage, processables[i]);
                }

                // Do post-processing: process the method call after its body execution. The return values (or exception) are going to be available.
                IMethodReturnMessage returnMessage = (nextSink.SyncProcessMessage(callMessage) as IMethodReturnMessage);

                if (returnMessage != null)
                {
                    for (int i = 0; i < processables.Length; i++)
                    {
                        processables[i].Processor.ProcessReturnMessage(returnMessage, processables[i]);
                    }

                    return returnMessage;
                }
            }

            return msg;
        }
    }
}
