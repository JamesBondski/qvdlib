namespace Bondski.QvdLib
{
    /// <summary>
    /// Contains one value from the value table.
    /// </summary>
    public readonly struct Value
    {
        /// <summary>
        /// Gets the Integer representaton of the value if applicable.
        /// </summary>
        public readonly int Int { get; init; }

        /// <summary>
        /// Gets the Double representation of the value if applicable.
        /// </summary>
        public readonly double Double { get; init; }

        /// <summary>
        /// Gets the string representation of the value if applicable.
        /// </summary>
        public readonly string String { get; init; }

        /// <summary>
        /// Gets the Qlik type of this value.
        /// </summary>
        public readonly ValueType Type { get; init; }
    }

    
}
