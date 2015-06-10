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
            Assert.IsNotNull(machine.Plugboard);
            Assert.IsNotNull(machine.FastRotor);
            Assert.IsNotNull(machine.MiddleRotor);
            Assert.IsNotNull(machine.SlowRotor);
            Assert.IsNotNull(machine.Reflector);
            Assert.AreEqual('A', machine.FastRotor.CurrentRingLetter);
            Assert.AreEqual('A', machine.MiddleRotor.CurrentRingLetter);
            Assert.AreEqual('A', machine.SlowRotor.CurrentRingLetter);
        }

        [TestMethod]
        public void TestRotors()
        {
            var machine = new MyEnigmaMachine();
            RepeatKey(machine, 'A', 26);
            Assert.AreEqual('A', machine.FastRotor.CurrentRingLetter);
            Assert.AreEqual('B', machine.MiddleRotor.CurrentRingLetter);
            RepeatKey(machine, 'A', 25*26);
            Assert.AreEqual('B', machine.SlowRotor.CurrentRingLetter);
        }

        [TestMethod]
        public void TestRotorsDoubleStepping()
        {
            var machine = new MyEnigmaMachine();
            machine.MiddleRotor.SetRingLetter('D');
            machine.FastRotor.SetRingLetter('O');
            RepeatKey(machine, 'A', 6);
            Assert.AreEqual('B', machine.SlowRotor.CurrentRingLetter);
            Assert.AreEqual('F', machine.MiddleRotor.CurrentRingLetter);
            Assert.AreEqual('U', machine.FastRotor.CurrentRingLetter);
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
            } while (machine.SlowRotor.CurrentRingLetter != 'A' || machine.MiddleRotor.CurrentRingLetter != 'A' || machine.FastRotor.CurrentRingLetter != 'A');
            
            Assert.AreEqual(26*26*25, count);
        }

        
        [TestMethod]
        public void TestEncryptionDefaultSettings()
        {
            var machine = new MyEnigmaMachine();
            string cypher = machine.Encrypt("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            Assert.AreEqual("FUVEPUMWARVQKEFGHGDIJFMFXI", cypher);
            machine.Reset();
            cypher = machine.Encrypt("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            Assert.AreEqual("FUVEPUMWARVQKEFGHGDIJFMFXI", cypher);
            machine.Reset();
            cypher = machine.Encrypt("FUVEPUMWARVQKEFGHGDIJFMFXI");
            Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ", cypher);

        }

        [TestMethod]
        public void TestEncryptionRingStartingLettertSettings()
        {
            var machine = new MyEnigmaMachine();
            machine.FastRotor.SetRingLetter('Q');
            machine.MiddleRotor.SetRingLetter('R');
            machine.SlowRotor.SetRingLetter('F');

            string cypher = machine.Encrypt("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            Assert.AreEqual("MHHKTNIROWJNYMNWKHMVEZQHWU", cypher);
            machine.Reset();
            cypher = machine.Encrypt("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            Assert.AreEqual("MHHKTNIROWJNYMNWKHMVEZQHWU", cypher);
            machine.Reset();
            cypher = machine.Encrypt("MHHKTNIROWJNYMNWKHMVEZQHWU");
            Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ", cypher);

        }

        [TestMethod]
        public void TestEncryptionRingOffsetSettings()
        {
            var machine = new MyEnigmaMachine();
            machine.FastRotor.SetupRotor(Rotor.Create("III", 'B'));
            machine.MiddleRotor.SetupRotor(Rotor.Create("II", 'B'));
            machine.SlowRotor.SetupRotor(Rotor.Create("I", 'B'));
            
            string cypher = machine.Encrypt("AAAAA");
            Assert.AreEqual("EWTYX", cypher);
            machine.Reset();
            cypher = machine.Encrypt("AAAAA");
            Assert.AreEqual("EWTYX", cypher);
            machine.Reset();
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
            machine.FastRotor.SetupRotor(Rotor.Create("II", 'T'), 'W');
            machine.MiddleRotor.SetupRotor(Rotor.Create("V", 'U'), 'C');
            machine.SlowRotor.SetupRotor(Rotor.Create("IV", 'F'), 'M');
            machine.Plugboard = new Plugboard("ABCKEFGHIJDLNMOPQRSTUZWXYV");
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
