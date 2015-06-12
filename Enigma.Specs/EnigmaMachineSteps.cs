﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
﻿using EnigmaMachine;
﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
﻿using MyEnigmaMachine = EnigmaMachine.Stephane.EnigmaMachine;

namespace Enigma.Specs
{
    [Binding]
    public class EnigmaMachineSteps
    {
        [Given(@"I test the following Enigma Machine implementations")]
        public void GivenITestTheFollowingEnigmaMachineImplementations(Table table)
        {
            IEnigmaMachine[] allMachines = table.Rows.Select(tr => Activator.CreateInstance(Type.GetType(tr["Type"], true), null)).Cast<IEnigmaMachine>().ToArray();
            ScenarioContext.Current["Machines"] = allMachines;
        }

        
        [Given(@"I use an empty plugboard")]
        public void GivenIUseAnEmptyPlugboard()
        {
        }

        private void LoopMachineImplementations(Action<IEnigmaMachine> handler)
        {
            var allMachines = (IEnigmaMachine[])ScenarioContext.Current["Machines"];
            foreach (IEnigmaMachine machine in allMachines)
                handler(machine);
        }
        
        [Given(@"I have the following rotor combination")]
        public void GivenIHaveTheFollowingRotorCombination(IEnumerable<RotorInfo> rotorInfos)
        {
            LoopMachineImplementations(m => m.SetupRotors(rotorInfos.ToArray()));
        }

        [StepArgumentTransformation]
        public IEnumerable<RotorInfo> TableToRotorInfoTransform(Table table)
        {
            var rotorDefinitions = new RotorInfo[3];
            foreach (TableRow row in table.Rows)
            {
                var rotorInfo = new RotorInfo(row["Type"], row["Starting Position"][0], row["Ring Setting"][0]);
                if (row["Position"] == "Left")
                    rotorDefinitions[0] = rotorInfo;
                else if (row["Position"] == "Middle")
                    rotorDefinitions[1] = rotorInfo;
                else
                    rotorDefinitions[2] = rotorInfo;
            }

            return rotorDefinitions;
        }
        
        [Given(@"I use reflector (A|B|C)")]
        public void GivenIUseReflector(string reflectorType)
        {
            LoopMachineImplementations(m => m.SetupReflector("Reflector " + reflectorType));
        }
        
        [When(@"I enter the text: ([A-Z]+)")]
        public void WhenIEnterTheText(string text)
        {
            int index = 0;
            LoopMachineImplementations(m =>
            {
                string cypher = m.Encrypt(text);
                ScenarioContext.Current.Add("CypherText" + (index++), cypher);
            });
        }
        
        [Then(@"I get the following output: ([A-Z]+)")]
        public void ThenIGetTheFollowingOutput(string output)
        {
            int index = 0;
            LoopMachineImplementations(m => Assert.AreEqual(output, ScenarioContext.Current["CypherText" + (index++)]));
        }

        [Then(@"the current letter position of (Left|Middle|Right) rotor is ([A-Z])")]
        public void ThenTheCurrentLetterPositionOfLeftRotorIs(string rotorPosition, char currentDisplayedLetter)
        {
            LoopMachineImplementations(m =>
            {
                char[] currentRingLetters = m.GetCurrentRotorRingLetters();
                char currentLetter = (rotorPosition == "Left")
                    ? currentRingLetters[0]
                    : ((rotorPosition == "Middle") ? currentRingLetters[1] : currentRingLetters[2]);
                Assert.AreEqual(currentDisplayedLetter, currentLetter);
            });
        }

        [Given(@"I use the following plugboard mappings")]
        public void GivenIUseTheFollowingPlugboardMappings(Table table)
        {
            char[] mappings = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            foreach (TableRow row in table.Rows)
            {
                char from = row["From"][0];
                char to = row["To"][0];
                mappings[from - 'A'] = to;
                mappings[to - 'A'] = from;
            }

            LoopMachineImplementations(m => m.SetupPlugboard(mappings));
        }

        [When(@"I press the letter ([A-Z]) repetidly until I reach the following rotor starting position")]
        public void WhenIPressTheLetterRepetidlyUntilIReachTheFollowingRotorStartingPosition(char letter, Table table)
        {
            char leftRotorLetter = GetRotorStartingPosition(table, "Left");
            char middleRotorLetter = GetRotorStartingPosition(table, "Middle");
            char rightRotorLetter = GetRotorStartingPosition(table, "Right");

            int index = 0;
            LoopMachineImplementations(m =>
            {
                var sb = new StringBuilder();
                do
                {
                    sb.Append(m.PressKey(letter));
                } while (!m.GetCurrentRotorRingLetters().SequenceEqual(new[] { leftRotorLetter, middleRotorLetter, rightRotorLetter }));

                ScenarioContext.Current["CypherText" + (index++)] = sb.ToString();
            });
        }

        private char GetRotorStartingPosition(Table table, string position)
        {
            return table.Rows.Single(r => r["Position"] == position)["Starting Position"][0];
        }

        [Then(@"the distinct letters of the output must not contain the letter ([A-Z])")]
        public void ThenTheDistinctLettersOfTheOutputMustNotContainTheLetter(char letter)
        {
            int index = 0;
            LoopMachineImplementations(m =>
            {
                var cypher = (string)ScenarioContext.Current["CypherText" + (index++)];
                Assert.IsFalse(cypher.ToCharArray().Distinct().Contains(letter));
            });
        }

        [Then(@"the distinct letters of the output must have a length of (\d+)")]
        public void ThenTheDistinctLettersOfTheOutputMustHaveALengthOf(int length)
        {
            int index = 0;
            LoopMachineImplementations(m =>
            {
                var cypher = (string)ScenarioContext.Current["CypherText" + (index++)];
                Assert.AreEqual(length, cypher.ToCharArray().Distinct().Count());
            });
        }

        private static readonly Random Random = new Random();

        [Given(@"I use a random plugboard")]
        public void GivenIUseARandomPlugboard()
        {
            string mapping = GenerateRandomMapping();
            LoopMachineImplementations(m => m.SetupPlugboard(mapping));
        }

        private static string GenerateRandomMapping()
        {
            char[] mapping = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            int connections = Random.Next(0, 14);
            var availablePositions = new List<int> {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25};
            for (int i = 0; i < connections; i++)
            {
                int randomPos1 = availablePositions[Random.Next(availablePositions.Count)];
                availablePositions.Remove(randomPos1);
                int randomPos2 = availablePositions[Random.Next(availablePositions.Count)];
                availablePositions.Remove(randomPos2);

                char tmp = mapping[randomPos1];
                mapping[randomPos1] = mapping[randomPos2];
                mapping[randomPos2] = tmp;
            }

            return new string(mapping);
        }

        [Given(@"I have a random rotor combination")]
        public void GivenIHaveARandomRotorCombination()
        {
            var rotorInfos = new []
            {
                new RotorInfo(GetRandomRotorType(), GetRandomLetter(), GetRandomLetter()),
                new RotorInfo(GetRandomRotorType(), GetRandomLetter(), GetRandomLetter()),
                new RotorInfo(GetRandomRotorType(), GetRandomLetter(), GetRandomLetter())
            };
            LoopMachineImplementations(m => m.SetupRotors(rotorInfos));
        }

        private static readonly string[] AvailableRotorTypes = {"I", "II", "III", "IV", "V", "VI", "VII", "VIII"};
        private string GetRandomRotorType()
        {
            return AvailableRotorTypes[Random.Next(AvailableRotorTypes.Length)];
        }

        private static char GetRandomLetter(char minLetter = 'A', char maxLetter = 'Z')
        {
            return (char)(Random.Next(maxLetter - minLetter + 1) + minLetter);
        }

        [Given(@"I use a random reflector")]
        public void GivenIUseARandomReflector()
        {
            string type = ("Reflector " + GetRandomLetter('A', 'C'));
            LoopMachineImplementations(m => m.SetupReflector(type));
        }

        [When(@"I reset the machine")]
        public void WhenIResetTheMachine()
        {
            LoopMachineImplementations(m => m.ResetRotors());
        }

        [When(@"I enter the previously encrypted text")]
        public void WhenIEnterThePreviouslyEncryptedText()
        {
            int index = 0;
            LoopMachineImplementations(m =>
            {
                ScenarioContext.Current["CypherText" + (index)] = m.Encrypt((string)ScenarioContext.Current["CypherText" + (index++)]);
            });
        }
    }
}
