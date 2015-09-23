# Enigma Machine
![alt text](http://upload.wikimedia.org/wikipedia/commons/thumb/3/3e/EnigmaMachineLabeled.jpg/768px-EnigmaMachineLabeled.jpg "")

An Enigma machine was any of several electro-mechanical rotor cipher machines used in the twentieth century for enciphering and deciphering secret messages. Enigma was invented by the German engineer Arthur Scherbius at the end of World War I.[1] Early models were used commercially from the early 1920s, and adopted by military and government services of several countries, most notably Nazi Germany before and during World War II.[2] Several different Enigma models were produced, but the German military models are the most commonly recognised.

German military messages enciphered on the Enigma machine were first broken by the Polish Cipher Bureau, beginning in December 1932. This success was a result of efforts by three Polish cryptologists, Marian Rejewski, Jerzy Różycki and Henryk Zygalski, working for Polish military intelligence. Rejewski reverse-engineered the device, using theoretical mathematics and material supplied by French military intelligence. Subsequently the three mathematicians designed mechanical devices for breaking Enigma ciphers, including the cryptologic bomb. From 1938 onwards, additional complexity was repeatedly added to the Enigma machines, making decryption more difficult and requiring further equipment and personnel—more than the Poles could readily produce.

On 25 July 1939, in Warsaw, the Poles initiated French and British military intelligence representatives into their Enigma-decryption techniques and equipment, including Zygalski sheets and the cryptologic bomb, and promised each delegation a Polish-reconstructed Enigma. The demonstration represented a vital basis for the later British continuation and effort.[3] During the war, British cryptologists decrypted a vast number of messages enciphered on Enigma. The intelligence gleaned from this source, codenamed "Ultra" by the British, was a substantial aid to the Allied war effort.[4]

Though Enigma had some cryptographic weaknesses, in practice it was German procedural flaws, operator mistakes, failure to systematically introduce changes in encipherment procedures, and Allied capture of key tables and hardware that, during the war, enabled Allied cryptologists to succeed.

## How Enigma Machines Work
The underlying principle of an Enigma machine cipher is that of letter substitution, meaning that each letter of our plaintext (undeciphered message) is substituted by another letter.

## The Journey Of A Single Letter
The Enigma machine is an electro-mechanical device. It is mechanically operated, with an electric signal passed through wires and various mechanical parts. The easiest way to explain the mechanics is to follow the journey of a single letter from keyboard to lampboard.

The diagram below (figure 1) shows the path the signal takes from pressing the letter 'T' on the keyboard to the 'G' lamp lighting up.

![alt text](http://enigma.louisedade.co.uk/wiringdiagram.png "Figure 1: How one letter is changed into another letter at each stage as it passes through an Enigma machine.")

## Keyboard
When the operator presses the letter 'T' on the keyboard it creates an electric signal that begins the journey through the Enigma machine wiring that will end with a lamp flashing on the lampboard.

## Plugboard
The first stop on the journey is the plugboard. Here the signal is connected to the 'T' input on the plugboard. Some of the letters on the plugboard will be wired up to other letters (the plugs), causing the signal to be diverted. If the 'T' input is not plugged to another letter then our signal will pass straight to the 'T output. In our case, though the 'T' is plugged to the 'K', so the signal is diverted to a new path, the letter is now 'K'.

## Static Rotor
The next stop is the static rotor, which as the name suggests does nothing to the signal it simply turns wires into contacts (the signal only passes when the contacts touch). So our signal is still the letter 'K'. The static rotor output is connected to the input of the right rotor. This is where things get more complicated.

## Rotors (Scramblers)
There are five possible rotors that can be used in any order for the three rotor positions: right, middle, left. Each rotor has an inner ring of contacts and an outer ring of contacts and their purpose is to scramble the signal. The outer ring contacts connect each rotor to the next rotor (or the static rotor / reflector) as well as its own inner ring. The inner ring contacts can be rotated relative to the outer ring which results in even more possible connections (and therefore, letter substitutions). The whole rotor itself can be rotated relative to the static rotor, so that the static rotor 'A' output is not connected to 'A' input on the rotating rotor.

Furthermore, as each letter is entered the rotors rotate by one position, so that the same letters are never connected together in the same message. To add further complication, each rotor has a notches (different rotors have the notch in different positions) which when reached, causes the next rotor to its left to step forward too. In the case of the middle rotor, it causes the left rotor to step as well as itself (the infamous double stepping mechanism).

In our example, we are using rotor III in the right-hand position.

## Reflector
The reflector takes the input and reflects back the electrical signal for its return journey through the rotors. There are two possible reflectors, each of which is wired up differently so that the input letter is transformed to a different letter when reflected back. In our example, we are using 'Reflector B', which turns our input letter 'H' into output letter 'D'.

It is important that the signal is scrambled when reflected, because of the way the Enigma machine is designed -- if you enter the cipher text you get back the clear text. So if the reflector output is the same letter as its input when the signal passes back through the rotors they will just unscramble what was already scrambled and you would get your original letter back again unencrypted!

## Reverse Journey
The reflected signal now passes back through the rotors, which work in exactly the same way in reverse. So our letter 'D' passes through the left rotor and becomes 'G', which then passes through the middle rotor and becomes 'R', which then passes through the right rotor and becomes 'W'. The signal remains unchanged as it passes through the static rotor again (connecting contacts to wires), before it passes through the plugboard - here the signal is again left as it is if there is no plug, or changed if the letter 'W' is plugged to another letter. In our case the 'W' is plugged to the letter 'G', so our plugboard output is 'G'.

## Lampboard
The final stop is the lampboard, where the plugboard output is connected to the corresponding lamp for that letter. In our example, the letter 'G' lights up meaning the original letter 'T' is encrypted as 'G'.

The Enigma machine operator notes down the output letter and then enters the next letter in the message, and so on for every letter in the message.

__Reference: The text above was taken from [the following site][enigma ref]__
[enigma ref]: http://www.reddit.com
----------
## Rotor Specification
The following link points to the official rotor specification. You will need this information to properly implement your Enigma machine.

[Rotors Specifications](https://github.com/stephl001/EnigmaMachine/wiki/Rotors-Specifications)
