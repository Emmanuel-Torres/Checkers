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
  | 5,5         | 7,7 > 5,5    |

Scenario: Player X requests the valid moves for a piece with 1 attack move and 1 regular move
  Given the following board with players O and X and player O is moving
  """
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   | O |   .
      |   |   |   |   | X |   |  
  """
  When player X requests the valid moves for location '7,5'
  Then the following moves should be available
  | Destination | MoveSequence |
  | 5,7         | 7,5 > 5,7    |
  | 6,4         | 7,5 > 6,4    |

Scenario: Player X requests the valid moves for a piece with 2 attack moves
  Given the following board with players O and X and player O is moving
  """
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   | O |   | O |   .
      |   |   |   |   | X |   |   .
      |   |   |   | O |   | O |   .
      |   |   |   |   |   |   |  
  """
  When player X requests the valid moves for location '5,5'
  Then the following moves should be available
  | Destination | MoveSequence |
  | 3,7         | 5,5 > 3,7    |
  | 3,3         | 5,5 > 3,3    |

Scenario: Player O requests the valid moves for a piece with 2 attack moves
  Given the following board with players O and X and player O is moving
  """
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   | X |   | X |   .
      |   |   |   |   | O |   |   .
      |   |   |   | X |   | X |   .
      |   |   |   |   |   |   |  
  """
  When player O requests the valid moves for location '5,5'
  Then the following moves should be available
  | Destination | MoveSequence |
  | 7,7         | 5,5 > 7,7    |
  | 7,3         | 5,5 > 7,3    |

Scenario: Player X requests the valid moves for a king piece with 4 attack moves
  Given the following board with players O and X and player O is moving
  """
      |   |   |   |   |    |   |   .
      |   |   |   |   |    |   |   .
      |   |   |   |   |    |   |   .
      |   |   |   |   |    |   |   .
      |   |   |   | O |    | O |   .
      |   |   |   |   | X$ |   |   .
      |   |   |   | O |    | O |   .
      |   |   |   |   |    |   |  
  """
  When player X requests the valid moves for location '5,5'
  Then the following moves should be available
  | Destination | MoveSequence |
  | 3,7         | 5,5 > 3,7    |
  | 3,3         | 5,5 > 3,3    |
  | 7,7         | 5,5 > 7,7    |
  | 7,3         | 5,5 > 7,3    |

Scenario: Player X requests the valid moves for a regular piece with 1 double jump attack available
  Given the following board with players O and X and player O is moving
  """
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   | O |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   | O |   .
      |   |   |   |   |   |   | X
  """
  When player X requests the valid moves for location '7,7'
  Then the following moves should be available
  | Destination | MoveSequence    |
  | 5,5         | 7,7 > 5,5       |
  | 3,3         | 7,7 > 5,5 > 3,3 |

Scenario: Player X requests the valid moves for a regular piece with 1 linear triple jump attack available
  Given the following board with players O and X and player O is moving
  """
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   | O |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   | O |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   | O |   .
      |   |   |   |   |   |   | X
  """
  When player X requests the valid moves for location '7,7'
  Then the following moves should be available
  | Destination | MoveSequence          |
  | 5,5         | 7,7 > 5,5             |
  | 3,3         | 7,7 > 5,5 > 3,3       |
  | 1,1         | 7,7 > 5,5 > 3,3 > 1,1 |

Scenario: Player X requests the valid moves for a regular piece with 1 triple jump and 1 double jump
  Given the following board with players O and X and player O is moving
  """
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   | O |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   | O |   | O |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   | O |   .
      |   |   |   |   |   |   | X
  """
  When player X requests the valid moves for location '7,7'
  Then the following moves should be available
  | Destination | MoveSequence          |
  | 5,5         | 7,7 > 5,5             |
  | 3,3         | 7,7 > 5,5 > 3,3       |
  | 1,1         | 7,7 > 5,5 > 3,3 > 1,1 |
  | 3,7         | 7,7 > 5,5 > 3,7       |

Scenario: Player X requests the valid moves for a regular piece with Y looking attack pattern
  Given the following board with players O and X and player O is moving
  """
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   | O |   |   |   | O |   .
      |   |   |   |   |   |   |   .
      |   |   |   | O |   | O |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   | O |   .
      |   |   |   |   |   |   | X
  """
  When player X requests the valid moves for location '7,7'
  Then the following moves should be available
  | Destination | MoveSequence          |
  | 5,5         | 7,7 > 5,5             |
  | 3,3         | 7,7 > 5,5 > 3,3       |
  | 1,1         | 7,7 > 5,5 > 3,3 > 1,1 |
  | 3,7         | 7,7 > 5,5 > 3,7       |
  | 1,5         | 7,7 > 5,5 > 3,7 > 1,5 |

Scenario: Player X requests the valid moves for a king piece with a single attack move
  Given the following board with players O and X and player O is moving
  """
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   | O |   .
      |   |   |   |   |   |   | X$
  """
  When player X requests the valid moves for location '7,7'
  Then the following moves should be available
  | Destination | MoveSequence |
  | 5,5         | 7,7 > 5,5    |

Scenario: Player X requests the valid moves for a king piece with 1 double jump attack available
  Given the following board with players O and X and player O is moving
  """
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   | O |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   | O |   .
      |   |   |   |   |   |   | X$
  """
  When player X requests the valid moves for location '7,7'
  Then the following moves should be available
  | Destination | MoveSequence    |
  | 5,5         | 7,7 > 5,5       |
  | 3,3         | 7,7 > 5,5 > 3,3 |

Scenario: Player X requests the valid moves for a piece that can turn into king piece
  Given the following board with players O and X and player O is moving
  """
      |   |   |   |   |   |   |   .
      | O |   | O |   |   |   |   .
    X |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |   .
      |   |   |   |   |   |   |  
  """
  When player X requests the valid moves for location '2,0'
  Then the following moves should be available
  | Destination | MoveSequence          |
  | 0,2         | 2,0 > 0,2             |
  | 2,4         | 2,0 > 0,2 > 2,4       |

Scenario: Player X requests the valid moves for a king piece with two double jumps that end in same destination
  Given the following board with players O and X and player O is moving
  """
      |   |   |   |   |    |   |   .
      |   |   |   |   |    |   |   .
      |   |   |   |   |    |   |   .
      |   |   |   |   |    |   |   .
      |   |   |   | O |    | O |   .
      |   |   |   |   |    |   |   .
      |   |   |   | O |    | O |   .
      |   |   |   |   | X$ |   |  
  """
  When player X requests the valid moves for location '7,5'
  Then the following moves should be available
  | Destination | MoveSequence                |
  | 5,7         | 7,5 > 5,7                   |
  | 3,5         | 7,5 > 5,7 > 3,5             |
  | 5,3         | 7,5 > 5,7 > 3,5 > 5,3       |
  | 7,5         | 7,5 > 5,7 > 3,5 > 5,3 > 7,5 |
  | 5,3         | 7,5 > 5,3                   |
  | 3,5         | 7,5 > 5,3 > 3,5             |
  | 5,7         | 7,5 > 5,3 > 3,5 > 5,7       |
  | 7,5         | 7,5 > 5,3 > 3,5 > 5,7 > 7,5 |