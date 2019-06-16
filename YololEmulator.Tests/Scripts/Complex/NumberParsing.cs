﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YololEmulator.Tests.Scripts.Complex
{
    [TestClass]
    public class NumberParsing
    {
        [TestMethod]
        public void ChaosAlpha()
        {
            var lines = new[] {
                "s=\"84726\" if s==\"\" then goto 1 end m=1 n=0 h=s",
                "if h==\"\" then s=h goto 6 else s=h-- t=s-h d=-1 end",
                "d=(t==\"0\")+(t==\"1\")*2+(t==\"2\")*3+(t==\"3\")*4+(t==\"4\")*5+(t==\"5\")*6",
                "d+=(t==\"6\")*7+(t==\"7\")*8+(t==\"8\")*9+(t==\"9\")*10-1",
                "if d==-1 then goto 6 end n+=d*m m*=10 goto 2",
                ":OUTPUT=n :INPUT=\"\" :REST=s goto 1"
            };

            var result = TestExecutor.Execute(lines);
        }
    }
}