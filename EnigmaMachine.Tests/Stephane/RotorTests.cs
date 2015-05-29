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
            {"ETW", new char[] {}}
        };
            
        [TestMethod]
        public void TestDefaultRotorMappings()
        {
            foreach (var kvp in ExpectedMappings)
            {
                var rotor = Rotor.Create(kvp.Key);
                IEnumerable<char> mappedLetters = AlphabetLetters.Select(letter => rotor.GetMappedLetter(letter));
                Assert.IsTrue(kvp.Value.SequenceEqual(mappedLetters));    
            }
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
            LoopAllRotorsAndOffsets(TestRotorOffset);
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
            IEnumerable<char> mappedLetters = AlphabetLetters.Select(letter => rotorWithOffset.GetMappedLetter(letter));
            Assert.IsTrue(newMapping.SequenceEqual(mappedLetters));
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
            IEnumerable<char> reversedLetters = mappedLetters.Select(l => rotor.GetMappedLetter(l, Rotor.MappingDirection.LeftToRight));

            Assert.IsTrue(AlphabetLetters.Zip(reversedLetters, (c1, c2) => c1 == c2).All(b => b));
        }
    }
}
