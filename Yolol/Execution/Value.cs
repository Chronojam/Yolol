﻿using System;
using System.Globalization;
using JetBrains.Annotations;
using Yolol.Grammar.AST.Expressions;

namespace Yolol.Execution
{
    public struct Value
        : IEquatable<Value>
    {
        public Type Type { get; }

        private readonly Number _number;
        public Number Number
        {
            get
            {
                if (Type != Type.Number)
                    throw new InvalidCastException($"Attempted to access value of type {Type} as a Number");
                return _number;
            }
        }

        private readonly string _string;
        public string String
        {
            get
            {
                if (Type != Type.String)
                    throw new InvalidCastException($"Attempted to access value of type {Type} as a String");
                return _string;
            }
        }

        public Value(string str)
        {
            _string = str;
            _number = new Number(0);
            Type = Type.String;
        }

        public Value(Number num)
        {
            _string = "";
            _number = num;
            Type = Type.Number;
        }

        public Value(bool @bool)
            : this(@bool ? 1 : 0)
        {
        }

        public static implicit operator Value(Number n)
        {
            return new Value(n);
        }

        public static implicit operator Value(decimal d)
        {
            return new Value(new Number(d));
        }

        public static implicit operator Value(string s)
        {
            return new Value(s);
        }

        public override string ToString()
        {
            if (Type == Type.Number)
                return Number.ToString(CultureInfo.InvariantCulture);

            return String;
        }

        public bool ToBool()
        {
            if (Type == Type.String)
                return true;
            else
                return Number != 0;
        }

        public object ToObject()
        {
            if (Type == Type.Number)
                return Number.Value;
            else
                return String;
        }

        public bool Equals(Value other)
        {
            switch (Type, other.Type)
            {
                case (Type.Number, Type.Number):
                    return Number == other.Number;

                case (Type.String, Type.String):
                    return String.Equals(other.String, StringComparison.OrdinalIgnoreCase);

                default:
                    return false;
            }

        }

        public override bool Equals(object obj)
        {
            return obj is Value other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)Type * 397;

                if (Type == Type.String)
                    hashCode += _string?.GetHashCode() ?? 1;
                else
                    hashCode += _number.GetHashCode();

                return hashCode;
            }
        }

        public static Value operator <(Value left, Value right)
        {
            switch (left.Type, right.Type)
            {
                case (Type.Number, Type.Number):
                    return new Value(left.Number < right.Number ? 1 : 0);

                default:
                    var l = left.ToString();
                    var r = right.ToString();
                    var comparison = StringComparer.OrdinalIgnoreCase.Compare(l, r);
                    return new Value(comparison < 0 ? 1 : 0);
            }
        }

        public static Value operator <=(Value left, Value right)
        {
            switch (left.Type, right.Type)
            {
                case (Type.Number, Type.Number):
                    return new Value(left.Number <= right.Number ? 1 : 0);

                default:
                    var l = left.ToString();
                    var r = right.ToString();
                    var comparison = StringComparer.OrdinalIgnoreCase.Compare(l, r);
                    return new Value(comparison <= 0 ? 1 : 0);
            }
        }

        public static Value operator >(Value left, Value right)
        {
            switch (left.Type, right.Type)
            {
                case (Type.Number, Type.Number):
                    return new Value(left.Number > right.Number ? 1 : 0);

                default:
                    var l = left.ToString();
                    var r = right.ToString();
                    var comparison = StringComparer.OrdinalIgnoreCase.Compare(l, r);
                    return new Value(comparison > 0 ? 1 : 0);
            }
        }

        public static Value operator >=(Value left, Value right)
        {
            switch (left.Type, right.Type)
            {
                case (Type.Number, Type.Number):
                    return new Value(left.Number >= right.Number ? 1 : 0);

                default:
                    var l = left.ToString();
                    var r = right.ToString();
                    var comparison = StringComparer.OrdinalIgnoreCase.Compare(l, r);
                    return new Value(comparison >= 0 ? 1 : 0);
            }
        }

        public static Value operator ==(Value left, Value right)
        {
            return left.Equals(right) ? 1 : 0;
        }

        public static Value operator !=(Value left, Value right)
        {
            return !left.Equals(right) ? 1 : 0;
        }

        public static Value operator -(Value left, Value right)
        {
            switch (left.Type, right.Type)
            {
                case (Type.Number, Type.Number):
                    return new Value(left.Number - right.Number);

                default:
                    var l = left.ToString();
                    var r = right.ToString();

                    var index = l.LastIndexOf(r, StringComparison.Ordinal);

                    if (index == -1)
                        return new Value(l);
                    else
                        return new Value(l.Remove(index, r.Length));
            }
        }

        public static Value operator +(Value left, Value right)
        {
            switch (left.Type, right.Type)
            {
                case (Type.Number, Type.Number):
                    return new Value(left.Number + right.Number);

                default:
                    return left.ToString() + right.ToString();
            }
        }

        public static Value operator *(Value left, Value right)
        {
            switch (left.Type, right.Type)
            {
                case (Type.Number, Type.Number):
                    return new Value(left.Number * right.Number);

                case (Type.String, Type.String):
                    throw new ExecutionException("Attempted to multiply strings");

                default:
                    throw new ExecutionException("Attempted to multiply mixed types");
            }
        }

        public static Value operator /(Value left, Value right)
        {
            switch (left.Type, right.Type)
            {
                case (Type.Number, Type.Number):
                    return new Value(left.Number / right.Number);

                case (Type.String, Type.String):
                    throw new ExecutionException("Attempted to divide strings");

                default:
                    throw new ExecutionException("Attempted to divide mixed types");
            }
        }

        public static Value operator &(Value left, Value right)
        {
            switch (left.Type, right.Type)
            {
                case (Type.Number, Type.Number):
                    return new Value(left.Number != 0 && right.Number != 0);

                case (Type.String, Type.String):
                    return new Value(true);

                case (Type.String, Type.Number):
                    return new Value(right.Number != 0);

                default:
                case (Type.Number, Type.String):
                    return new Value(left.Number != 0);
            }
        }

        public static Value operator |(Value left, Value right)
        {
            switch (left.Type, right.Type)
            {
                case (Type.Number, Type.Number):
                    return new Value(left.Number != 0 || right.Number != 0);

                default:
                    return new Value(true);
            }
        }

        public static Value operator %(Value left, Value right)
        {
            switch (left.Type, right.Type)
            {
                case (Type.Number, Type.Number):
                    if (right.Number == 0)
                        throw new ExecutionException("Divide by zero");
                    return new Value(left.Number.Value % right.Number.Value);

                case (Type.String, Type.String):
                    throw new ExecutionException("Attempted to modulo strings");

                default:
                    throw new ExecutionException("Attempted to modulo mixed types");
            }
        }

        public static Value Exponent(Value left, Value right)
        {
            switch (left.Type, right.Type)
            {
                case (Type.Number, Type.Number):
                    var v = Math.Pow((double)left.Number.Value, (double)right.Number.Value);

                    if (double.IsPositiveInfinity(v))
                        return new Value(Number.MaxValue);

                    if (double.IsNegativeInfinity(v) || double.IsNaN(v))
                        return new Value(Number.MinValue);

                    return new Value((decimal)v);

                case (Type.String, Type.String):
                    throw new ExecutionException("Attempted to exponent strings");

                default:
                    throw new ExecutionException("Attempted to exponent mixed types");
            }
        }

        [NotNull] public BaseExpression ToConstant()
        {
            if (Type == Type.Number)
                return new ConstantNumber(Number);
            else
                return new ConstantString(String);
        }
    }
}
