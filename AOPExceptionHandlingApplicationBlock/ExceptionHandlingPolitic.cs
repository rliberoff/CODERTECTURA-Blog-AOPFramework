using System;

namespace AOPExceptionHandlingApplicationBlock
{
    /// <summary>
    /// Defines the exception handling politics to use.
    /// </summary>
    public enum ExceptionHandlingPolitic
    {
        /// <summary>
        /// Represents the propagation politic. 
        /// This politic specifies that every exception will be passed to the caller without any transformation.
        /// </summary>
        Propagate,

        /// <summary>
        /// Represents the wrap politic. 
        /// This politic specifies that every exception will be wrapped inside another exception and then passed to the caller.
        /// </summary>
        Wrap,

        /// <summary>
        /// Represents the replacement politic. 
        /// This politic specifies that every exception will be replaced for another exception; then the new exception will be passed to the caller.
        /// </summary>
        /// <remarks>
        /// The original exception will be lost.
        /// </remarks>
        Replace
    }
}
