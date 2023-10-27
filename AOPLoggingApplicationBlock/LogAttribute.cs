using System;
using System.Diagnostics;

using AOPFramework;

namespace AOPLoggingApplicationBlock
{
    /// <summary>
    /// A custom attribute for logging purposes under an Aspect Oriented Programming paradigm.
    /// </summary>
    /// <remarks>
    /// It applies only to methods.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class LogAttribute : InterceptableAttribute
    {
        /// <summary>
        /// A stopwatch to use then the verbosity level is set to its highest value, in order to
        /// log the amount of time consumed by methods calls.
        /// </summary>
        private readonly Stopwatch timer;

        /// <summary>
        /// The verbosity level to log.
        /// </summary>
        private readonly VerbosityLevel verbosity;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogAttribute" /> class.
        /// </summary>
        /// <param name="verbosity">The verbosity level to log.</param>        
        public LogAttribute(VerbosityLevel verbosity)
        {
            this.verbosity = verbosity;
            this.Processor = Activator.CreateInstance(typeof(LogProcessor)) as IProcessor;

            if (verbosity.Equals(VerbosityLevel.Full))
            {
                this.timer = new Stopwatch();
                this.timer.Reset();
            }
        }

        /// <summary>
        /// Gets the verbosity level to log.
        /// </summary>
        /// <value>The verbosity level of the log trace as defined in <see cref="VerbosityLevel"/>.</value>
        public VerbosityLevel Verbosity { get { return this.verbosity; } }

        /// <summary>
        /// Gets the this log timer.
        /// </summary>
        /// <remarks>
        /// If the <c>Verbosity</c> is different than <see cref="VerbosityLevel.Full"/> then this property will return <c>null</c>.
        /// </remarks>
        /// <value>
        /// An instance of a <see cref="Stopwatch"/> to retrieve execution time when logging with <see cref="VerbosityLevel.Full"/>.
        /// </value>
        /// <see cref="Stopwatch"/>
        /// <see cref="AOPLoggingApplicationBlock.VerbosityLevel"/>
        public Stopwatch Timer { get { return this.timer; } }
    }
}
