Feature: Room
  To ensure that room logic works correctly

Scenario: Joining an existing room with code
  Given player O has a room 'room1'
  When player X tries to join room 'room1'
  Then player X successfully joined room 'room1'

Scenario: Joining a full room
  Given player O and player X are in room 'room1'
  When player Y tries to join room 'room1'
  Then the action should fail with error 'Cannot join room because it is already full'

Scenario: Room owner tries joining its own room as a guest
  Given player O has a room 'room1'
  When player O tries to join room 'room1'
  Then the action should fail with error 'Cannot join room because you are already in the room'

Scenario: Room owner starts a new game
  Given player O and player X are in room 'room1'
  When player O tries to start a game
  Then a game should now exist for room 'room1'

Scenario: Room owner tries to start a game without a room guest
  Given player O has a room 'room1'
  When player O tries to start a game
  Then the action should fail with error 'Cannot start a game with only one player'

Scenario: Room guest triest to start a game
  Given player O and player X are in room 'room1'
  When player X tries to start a game
  Then the action should fail with error 'Room guest cannot start a game'

Scenario: Room owner tries to start a new game while one is already ongoing
  Given player O and player X are in room 'room1'
  And room 'room1' already has an ongoing game
  When player O tries to start a game
  Then the action should fail with error 'Cannot start a new game while one is already ongoing'

Scenario: Room owner can kick the guest player from the room
  Given player O and player X are in room 'room1'
  And room 'room1' already has an ongoing game
  When player O tries to kick the guest player of room 'room1'
  Then the guest player should no longer be in room 'room1'
  And any ongoing game should be terminated

Scenario: Another player (not room owner) tries to kick guet player from room
  Given player O and player X are in room 'room1'
  When player Y tries to kick the guest player of room 'room1'
  Then the action should fail with error 'Only room owner can kick guest player from room'