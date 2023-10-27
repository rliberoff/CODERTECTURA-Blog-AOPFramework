using System;
using System.Runtime.Remoting.Messaging;

namespace AOPFramework
{
    /// <summary>
    /// Defines the interface for those objects that will process the calling and return messages for a class that extends 
    /// from <c>InterceptableObject</c>.
    /// </summary>
    /// <seealso cref="AOPFramework.InterceptableObject"/>
    public interface IProcessor
    {
        /// <summary>
        /// Process a <c>Method Call Message</c>.
        /// </summary>
        /// <remarks>
        /// This method will be called before the execution of the body of an interceptable or processable object.
        /// </remarks>
        /// <param name="callMessage">The <c>Method Call Message</c> to process.</param>
        /// <param name="processable">The processable decorated object to process.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(@"Microsoft.Naming", @"CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = @"processable", Justification = @"According to Merriam-Webster dictionary, the word 'Processable' exists and is correctly spelled (http://www.merriam-webster.com/dictionary/processable).")]
        void ProcessCallMessage(IMethodCallMessage callMessage, InterceptableAttribute processable);

        /// <summary>
        /// Process a <c>Method Return Message</c>.
        /// </summary>
        /// <remarks>
        /// This method will be called after the execution of the body of an interceptable or processable object. The <c>returnMessage</c> parameter will
        /// contain the result of the execution, including any possible exception type.
        /// </remarks>
        /// <param name="returnMessage">The <c>Method Return Message</c> to process.</param>
        /// <param name="processable">The processable decorated object to process.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(@"Microsoft.Naming", @"CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = @"processable", Justification = @"According to Merriam-Webster dictionary, the word 'Processable' exists and is correctly spelled (http://www.merriam-webster.com/dictionary/processable).")]
        void ProcessReturnMessage(IMethodReturnMessage returnMessage, InterceptableAttribute processable);
    }
}
