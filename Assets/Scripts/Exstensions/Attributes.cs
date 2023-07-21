using System;

internal class FixAttribute : Attribute
{
    public FixAttribute()
    {
        
    }
    public FixAttribute(string comment)
    {
    }
}

internal class BadPerformanceAttribute : Attribute
{
}

internal class UsedInAnimatorAttribute : Attribute
{
}

internal class DeleteAttribute : Attribute
{
}
