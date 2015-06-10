using System;
using EnigmaMachine.Stephane;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnigmaMachine.Tests.Stephane
{
    [TestClass]
    public class SocketTests
    {
        [TestMethod]
        public void TestRotorOffsets()
        {
            var socket = new RotorSocket(Rotor.Create("I"));
            Assert.AreEqual('A', socket.CurrentRingLetter);
            socket.Advance();
            Assert.AreEqual('B', socket.CurrentRingLetter);
            socket.Reset();
            Assert.AreEqual('A', socket.CurrentRingLetter);
            for (int i=0; i<26; i++)
                socket.Advance();
            Assert.AreEqual('A', socket.CurrentRingLetter);
        }

        [TestMethod]
        public void TestLetterMappings()
        {
            var socket = new RotorSocket(Rotor.Create("I"));
            Assert.AreEqual('E', socket.GetMappedLetter('A'));
            Assert.AreEqual('A', socket.GetMappedLetter('E', LetterMapper.MappingDirection.LeftToRight));

            //If for example rotor I is in the B-position, an A enters at the letter B 
            //which is wired to the K. Because of the offset this K enters the next rotor in the J position.
            socket.Advance();
            Assert.AreEqual('J', socket.GetMappedLetter('A'));
            Assert.AreEqual('A', socket.GetMappedLetter('J', LetterMapper.MappingDirection.LeftToRight));
        }

        [TestMethod]
        public void TestLetterMappingsWithRotorOffsetSettings()
        {
            var socket = new RotorSocket(Rotor.Create("I", 'B'));
            Assert.AreEqual('K', socket.GetMappedLetter('A'));
            Assert.AreEqual('A', socket.GetMappedLetter('K', LetterMapper.MappingDirection.LeftToRight));
            socket.Advance();
            Assert.AreEqual('E', socket.GetMappedLetter('A'));
            Assert.AreEqual('A', socket.GetMappedLetter('E', LetterMapper.MappingDirection.LeftToRight));
        }

        [TestMethod]
        public void TestNotch()
        {
            var socket = new RotorSocket(Rotor.Create("I"));
            while (!socket.IsSocketInNotchPosition)
                socket.Advance();
            Assert.AreEqual('Q', socket.CurrentRingLetter);
        }
    }
}
