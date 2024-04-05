Feature: Valid Moves
    To ensure that the valid moves are returned in each Scenario

Scenario: Player X requests moves for a regular piece
  Given the following board with players O and X and player X is moving
  """
    O |   | O |   | O |   | O |   .
      | O |   | O |   | O |   | O .
    O |   | O |   | O |   |   |   .
      |   |   |   |   | O |   |   .
      |   |   |   |   |   |   |   .
      | X |   | X |   | X |   | X .
    X |   | X |   | X |   | X |   .
      | X |   | X |   | X |   | X
  """
  When player X requests the valid moves for location '5,1'
  Then the following moves should be available
  | Destination | MoveSequence |
  | 4,0         | 5,1 > 4,0    |
  | 4,2         | 5,1 > 4,2    |

Scenario: Player O requests moves for a regular piece
  Given the following board with players O and X and player X is moving
  """
    O |   | O |   | O |   | O |   .
      | O |   | O |   | O |   | O .
    O |   | O |   | O |   |   |   .
      |   |   |   |   | O |   |   .
      |   |   |   |   |   |   |   .
      | X |   | X |   | X |   | X .
    X |   | X |   | X |   | X |   .
      | X |   | X |   | X |   | X
  """
  When player O requests the valid moves for location '2,2'
  Then the following moves should be available
  | Destination | MoveSequence |
  | 3,1         | 2,2 > 3,1    |
  | 3,3         | 2,2 > 3,3    |

Scenario: Player X requests the valid moves for a square owned by player O
  Given the following board with players O and X and player X is moving
  """
    O |   | O |   | O |   | O |   .
      | O |   | O |   | O |   | O .
    O |   | O |   | O |   |   |   .
      |   |   |   |   | O |   |   .
      |   |   |   |   |   |   |   .
      | X |   | X |   | X |   | X .
    X |   | X |   | X |   | X |   .
      | X |   | X |   | X |   | X
  """
  When player X requests the valid moves for location '2,2'
  Then no valid moves should be available

Scenario: Player X requests the valid moves for an empty square
  Given the following board with players O and X and player X is moving
  """
    O |   | O |   | O |   | O |   .
      | O |   | O |   | O |   | O .
    O |   | O |   | O |   |   |   .
      |   |   |   |   | O |   |   .
      |   |   |   |   |   |   |   .
      | X |   | X |   | X |   | X .
    X |   | X |   | X |   | X |   .
      | X |   | X |   | X |   | X
  """
  When player X requests the valid moves for location '3,3'
  Then no valid moves should be available

Scenario: Player X requests the valid moves for a King piece
  Given the following board with players O and X and player X is moving
  """
    O |   | O |   | O  |   | O |   .
      | O |   | O |    | O |   | O .
    O |   | O |   | O  |   | O |   .
      |   |   |   |    |   |   |   .
      |   |   |   | X$ |   |   |   .
      | X |   |   |    | X |   | X .
    X |   | X |   | X  |   | X |   .
      | X |   | X |    | X |   | X
  """
  When player X requests the valid moves for location '4,4'
  Then the following moves should be available
  | Destination | MoveSequence |
  | 5,3         | 4,4 > 5,3    |
  | 3,3         | 4,4 > 3,3    |
  | 3,5         | 4,4 > 3,5    |

Scenario: Player O requests the valid moves for a King piece
  Given the following board with players O and X and player X is moving
  """
    O |   | O |   | O  |   | O |   .
      | O |   | O |    | O |   | O .
    O |   | O |   | O  |   | O |   .
      |   |   |   |    |   |   |   .
      |   |   |   | O$ |   |   |   .
      | X |   |   |    | X |   | X .
    X |   | X |   | X  |   | X |   .
      | X |   | X |    | X |   | X
  """
  When player O requests the valid moves for location '4,4'
  Then the following moves should be available
  | Destination | MoveSequence |
  | 5,3         | 4,4 > 5,3    |
  | 3,3         | 4,4 > 3,3    |
  | 3,5         | 4,4 > 3,5    |

Scenario: Player X requests the valid moves for a piece with 1 attack available
  Given the following board with players O and X and player O is moving
  """
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   | O |   .
      |   |   |   |   |   |   | X
  """
  When player X requests the valid moves for location '7,7'
  Then the following moves should be available
  | Destination | MoveSequence |
  | 5,5         | 7,7 > 5,7    |
