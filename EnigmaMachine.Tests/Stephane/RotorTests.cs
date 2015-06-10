using System;
using System.Collections.Generic;
using System.Linq;
using EnigmaMachine.Stephane;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnigmaMachine.Tests.Stephane
{
    [TestClass]
    public class RotorTests
    {
        private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static readonly char[] AlphabetLetters = Alphabet.ToCharArray();

        private static readonly IDictionary<string,char[]> ExpectedMappings = new Dictionary<string, char[]>
        {
            {"I", "EKMFLGDQVZNTOWYHXUSPAIBRCJ".ToCharArray()},
            {"II", "AJDKSIRUXBLHWTMCQGZNPYFVOE".ToCharArray()},
            {"III", "BDFHJLCPRTXVZNYEIWGAKMUSQO".ToCharArray()},
            {"IV", "ESOVPZJAYQUIRHXLNFTGKDCMWB".ToCharArray()},
            {"V", "VZBRGITYUPSDNHLXAWMJQOFECK".ToCharArray()},
            {"VI", "JPGVOUMFYQBENHZRDKASXLICTW".ToCharArray()},
            {"VII", "NZJHGRCXMYSWBOUFAIVLPEKQDT".ToCharArray()},
            {"VIII", "FKQHTLXOCBJSPDZRAMEWNIUYGV".ToCharArray()},
            {"Reflector A", "EJMZALYXVBWFCRQUONTSPIKHGD".ToCharArray()},
            {"Reflector B", "YRUHQSLDPXNGOKMIEBFZCWVJAT".ToCharArray()},
            {"Reflector C", "FVPJIAOYEDRZXWGCTKUQSBNMHL".ToCharArray()},
            {"ETW", AlphabetLetters}
        };

        private static readonly IDictionary<string, char[]> ExpectedNotches = new Dictionary<string, char[]>
        {
            {"I", "Q".ToCharArray()},
            {"II", "E".ToCharArray()},
            {"III", "V".ToCharArray()},
            {"IV", "J".ToCharArray()},
            {"V", "Z".ToCharArray()},
            {"VI", "ZM".ToCharArray()},
            {"VII", "ZM".ToCharArray()},
            {"VIII", "ZM".ToCharArray()},
            {"Reflector A", new char[] {}},
            {"Reflector B", new char[] {}},
            {"Reflector C", new char[] {}},
            {"ETW", new char[] {}}
        };
            
        [TestMethod]
        public void TestDefaultRotorMappings()
        {
            foreach (var kvp in ExpectedMappings)
            {
                var rotor = Rotor.Create(kvp.Key);
                IEnumerable<char> mappedLetters = AlphabetLetters.Select(letter => rotor.GetMappedLetter(letter));
                Assert.IsTrue(SequencesOrderedEqual(kvp.Value, mappedLetters));    
            }
        }

        private static bool SequencesOrderedEqual(IEnumerable<char> seq1, IEnumerable<char> seq2)
        {
            return seq1.Zip(seq2, (c1, c2) => c1 == c2).All(b => b);
        }

        [TestMethod]
        public void TestRotorNotches()
        {
            foreach (var kvp in ExpectedNotches)
            {
                var rotor = Rotor.Create(kvp.Key);
                IEnumerable<bool> notches = BuildNotchesPredicateFromLetters(kvp.Value);
                Assert.IsTrue(AlphabetLetters.Zip(notches, (letter, isNotch) => rotor.IsNotch(letter) == isNotch).All(res => res));
            }
        }

        private IEnumerable<bool> BuildNotchesPredicateFromLetters(params char[] letters)
        {
            foreach (char letter in AlphabetLetters)
            {
                if (letters.Contains(letter))
                {
                    yield return true;
                    continue;
                }

                yield return false;
            }
        }

        
        [TestMethod]
        public void TestRotorRingSetting()
        {
            //LoopAllRotorsAndOffsets(TestRotorOffset);
            var rotorWithOffset = Rotor.Create("I");
            Assert.AreEqual('E', rotorWithOffset.GetMappedLetter('A'));
            Assert.AreEqual('A', rotorWithOffset.GetMappedLetter('E', LetterMapper.MappingDirection.LeftToRight));
            rotorWithOffset = Rotor.Create("I", 'B');
            Assert.AreEqual('K', rotorWithOffset.GetMappedLetter('A'));
            Assert.AreEqual('A', rotorWithOffset.GetMappedLetter('K', LetterMapper.MappingDirection.LeftToRight));
            Assert.AreEqual('F', rotorWithOffset.GetMappedLetter('B'));
            Assert.AreEqual('B', rotorWithOffset.GetMappedLetter('F', LetterMapper.MappingDirection.LeftToRight));
        }

        private void LoopAllRotorsAndOffsets(Action<string, char> handler)
        {
            foreach (string rotorType in ExpectedMappings.Keys)
            {
                foreach (char offset in AlphabetLetters)
                {
                    handler(rotorType, offset);
                }
            }
        }

        private void TestRotorOffset(string rotorType, char offset)
        {
            var rotorWithOffset = Rotor.Create(rotorType, offset);

            IEnumerable<char> newMapping = RotateLeft(ExpectedMappings[rotorType], offset - 'A');
            IEnumerable<char> mappedLetters = AlphabetLetters.Select(letter => rotorWithOffset.GetMappedLetter(letter)).ToArray();
            Assert.IsTrue(SequencesOrderedEqual(newMapping, mappedLetters));

            var reverseSequence = mappedLetters.Select(letter => rotorWithOffset.GetMappedLetter(letter, LetterMapper.MappingDirection.LeftToRight));
            Assert.IsTrue(SequencesOrderedEqual(reverseSequence, AlphabetLetters));
        }

        private IEnumerable<char> RotateLeft(char[] array, int steps)
        {
            steps = steps%array.Length;

            var rotatedList = new List<char>();
            rotatedList.AddRange(array.Skip(steps));
            rotatedList.AddRange(array.Take(steps));

            return rotatedList.ToArray();
        }

        [TestMethod]
        public void TestRotorReverseMapping()
        {
            LoopAllRotorsAndOffsets(TestRotorReflection);
        }

        private void TestRotorReflection(string rotorType, char offset)
        {
            var rotor = Rotor.Create(rotorType, offset);
            IEnumerable<char> mappedLetters = AlphabetLetters.Select(l => rotor.GetMappedLetter(l)).ToArray();
            IEnumerable<char> reversedLetters = mappedLetters.Select(l => rotor.GetMappedLetter(l, LetterMapper.MappingDirection.LeftToRight));

            Assert.IsTrue(SequencesOrderedEqual(AlphabetLetters, reversedLetters));
        }
    }
}
