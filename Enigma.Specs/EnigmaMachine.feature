Feature: EnigmaMachine
	In order to to send secret messages
	As a german officer
	I want to be able to encrypt messages before sending them

Background:
	Given I test the following Enigma Machine implementations
	| Author            | Type                                                |
	| Stephane Larocque | EnigmaMachine.Stephane.EnigmaMachine, EnigmaMachine |
#	| <author>          | <Your type implementing IEnigmaMachine>             |

Scenario: Default Settings Encryption
	Given I use an empty plugboard
	And I have the following rotor combination
	| Position | Type | Ring Setting | Starting Position |
	| Left     | I    | A            | A                 |
	| Middle   | II   | A            | A                 |
	| Right    | III  | A            | A                 |
	And I use reflector B
	When I enter the text: AAAAA
	Then I get the following output: BDZGO
	And the current letter position of Left rotor is A
	And the current letter position of Middle rotor is A
	And the current letter position of Right rotor is F

Scenario: Default Settings Decryption
	Given I use an empty plugboard
	And I have the following rotor combination
	| Position | Type | Ring Setting | Starting Position |
	| Left     | I    | A            | A                 |
	| Middle   | II   | A            | A                 |
	| Right    | III  | A            | A                 |
	And I use reflector B
	When I enter the text: BDZGO
	Then I get the following output: AAAAA
	And the current letter position of Left rotor is A
	And the current letter position of Middle rotor is A
	And the current letter position of Right rotor is F

Scenario: Stepping
	Given I use an empty plugboard
	And I have the following rotor combination
	| Position | Type | Ring Setting | Starting Position |
	| Left     | I    | A            | A                 |
	| Middle   | II   | A            | A                 |
	| Right    | III  | A            | V                 |
	And I use reflector B
	When I enter the text: TEST
	Then the current letter position of Left rotor is A
	And the current letter position of Middle rotor is B
	And the current letter position of Right rotor is Z

Scenario: Double Stepping
	Given I use an empty plugboard
	And I have the following rotor combination
	| Position | Type | Ring Setting | Starting Position |
	| Left     | III  | A            | A                 |
	| Middle   | II   | A            | D                 |
	| Right    | I    | A            | O                 |
	And I use reflector B
	When I enter the text: COOKIE
	Then the current letter position of Left rotor is B
	Then the current letter position of Middle rotor is F
	Then the current letter position of Right rotor is U

Scenario: Plugboard
	Given I use the following plugboard mappings
	| From | To |
	| A    | F  |
	| B    | Z  |
	| E    | R  |
	And I have the following rotor combination
	| Position | Type | Ring Setting | Starting Position |
	| Left     | I    | A            | A                 |
	| Middle   | II   | A            | A                 |
	| Right    | III  | A            | A                 |
	And I use reflector B
	When I enter the text: COOKIE
	Then I get the following output: QINHDT

Scenario: Cannot Encode Letter Into Itself
	Given I use an empty plugboard
	And I have the following rotor combination
	| Position | Type | Ring Setting | Starting Position |
	| Left     | I    | A            | A                 |
	| Middle   | II   | A            | A                 |
	| Right    | III  | A            | A                 |
	And I use reflector B
	When I press the letter A repetidly until I reach the following rotor starting position
	| Position | Starting Position |
	| Left     | A                 |
	| Middle   | A                 |
	| Right    | A                 |
	Then the distinct letters of the output must not contain the letter A
	And the distinct letters of the output must have a length of 25

Scenario: Encryption and Decryption
	Given I use a random plugboard
	And I have a random rotor combination
	And I use a random reflector
	When I enter the text: HELLOWORLD
	And I reset the machine
	And I enter the previously encrypted text
	Then I get the following output: HELLOWORLD

Scenario: All Implementations Should Encrypt/Decrypt The Same Way
	Given I use a random plugboard
	And I have a random rotor combination
	And I use a random reflector
	When I enter a random text into the machine
	Then all Enigma implementations should encrypt and decrypt the exact same thing
	