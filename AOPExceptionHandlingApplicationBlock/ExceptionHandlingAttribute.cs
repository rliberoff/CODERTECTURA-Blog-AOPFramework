using System;

using AOPFramework;

namespace AOPExceptionHandlingApplicationBlock
{
    /// <summary>
    /// A custom attribute for exception handling purposes under an Aspect Oriented Programming paradigm.
    /// </summary>
    /// <remarks>
    /// It applies only to methods.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class ExceptionHandlingAttribute : InterceptableAttribute
    {
        /// <summary>
        /// Represents the exception handling politic to apply while intercepting with this concern.
        /// </summary>
        private readonly ExceptionHandlingPolitic politic;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlingAttribute" /> class.
        /// </summary>
        /// <param name="politic">The exception handling politic to follow.</param>
        public ExceptionHandlingAttribute(ExceptionHandlingPolitic politic)
        {
            this.politic = politic;
            this.Processor = Activator.CreateInstance(typeof(ExceptionHandlingProcessor)) as IProcessor;
        }

        /// <summary>
        /// Gets the specified exception handling politic.
        /// </summary>
        /// <value>
        /// Returns the politic to use when handling exceptions represented as a <see cref="ExceptionHandlingPolitic"/>.
        /// </value>
        public ExceptionHandlingPolitic Politic { get { return this.politic; } }
    }
}
