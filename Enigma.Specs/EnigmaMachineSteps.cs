using System.Linq;
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
        [Given(@"I use an Enigma machine model M(3|4)")]
        public void GivenIUseAnEnigmaMachineModelM(int rotorCount)
        {
            ScenarioContext.Current.Add("Machine", new MyEnigmaMachine());
        }
        
        [Given(@"I use an empty plugboard")]
        public void GivenIUseAnEmptyPlugboard()
        {
        }
        
        [Given(@"I have the following rotor combination")]
        public void GivenIHaveTheFollowingRotorCombination(Table table)
        {
            var machine = (MyEnigmaMachine)ScenarioContext.Current["Machine"];
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
            var machine = (MyEnigmaMachine)ScenarioContext.Current["Machine"];
            machine.Reflector.SetupRotor(Reflector.Create("Reflector " + reflectorType));
        }
        
        [When(@"I enter the text: ([A-Z]+)")]
        public void WhenIEnterTheText(string text)
        {
            var machine = (MyEnigmaMachine)ScenarioContext.Current["Machine"];
            string cypher = machine.Encrypt(text);
            ScenarioContext.Current.Add("CypherText", cypher);
        }
        
        [Then(@"I get the following output: ([A-Z]+)")]
        public void ThenIGetTheFollowingOutput(string output)
        {
            Assert.AreEqual(output, ScenarioContext.Current["CypherText"]);
        }

        [Then(@"the current letter position of (Left|Middle|Right) rotor is ([A-Z])")]
        public void ThenTheCurrentLetterPositionOfLeftRotorIs(string rotorPosition, char currentDisplayedLetter)
        {
            var machine = (MyEnigmaMachine)ScenarioContext.Current["Machine"];
            RotorSocket socket = GetSocket(machine, rotorPosition);
            Assert.AreEqual(currentDisplayedLetter, socket.CurrentRingLetter);
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

            var machine = (MyEnigmaMachine)ScenarioContext.Current["Machine"];
            machine.Plugboard = new Plugboard(new string(mappings));
        }

        [When(@"I press the letter ([A-Z]) repetidly until I reach the following rotor starting position")]
        public void WhenIPressTheLetterRepetidlyUntilIReachTheFollowingRotorStartingPosition(char letter, Table table)
        {
            var machine = (MyEnigmaMachine)ScenarioContext.Current["Machine"];
            char leftRotorLetter = GetRotorStartingPosition(table, "Left");
            char middleRotorLetter = GetRotorStartingPosition(table, "Middle");
            char rightRotorLetter = GetRotorStartingPosition(table, "Right");

            var sb = new StringBuilder();
            do
            {
                sb.Append(machine.PressKey(letter));
            } while ((machine.SlowRotor.CurrentRingLetter != leftRotorLetter) ||
                     (machine.MiddleRotor.CurrentRingLetter != middleRotorLetter) ||
                     (machine.FastRotor.CurrentRingLetter != rightRotorLetter));

            ScenarioContext.Current["CypherText"] = sb.ToString();
        }

        private char GetRotorStartingPosition(Table table, string position)
        {
            return table.Rows.Single(r => r["Position"] == position)["Starting Position"][0];
        }

        [Then(@"the distinct letters of the output must not contain the letter ([A-Z])")]
        public void ThenTheDistinctLettersOfTheOutputMustNotContainTheLetter(char letter)
        {
            var cypher = (string)ScenarioContext.Current["CypherText"];
            Assert.IsFalse(cypher.ToCharArray().Distinct().Contains(letter));
        }

        [Then(@"the distinct letters of the output must have a length of (\d+)")]
        public void ThenTheDistinctLettersOfTheOutputMustHaveALengthOf(int length)
        {
            var cypher = (string)ScenarioContext.Current["CypherText"];
            Assert.AreEqual(length, cypher.ToCharArray().Distinct().Count());
        }
    }
}
