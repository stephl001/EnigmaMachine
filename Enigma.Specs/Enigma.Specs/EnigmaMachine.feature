Feature: EnigmaMachine
	In order to to send secret messages
	As a german officer
	I want to be able to encrypt messages before sending them

Background:
	Given I use an Enigma machine model M3
	
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
	And the current letter position of Center rotor is A
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
	And the current letter position of Center rotor is A
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
	And the current letter position of Center rotor is B
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
	And the current letter position of Center rotor is F
	And the current letter position of Right rotor is U

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
	Then I get the following output: XBGSHD

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