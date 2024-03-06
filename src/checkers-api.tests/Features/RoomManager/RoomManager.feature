Feature: Room Manager
  To ensure that the room manager works as intended

Scenario: Creating a room
  Given a room manager exists
  When player O creates room 'room1' with code '123'
  Then room 'room1' should now exist
  And player O now exists in the player-room list

Scenario: Creating multiple rooms at once
  Given a room manager exists
  When the following rooms are created
  | Player | RoomId | RoomCode |
  | O      | room1  | 123      |
  | P      | room2  | 123      |
  | Q      | room3  | 123      |
  | R      | room4  | 123      |
  | S      | room5  | 123      |
  | T      | room6  | 123      |
  Then the following rooms should exist
  | Rooms  |
  | room1  |
  | room2  |
  | room3  |
  | room4  |
  | room5  |
  | room6  |

Scenario: Creating a room with id of existing room
  Given a room manager exists
  And the following rooms exist
  | Player | RoomId | RoomCode |
  | O      | room1  | 123      |
  When player X creates room 'room1' with code '123'
  Then the action should fail with error 'Cannot create room because room id already exists'

Scenario: Creating a room while already being in one
  Given a room manager exists
  And the following rooms exist
  | Player | RoomId | RoomCode |
  | O      | room1  | 123      |
  When player O creates room 'room2' with code '123'
  Then the action should fail with error 'Cannot create room because player is already in a room'

Scenario: Joining an available room with code
  Given a room manager exists
  And the following rooms exist
  | Player | RoomId | RoomCode |
  | O      | room1  | 123      |
  When player X tries to join room 'room1' with code '123'
  Then player X successfully joined room 'room1'
  And player X now exists in the player-room list

Scenario: Joining a room that does not exist
  Given a room manager exists
  And the following rooms exist
  | Player | RoomId | RoomCode |
  | O      | room1  | 123      |
  When player X tries to join room 'room2' with code '123'
  Then the action should fail with error 'Cannot complete action <Join Room> because room does not exist'

Scenario: Joining a room when player is already in another room
  Given a room manager exists
  And the following rooms exist
  | Player | RoomId | RoomCode |
  | O      | room1  | 123      |
  | P      | room2  | 123      |
  When player O tries to join room 'room2' with code '123'
  Then the action should fail with error 'Cannot join room because player is already in a room'

Scenario: A kicked player is removed from room-game list
  Given a room manager exists
  And player O and player X are in room 'room1':'123'
  When player O tries to kick the guest player of room 'room1'
  Then player X should not exists in player-room list

Scenario: Removing a room also removes players from player-room list
  Given a room manager exists
  And player O and player X are in room 'room1':'123'
  When room 'room1' gets removed
  Then room 'room1' should not exist
  And player O should not exists in player-room list
  And player X should not exists in player-room list

