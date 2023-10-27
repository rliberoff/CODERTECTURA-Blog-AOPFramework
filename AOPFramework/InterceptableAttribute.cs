using System;

namespace AOPFramework
{
    /// <summary>
    /// Defines or decorates a class as interceptable. An interceptable class can be processed by a class that implements the <c>IProcessor</c> interface.
    /// </summary>    
    public abstract class InterceptableAttribute : Attribute
    {
        /// <summary>
        /// Sets or gets the processor for this class.
        /// </summary>
        public IProcessor Processor { get; set; }

        /// <summary>
        /// Sets or gets this attribute processing priority.
        /// </summary>
        /// <remarks>
        /// The bigger the value of this property, the bigger the importance or priority of processing
        /// this attribute.
        /// </remarks>
        public int Priority { get; set; }
    }
}
