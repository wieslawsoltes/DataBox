using System;

namespace DataBox
{
    [Flags]
    public enum DataBoxGridLinesVisibility
    {
        None = 0,
        Horizontal = 1,
        Vertical = 2,
        All = Vertical | Horizontal
    }
}
