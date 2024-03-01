# Feature: Room Manager
#   To ensure that rooms are properly managed

# Scenario: Player O creates a new room
#   When player O creates a room 'room1'
#   Then room with id 'room1' should exist

# Scenario: Player X tries to join an existing room
#   Given player O has a room 'room1'
#   When player X tries to join room 'room1'
#   Then room 'room1' should have player O and X

# Scenario: Player X tires to join a non-existing room
#   When player X tries to join room 'room1'
#   Then the action should fail with error '...'

# Scenario: Player Y tries to join a full room
#   Given player X has a room 'room1' with player O
#   When player Y tries to join room 'room1'
#   Then the action should fail with error '...'