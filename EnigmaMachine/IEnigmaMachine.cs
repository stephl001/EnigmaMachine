using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnigmaMachine
{
    public interface IEnigmaMachine
    {
        char PressKey(char letter);
        void SetupPlugboard(string mappings);
        void SetupRotors(RotorInfo[] rotorInfos);
        void SetupReflector(string type);
        char[] GetCurrentRotorRingLetters();
        void ResetRotors();
    }
}
