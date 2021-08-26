using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Bondski.QvdLib.Tests
{
    public class ValueTest
    {
        [Fact]
        public void StringTest()
        {
            Value val = new Value()
            {
                String = "123",
                Type = ValueType.String
            };

            Assert.Equal("123", val.AsString());
            Assert.Throws<InvalidValueException>( () => val.AsInt() );
            Assert.Throws<InvalidValueException>( () => val.AsDouble() );

            Assert.Equal("123", val.ToString());
            Assert.Equal(123, val.ToInt(true));
            Assert.Throws<InvalidValueException>( () => val.ToInt(false) );
            Assert.Throws<InvalidValueException>(() => val.ToInt());
            Assert.Equal(123, val.ToDouble(true));
            Assert.Throws<InvalidValueException>(() => val.ToDouble(false));
            Assert.Throws<InvalidValueException>(() => val.ToDouble());
        }

        [Fact]
        public void IntTest()
        {
            Value val = new Value()
            {
                Int = 123,
                Type = ValueType.Int
            };

            Assert.Equal(123, val.AsInt());
            Assert.Throws<InvalidValueException>(() => val.AsString());
            Assert.Throws<InvalidValueException>(() => val.AsDouble());

            Assert.Equal("123", val.ToString());
            Assert.Equal(123, val.ToInt(true));
            Assert.Equal(123, val.ToInt(false));
            Assert.Equal(123, val.ToInt());
            Assert.Equal(123, val.ToDouble(true));
            Assert.Equal(123, val.ToDouble(false));
            Assert.Equal(123, val.ToDouble());
        }

        [Fact]
        public void DoubleTest()
        {
            Value val = new Value()
            {
                Double = 123,
                Type = ValueType.Double
            };

            Assert.Equal(123, val.AsDouble());
            Assert.Throws<InvalidValueException>(() => val.AsString());
            Assert.Throws<InvalidValueException>(() => val.AsInt());

            Assert.Equal("123", val.ToString());
            Assert.Equal(123, val.ToInt(true));
            Assert.Equal(123, val.ToInt(false));
            Assert.Equal(123, val.ToInt());
            Assert.Equal(123, val.ToDouble(true));
            Assert.Equal(123, val.ToDouble(false));
            Assert.Equal(123, val.ToDouble());
        }

        [Fact]
        public void DualIntTest()
        {
            Value val = new Value()
            {
                Int = 123,
                String = "ABC",
                Type = ValueType.DualInt
            };

            Assert.Equal(123, val.AsInt());
            Assert.Equal("ABC", val.AsString());
            Assert.Throws<InvalidValueException>(() => val.AsDouble());

            Assert.Equal("ABC", val.ToString());
            Assert.Equal(123, val.ToInt(true));
            Assert.Equal(123, val.ToInt(false));
            Assert.Equal(123, val.ToInt());
            Assert.Equal(123, val.ToDouble(true));
            Assert.Equal(123, val.ToDouble(false));
            Assert.Equal(123, val.ToDouble());
        }

        [Fact]
        public void DualDoubleTest()
        {
            Value val = new Value()
            {
                Double = 123,
                String = "ABC",
                Type = ValueType.DualDouble
            };

            Assert.Equal(123, val.AsDouble());
            Assert.Equal("ABC", val.AsString());
            Assert.Throws<InvalidValueException>(() => val.AsInt());

            Assert.Equal("ABC", val.ToString());
            Assert.Equal(123, val.ToInt(true));
            Assert.Equal(123, val.ToInt(false));
            Assert.Equal(123, val.ToInt());
            Assert.Equal(123, val.ToDouble(true));
            Assert.Equal(123, val.ToDouble(false));
            Assert.Equal(123, val.ToDouble());
        }

        [Fact]
        public void NullTest()
        {
            Value val = new Value()
            {
                Type = ValueType.Null
            };

            Assert.Null(val.AsString());
            Assert.Null(val.AsInt());
            Assert.Null(val.AsDouble());

            Assert.Null(val.ToString());
            Assert.Null(val.ToInt());
            Assert.Null(val.ToInt(true));
            Assert.Null(val.ToInt(false));
            Assert.Null(val.ToDouble());
            Assert.Null(val.ToDouble(true));
            Assert.Null(val.ToDouble(false));
        }
    }
}
