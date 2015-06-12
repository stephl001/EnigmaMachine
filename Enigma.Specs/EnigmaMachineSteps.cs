<<<<<<< Updated upstream
﻿using System.Linq;
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
>>>>>>> Stashed changes
using System.Text;
using EnigmaMachine.Stephane;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using MyEnigmaMachine = EnigmaMachine.Stephane.EnigmaMachine;

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
        public void GivenIHaveTheFollowingRotorCombination(Table table)
        {
<<<<<<< Updated upstream
            var machine = (MyEnigmaMachine)ScenarioContext.Current["Machine"];
=======
            LoopMachineImplementations(m => m.SetupRotors(rotorInfos.ToArray()));
        }

        [StepArgumentTransformation]
        public IEnumerable<RotorInfo> TableToRotorInfoTransform(Table table)
        {
            var rotorDefinitions = new RotorInfo[3];
>>>>>>> Stashed changes
            foreach (TableRow row in table.Rows)
            {
                var rotor = Rotor.Create(row["Type"], row["Ring Setting"][0]);
                RotorSocket socket = GetSocket(machine, row["Position"]);
                socket.SetupRotor(rotor, row["Starting Position"][0]);
            }
        }

        private RotorSocket GetSocket(MyEnigmaMachine machine, string position)
        {
            if (position == "Left")
                return machine.SlowRotor;
            if (position == "Middle")
                return machine.MiddleRotor;

            return machine.FastRotor;
        }
        
        [Given(@"I use reflector (A|B|C)")]
        public void GivenIUseReflector(string reflectorType)
        {
<<<<<<< Updated upstream
            var machine = (MyEnigmaMachine)ScenarioContext.Current["Machine"];
            machine.Reflector.SetupRotor(Reflector.Create("Reflector " + reflectorType));
=======
            LoopMachineImplementations(m => m.SetupReflector("Reflector " + reflectorType));
>>>>>>> Stashed changes
        }
        
        [When(@"I enter the text: ([A-Z]+)")]
        public void WhenIEnterTheText(string text)
        {
<<<<<<< Updated upstream
            var machine = (MyEnigmaMachine)ScenarioContext.Current["Machine"];
            string cypher = machine.Encrypt(text);
            ScenarioContext.Current.Add("CypherText", cypher);
=======
            int index = 0;
            LoopMachineImplementations(m =>
            {
                string cypher = m.Encrypt(text);
                ScenarioContext.Current.Add("CypherText" + (index++), cypher);
            });
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
            var machine = (MyEnigmaMachine)ScenarioContext.Current["Machine"];
            RotorSocket socket = GetSocket(machine, rotorPosition);
            Assert.AreEqual(currentDisplayedLetter, socket.CurrentRingLetter);
=======
            LoopMachineImplementations(m =>
            {
                char[] currentRingLetters = m.GetCurrentRotorRingLetters();
                char currentLetter = (rotorPosition == "Left")
                    ? currentRingLetters[0]
                    : ((rotorPosition == "Middle") ? currentRingLetters[1] : currentRingLetters[2]);
                Assert.AreEqual(currentDisplayedLetter, currentLetter);
            });
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
            var machine = (MyEnigmaMachine)ScenarioContext.Current["Machine"];
            machine.Plugboard = new Plugboard(new string(mappings));
=======
            LoopMachineImplementations(m => m.SetupPlugboard(mappings));
>>>>>>> Stashed changes
        }

        [When(@"I press the letter ([A-Z]) repetidly until I reach the following rotor starting position")]
        public void WhenIPressTheLetterRepetidlyUntilIReachTheFollowingRotorStartingPosition(char letter, Table table)
        {
<<<<<<< Updated upstream
            var machine = (MyEnigmaMachine)ScenarioContext.Current["Machine"];
=======
>>>>>>> Stashed changes
            char leftRotorLetter = GetRotorStartingPosition(table, "Left");
            char middleRotorLetter = GetRotorStartingPosition(table, "Middle");
            char rightRotorLetter = GetRotorStartingPosition(table, "Right");

            int index = 0;
            LoopMachineImplementations(m =>
            {
<<<<<<< Updated upstream
                sb.Append(machine.PressKey(letter));
            } while ((machine.SlowRotor.CurrentRingLetter != leftRotorLetter) ||
                     (machine.MiddleRotor.CurrentRingLetter != middleRotorLetter) ||
                     (machine.FastRotor.CurrentRingLetter != rightRotorLetter));
=======
                var sb = new StringBuilder();
                do
                {
                    sb.Append(m.PressKey(letter));
                } while (!m.GetCurrentRotorRingLetters().SequenceEqual(new[] { leftRotorLetter, middleRotorLetter, rightRotorLetter }));
>>>>>>> Stashed changes

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
    }
}
