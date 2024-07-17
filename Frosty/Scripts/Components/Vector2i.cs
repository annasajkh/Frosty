using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Frosty.Scripts.Components;

public readonly struct Vector2i : IEquatable<Vector2i>, IFormattable
{
    public int X { get; }
    public int Y { get; }

    public Vector2i(int x, int y)
    {
        X = x;
        Y = y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector2i left, Vector2i right)
    {
        return left.X == right.X && left.Y == right.Y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator +(Vector2i left, Vector2i right)
    {
        return new Vector2i(left.X + right.X, left.Y + right.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator -(Vector2i left, Vector2i right)
    {
        return new Vector2i(left.X - right.X, left.Y - right.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator *(Vector2i left, Vector2i right)
    {
        return new Vector2i(left.X * right.X, left.Y * right.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator /(Vector2i left, Vector2i right)
    {
        return new Vector2i(left.X / right.X, left.Y / right.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2i operator %(Vector2i left, Vector2i right)
    {
        return new Vector2i(left.X % right.X, left.Y % right.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector2i left, Vector2i right)
    {
        return !(left == right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Vector2i other && Equals(other);
    }

    public static explicit operator Vector2i(Vector2 value)
    {
        return new Vector2i((int)value.X, (int)value.Y);
    }

    public readonly bool Equals(Vector2i other)
    {
        return this == other;
    }

    public override readonly int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public override readonly string ToString()
    {
        return ToString("G", CultureInfo.CurrentCulture);
    }

    public readonly string ToString(string? format)
    {
        return ToString(format, CultureInfo.CurrentCulture);
    }

    public readonly string ToString(string? format, IFormatProvider? formatProvider)
    {
        StringBuilder stringBuilder = new StringBuilder();

        string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

        stringBuilder.Append('<');
        stringBuilder.Append(X.ToString(format, formatProvider));
        stringBuilder.Append(separator);
        stringBuilder.Append(' ');
        stringBuilder.Append(Y.ToString(format, formatProvider));
        stringBuilder.Append('>');

        return stringBuilder.ToString();
    }
}