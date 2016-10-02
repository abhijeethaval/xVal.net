﻿using System;
using Xunit;

namespace XVal.Core.Tests
{
    public class MessageFormatterTests
    {
        [Fact]
        public void ConstructorThrowsIfFormatIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new MessageFormatter<object>(null));
            Assert.Equal("Value cannot be null." + Environment.NewLine + "Parameter name: format", exception.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void ConstructorThrowsIfFormatIsEmptyOrSpace(string message)
        {
            var exception = Assert.Throws<ArgumentException>(() => new MessageFormatter<object>(message));
            Assert.Equal("Value cannot be empty string or white space." + Environment.NewLine + "Parameter name: format", exception.Message);
        }

        [Fact]
        public void GetMessageWithoutFormatParameter()
        {
            var format = "This is message";
            var employee = GetEmployee();
            var messageFormatter = new MessageFormatter<Employee>(format);
            var message = messageFormatter.GetMessage(employee);
            Assert.Equal(format, message);
        }

        [Fact]
        public void GetsMessageWithFormatParameter()
        {
            var format = "Id is {0}";
            var employee = GetEmployee();
            var messageFormatter = new MessageFormatter<Employee>(format, e => e.Id);
            var message = messageFormatter.GetMessage(employee);
            Assert.Equal(string.Format(format, employee.Id), message);
        }

        [Fact]
        public void GetsMessageWithMultipleFormatParameters()
        {
            var format = "Fullname is {0} {1}";
            var employee = GetEmployee();
            var messageFromatter = new MessageFormatter<Employee>(format, e => e.Firstname, e => e.Lastname);
            var message = messageFromatter.GetMessage(employee);
            Assert.Equal(string.Format(format, employee.Firstname, employee.Lastname), message);
        }

        public Employee GetEmployee()
        {
            return new Employee { Id = 1, Firstname = "Ashish", Lastname = "Lokare" };
        }
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
