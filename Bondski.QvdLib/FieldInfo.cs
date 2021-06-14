// <copyright file="FieldInfo.cs" company="Matthias Kersting">
// Copyright (c) Matthias Kersting. All rights reserved.
// </copyright>

namespace Bondski.QvdLib
{
    /// <summary>
    /// Holds information about a field in the QVD file.
    /// </summary>
    public record FieldInfo(string name, int? bitOffset);
}
