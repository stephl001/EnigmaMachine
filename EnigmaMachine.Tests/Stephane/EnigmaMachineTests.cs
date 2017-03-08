using EnigmaMachine.Stephane;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyEnigmaMachine = EnigmaMachine.Stephane.EnigmaMachine;

namespace EnigmaMachine.Tests.Stephane
{
    [TestClass]
    public class EnigmaMachineTests
    {
        [TestMethod]
        public void TestDefaultSettings()
        {
            var machine = new MyEnigmaMachine();
            char[] ringLetters = machine.GetCurrentRotorRingLetters();
            Assert.AreEqual('A', ringLetters[2]);
            Assert.AreEqual('A', ringLetters[1]);
            Assert.AreEqual('A', ringLetters[0]);
        }

        [TestMethod]
        public void TestRotors()
        {
            var machine = new MyEnigmaMachine();
            RepeatKey(machine, 'A', 26);
            char[] ringLetters = machine.GetCurrentRotorRingLetters();
            Assert.AreEqual('A', ringLetters[2]);
            Assert.AreEqual('B', ringLetters[1]);
            RepeatKey(machine, 'A', 25*26);
            ringLetters = machine.GetCurrentRotorRingLetters();
            Assert.AreEqual('B', ringLetters[0]);
        }

        [TestMethod]
        public void TestRotorsDoubleStepping()
        {
            var machine = new MyEnigmaMachine();
            machine.SetStartupRotorRingLetters(new [] {'A', 'D', 'O'});
            RepeatKey(machine, 'A', 6);
            char[] ringLetters = machine.GetCurrentRotorRingLetters();
            Assert.AreEqual('B', ringLetters[0]);
            Assert.AreEqual('F', ringLetters[1]);
            Assert.AreEqual('U', ringLetters[2]);
        }

        [TestMethod]
        public void TestFullCycle()
        {
            var machine = new MyEnigmaMachine();
            int count = 0;
            do
            {
                machine.PressKey('A');
                count++;
            } while (machine.GetCurrentRotorRingLetters()[0] != 'A' || machine.GetCurrentRotorRingLetters()[1] != 'A' || machine.GetCurrentRotorRingLetters()[2] != 'A');
            
            Assert.AreEqual(26*26*25, count);
        }

        
        [TestMethod]
        public void TestEncryptionDefaultSettings()
        {
            var machine = new MyEnigmaMachine();
            string cypher = machine.Encrypt("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            Assert.AreEqual("FUVEPUMWARVQKEFGHGDIJFMFXI", cypher);
            machine.ResetRotors();
            cypher = machine.Encrypt("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            Assert.AreEqual("FUVEPUMWARVQKEFGHGDIJFMFXI", cypher);
            machine.ResetRotors();
            cypher = machine.Encrypt("FUVEPUMWARVQKEFGHGDIJFMFXI");
            Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ", cypher);

        }

        [TestMethod]
        public void TestEncryptionRingStartingLetterSettings()
        {
            var machine = new MyEnigmaMachine();
            machine.SetStartupRotorRingLetters(new[] {'F', 'R', 'Q'});
            string cypher = machine.Encrypt("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            Assert.AreEqual("MHHKTNIROWJNYMNWKHMVEZQHWU", cypher);
            machine.ResetRotors();
            cypher = machine.Encrypt("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            Assert.AreEqual("MHHKTNIROWJNYMNWKHMVEZQHWU", cypher);
            machine.ResetRotors();
            cypher = machine.Encrypt("MHHKTNIROWJNYMNWKHMVEZQHWU");
            Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ", cypher);

        }

        [TestMethod]
        public void TestEncryptionRingOffsetSettings()
        {
            var machine = new MyEnigmaMachine();
            machine.SetupRotors(new[] {new RotorInfo("I", 'A', 'B'), new RotorInfo("II", 'A', 'B'), new RotorInfo("III", 'A', 'B')});
            
            string cypher = machine.Encrypt("AAAAA");
            Assert.AreEqual("EWTYX", cypher);
            machine.ResetRotors();
            cypher = machine.Encrypt("AAAAA");
            Assert.AreEqual("EWTYX", cypher);
            machine.ResetRotors();
            cypher = machine.Encrypt("EWTYX");
            Assert.AreEqual("AAAAA", cypher);
        }

        [TestMethod]
        public void TestNonCharEncryption()
        {
            var machine = new MyEnigmaMachine();
            string cypher = machine.Encrypt("THIS IS A TEST; ANOTHER TEST. HELLO, WORLD!");
            Assert.AreEqual("ZPJJ SV S PGBW; WWIUKOG FXYM. LPRFQ, ZPVKW!", cypher);
        }

        [TestMethod]
        public void TestLowercaseEncryption()
        {
            var machine = new MyEnigmaMachine();
            string cypher = machine.Encrypt("This is a test; Another test. Hello, World!");
            Assert.AreEqual("Zpjj sv s pgbw; Wwiukog fxym. Lprfq, Zpvkw!", cypher);
        }

        [TestMethod]
        public void TestFullParameters()
        {
            var machine = new MyEnigmaMachine();
            machine.SetupRotors(new[] { new RotorInfo("IV", 'M', 'F'), new RotorInfo("V", 'C', 'U'), new RotorInfo("II", 'W', 'T') });
            machine.SetupPlugboard("ABCKEFGHIJDLNMOPQRSTUZWXYV");
            string cypher = machine.Encrypt("This is a test");
            Assert.AreEqual("Rmxe li c wmua", cypher);
        }

        private void RepeatKey(MyEnigmaMachine machine, char p1, int p2)
        {
            for (int i = 0; i < p2; i++)
                machine.PressKey(p1);
        }
    }
}
