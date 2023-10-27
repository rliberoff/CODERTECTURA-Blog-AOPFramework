using System;
using System.Runtime.Remoting.Messaging;

using AOPFramework;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace AOPExceptionHandlingApplicationBlock
{
    /// <summary>
    /// Intercept processor for the calling and returning messages of the methods decorated as interceptable by this component.
    /// </summary>
    internal class ExceptionHandlingProcessor : IProcessor
    {
        /// <summary>
        /// Constant that identifies the exception handling policy of propagation.
        /// </summary>
        private const string PROPAGATE = @"PROPAGATE";

        /// <summary>
        /// Constant that identifies the exception handling policy of replacement.
        /// </summary>
        private const string REPLACE = @"REPLACE";

        /// <summary>
        /// Constant that identifies the exception handling policy of wrap.
        /// </summary>
        private const string WRAP = @"WRAP";

        /// <summary>
        /// Processes the call message, which happens before the execution of the body of the interceptable or processable method.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Since the exception handling only occurs after the execution of the body of the interceptable or processable method, this
        /// method does nothing.
        /// </para>
        /// <para>
        /// This is because the exception type is contained in the <c>Method Return Message</c>.
        /// </para>
        /// </remarks>
        /// <param name="callMessage">The <c>Method Call Message</c> to process.</param>
        /// <param name="processable">The processable decorated object to process.</param>
        public void ProcessCallMessage(IMethodCallMessage callMessage, InterceptableAttribute processable)
        {
            // Do Nothing.
        }

        /// <summary>
        /// Processes the return message, which happens after the execution of the body of the interceptable or processable method.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Since the <c>Method Return Message</c> contains the exception type, this method processes such message to retrieve the exception
        /// information.
        /// </para>
        /// <para>
        /// This method leverages the Microsoft Enterprise Library Exception Handling Application Block.
        /// </para>
        /// </remarks>
        /// <param name="returnMessage">The <c>Method Return Message</c> to process.</param>
        /// <param name="processable">The processable or interceptable object.</param>
        public void ProcessReturnMessage(IMethodReturnMessage returnMessage, InterceptableAttribute processable)
        {
            if (returnMessage != null && returnMessage.Exception != null)
            {
                string policyName = null;

                switch (((ExceptionHandlingAttribute)processable).Politic)
                {
                    case ExceptionHandlingPolitic.Propagate:
                        policyName = PROPAGATE;
                        break;

                    case ExceptionHandlingPolitic.Replace:
                        policyName = REPLACE;
                        break;

                    case ExceptionHandlingPolitic.Wrap:
                        policyName = WRAP;
                        break;

                    default:
                        throw new InvalidOperationException(@"Invalid or not defined exception handling politic.");
                }

                // Call to the static main entrance of the Microsoft Enterprise Library Exception Application Block.
                if (ExceptionPolicy.HandleException(returnMessage.Exception, policyName))
                {
                    throw returnMessage.Exception;
                }
            }
        }
    }
}
