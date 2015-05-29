using System;
using System.Linq;
using EnigmaMachine.Stephane;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnigmaMachine.Tests.Stephane
{
    [TestClass]
    public class PlugboardTests
    {
        private static readonly char[] AlphabetLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        [TestMethod]
        public void TestNoConnection()
        {
            var plugboard = new Plugboard();
            var rightLeftMappings = AlphabetLetters.Select(c => plugboard.GetMappedLetter(c)).ToArray();
            var leftRightMappings = AlphabetLetters.Select(c => plugboard.GetMappedLetter(c, LetterMapper.MappingDirection.LeftToRight)).ToArray();
            Assert.IsTrue(rightLeftMappings.Zip(leftRightMappings, (c1, c2) => c1 == c2).All(res => res));

            Assert.IsTrue(rightLeftMappings.Zip(AlphabetLetters, (c1,c2) => c1 == c2).All(res => res));
        }

        [TestMethod]
        public void TestConnections()
        {
            var plugboard = new Plugboard("ABCKEFGHIJDLNMOPQRSTUZWXYV");
            var rightLeftMappings = AlphabetLetters.Select(c => plugboard.GetMappedLetter(c)).ToArray();
            var leftRightMappings = rightLeftMappings.Select(c => plugboard.GetMappedLetter(c, LetterMapper.MappingDirection.LeftToRight)).ToArray();
            Assert.IsTrue(AlphabetLetters.Zip(leftRightMappings, (c1, c2) => c1 == c2).All(res => res));
        }
    }
}
