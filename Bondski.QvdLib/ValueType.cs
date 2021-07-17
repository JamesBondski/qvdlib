namespace Bondski.QvdLib
{
    /// <summary>
    /// Enum detailing what kind of value it is.
    /// </summary>
    public enum ValueType
    {
        /// <summary>
        /// Indicates a NULL value
        /// </summary>
        Null = 0,

        /// <summary>
        /// Integer, 0x01 (4 bytes)
        /// </summary>
        Int = 1,

        /// <summary>
        /// Double, 0x02 (8 bytes)
        /// </summary>
        Double = 2,

        /// <summary>
        /// String, 0x03 (null-terminated)
        /// </summary>
        String = 4,

        /// <summary>
        /// Dual, first an int, then the string representation
        /// </summary>
        DualInt = 5,

        /// <summary>
        /// Dual Double, first a double, then the string representation
        /// </summary>
        DualDouble = 6,
    }
}
