﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YololEmulator.Tests.Expressions.Num
{
    [TestClass]
    public class NonEquality
    {
        [TestMethod]
        public void ConstantConstant()
        {
            var result = TestExecutor.Execute("a = 2 ~= 3");

            var a = result.GetVariable("a");

            Assert.AreEqual(1, a.Value.Number);
        }

        [TestMethod]
        public void VariableConstant()
        {
            var result = TestExecutor.Execute("a = 2 b = a ~= 3");

            var a = result.GetVariable("a");
            var b = result.GetVariable("b");

            Assert.AreEqual(2, a.Value.Number);
            Assert.AreEqual(1, b.Value.Number);
        }

        [TestMethod]
        public void VariableVariable()
        {
            var result = TestExecutor.Execute("a = 2 b = 3 c = b ~= a");

            var a = result.GetVariable("a");
            var b = result.GetVariable("b");
            var c = result.GetVariable("c");

            Assert.AreEqual(2, a.Value.Number);
            Assert.AreEqual(3, b.Value.Number);
            Assert.AreEqual(1, c.Value.Number);
        }
    }
}
