Feature: Room
  To ensure that room logic works correctly

Scenario: Creating a new public room
  When player O creates a room 'room1'
  Then public room 'room1' should exist with player O as its owner

Scenario: Creating a new private room
  When player O creates a private room 'room1' with code '123'
  Then private room 'room1' should exist with player O as its owner

# Scenario: Joining an existing public room
#   Given player O has a public room 'room1'
#   When player X tries to join room 'room1'
#   Then player X successfully joined room 'room1'

# Scenario: Joining a full room
#   Given player O and player X are in room 'room1'
#   When player Y tries to join room 'room1'
#   Then the action should fail error 'Cannot join room1 becuase it is already full'

# Scenario: Joining an existing private room with code
#   Given player O has a private room 'room1' with code '123'
#   When player X tries to join room 'room1' with code '123'
#   Then player X successfully joined room 'room1'

# Scenario: Joining an existing private room without the code
#   Given player O has a private room 'room1' with code '123'
#   When player X tries to join room 'room1'
#   Then the action should fail with error 'Cannot join private room room1 becuase the wrong code was provided'
