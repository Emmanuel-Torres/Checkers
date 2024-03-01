Feature: Room
  To ensure that room logic works correctly


Scenario: Creating a new private room
  When player O creates a room 'room1' with code '123'
  Then room 'room1' should exist with player O as its owner

Scenario: Joining an existing room with code
  Given player O has a room 'room1' with code '123'
  When player X tries to join room 'room1' with code '123'
  Then player X successfully joined room 'room1'

Scenario: Joining a full room
  Given player O and player X are in room 'room1':'123' 
  When player Y tries to join room 'room1' with code '123'
  Then the action should fail with error 'Cannot join room because it is already full'

Scenario: Joining an existing room with incorrect code
  Given player O has a room 'room1' with code '123'
  When player X tries to join room 'room1' with code '234'
  Then the action should fail with error 'Cannot join room because code was incorrect'

Scenario: Room owner tries joining its own room as a guest
  Given player O has a room 'room1' with code '123'
  When player O tries to join room 'room1' with code '123'
  Then the action should fail with error 'Cannot join room because you are already in the room'