Feature: Game
    To ensure that my game logic is correct

Scenario: Generating default board board
    When I start a game with players O and X
    Then the board should look like this
    """
     O |   | O |   | O |   | O |   |
       | O |   | O |   | O |   | O |
     O |   | O |   | O |   | O |   |
       |   |   |   |   |   |   |   |
       |   |   |   |   |   |   |   |
       | X |   | X |   | X |   | X |
     X |   | X |   | X |   | X |   |
       | X |   | X |   | X |   | X
    """

Scenario: Generating a game with a starter board
  Given the following board with players O and X
  """
    O |   | O |   | O |   | O |   |
      | O |   | O |   | O |   | O |
    O |   | O |   | O |   |   |   |
      |   |   |   |   | O |   |   |
      |   |   |   |   |   |   |   |
      | X |   | X |   | X |   | X |
    X |   | X |   | X |   | X |   |
      | X |   | X |   | X |   | X
  """
  Then the board should look like this
  """
    O |   | O |   | O |   | O |   |
      | O |   | O |   | O |   | O |
    O |   | O |   | O |   |   |   |
      |   |   |   |   | O |   |   |
      |   |   |   |   |   |   |   |
      | X |   | X |   | X |   | X |
    X |   | X |   | X |   | X |   |
      | X |   | X |   | X |   | X
  """

Scenario: Player X makes a valid move
  Given the following board with players O and X
  """
    O |   | O |   | O |   | O |   |
      | O |   | O |   | O |   | O |
    O |   | O |   | O |   | O |   |
      |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
      | X |   | X |   | X |   | X |
    X |   | X |   | X |   | X |   |
      | X |   | X |   | X |   | X
  """
  When player X makes a move from 5,1 to 4,2
  Then the board should look like this
  """
    O |   | O |   | O |   | O |   |
      | O |   | O |   | O |   | O |
    O |   | O |   | O |   | O |   |
      |   |   |   |   |   |   |   |
      |   | X |   |   |   |   |   |
      |   |   | X |   | X |   | X |
    X |   | X |   | X |   | X |   |
      | X |   | X |   | X |   | X
  """

Scenario: Player O makes a valid move
  Given the following board with players O and X
  """
    O |   | O |   | O |   | O |   |
      | O |   | O |   | O |   | O |
    O |   | O |   | O |   | O |   |
      |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
      | X |   | X |   | X |   | X |
    X |   | X |   | X |   | X |   |
      | X |   | X |   | X |   | X
  """
  When player O makes a move from 2,6 to 3,5
  Then the board should look like this
  """
    O |   | O |   | O |   | O |   |
      | O |   | O |   | O |   | O |
    O |   | O |   | O |   |   |   |
      |   |   |   |   | O |   |   |
      |   |   |   |   |   |   |   |
      | X |   | X |   | X |   | X |
    X |   | X |   | X |   | X |   |
      | X |   | X |   | X |   | X
  """

Scenario Outline: Validating regular moves (excluding king moves and capturing)
  Given the following board with players O and X
  """
      |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
    O |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
      | X |   |   |   | O |   |   |
    X |   |   |   | X |   |   |   |
      |   |   |   |   |   |   |  
  """
  When player <player> makes a move from <sr>,<sc> to <dr>,<dc>
  Then the move should fail with error '<error>'

  Examples:
    | player | sr  | sc  | dr | dc  | error                                                                  |
    | X      | -1  | 8   | 5  | 0   | Source location (-1,8) is out of bounds                  |
    | X      | 5   | 1   | 1  | -1  | Destination location (1,-1) is out of bounds             |
    | X      | 8   | 8   | 5  | 0   | Source location (8,8) is out of bounds                   |
    | X      | 5   | 1   | 8  | 8   | Destination location (8,8) is out of bounds              |
    | X      | 0   | 0   | 1  | 1   | Source location (0,0) does not contain a piece           |
    | X      | 2   | 0   | 3  | 1   | Player X does not own the piece at source location (2,0) |
    | X      | 6   | 0   | 5  | 1   | Destination location (5,1) is not empty                  |
    | X      | 5   | 1   | 6  | 2   | Regular pieces cannot move backwards                     |
    | O      | 2   | 0   | 1  | 1   | Regular pieces cannot move backwards                     | 
    | X      | 5   | 1   | 5  | 2   | Pieces can only move diagonally                          |
    | X      | 5   | 1   | 4  | 1   | Pieces can only move diagonally                          |
    | X      | 5   | 1   | 4  | 4   | Pieces can only move diagonally                          |
    | X      | 5   | 1   | 3  | 3   | Pieces can only move one square when not capturing       |
    | X      | 6   | 4   | 3  | 7   | Pieces can only move one square when not capturing       |

Scenario: Crowning regular piece from player X
  Given the following board with players O and X
  """
      |   |   |   |   |   |   |   |
      | X |   |   |   |   |   |   |
    O |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |  
  """
  When player X makes a move from 1,1 to 0,0
  Then the board should look like this
  """
    X$ |   |   |   |   |   |   |   |
       |   |   |   |   |   |   |   |
     O |   |   |   |   |   |   |   |
       |   |   |   |   |   |   |   |
       |   |   |   |   |   |   |   |
       |   |   |   |   |   |   |   |
       |   |   |   |   |   |   |   |
       |   |   |   |   |   |   |  
  """
  And the piece at 0,0 should be a king piece

Scenario: Crowning regular piece from player O
  Given the following board with players O and X
  """
      |   |   |   |   |   |   |   |
      | X |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
      |   |   |   |   |   | O |   |
      |   |   |   |   |   |   |  
  """
  When player O makes a move from 6,6 to 7,7
  Then the board should look like this
  """
      |   |   |   |   |   |   |   |
      | X |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
      |   |   |   |   |   |   | O$
  """
  And the piece at 7,7 should be a king piece

Scenario: Generating a board with king piece already in it
  Given the following board with players O and X
  """
   O$ |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |  
  """
  Then the board should look like this
  """
   O$ |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |   |
      |   |   |   |   |   |   |  
  """

Scenario: King piece from player X can move backwards
  Given the following board with players O and X
  """
    |    |   |   |   |   |   |   |
    | X$ |   |   |   |   |   |   |
    |    |   |   |   |   |   |   |
    |    |   |   |   |   |   |   |
    |    |   |   |   |   |   |   |
    |    |   |   |   |   |   |   |
    |    |   |   |   |   | O |   |
    |    |   |   |   |   |   |  
  """
  When player X makes a move from 1,1 to 2,2
  Then the board should look like this
  """
    |   |    |   |   |   |   |   |
    |   |    |   |   |   |   |   |
    |   | X$ |   |   |   |   |   |
    |   |    |   |   |   |   |   |
    |   |    |   |   |   |   |   |
    |   |    |   |   |   |   |   |
    |   |    |   |   |   | O |   |
    |   |    |   |   |   |   |  
  """

Scenario: King piece from player O can move backwards
  Given the following board with players O and X
  """
    |   |   |   |   |   |    |   |
    | X |   |   |   |   |    |   |
    |   |   |   |   |   |    |   |
    |   |   |   |   |   |    |   |
    |   |   |   |   |   |    |   |
    |   |   |   |   |   |    |   |
    |   |   |   |   |   | O$ |   |
    |   |   |   |   |   |    |  
  """
  When player O makes a move from 6,6 to 5,5
  Then the board should look like this
  """
    |   |   |   |   |    |   |   |
    | X |   |   |   |    |   |   |
    |   |   |   |   |    |   |   |
    |   |   |   |   |    |   |   |
    |   |   |   |   |    |   |   |
    |   |   |   |   | O$ |   |   |
    |   |   |   |   |    |   |   |
    |   |   |   |   |    |   |  
  """

Scenario: Player X captures a piece from player O
  Given the following board with players O and X
  """
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   | O |   |   |   |   |   |
    |   |   | X |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |  
  """
  When player X makes a move from 3,3 to 1,1
  Then the board should look like this
  """
    |   |   |   |   |   |   |   |
    | X |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |  
  """

Scenario: Player O captures a piece from player X
  Given the following board with players O and X
  """
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   | O |   |   |   |   |   |
    |   |   | X |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |  
  """
  When player O makes a move from 2,2 to 4,4
  Then the board should look like this
  """
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   | O |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |  
  """

Scenario: Player X captures a piece from player O using a king piece
  Given the following board with players O and X
  """
    |    |   |   |   |   |   |   |
    | X$ |   |   |   |   |   |   |
    |    | O |   |   |   |   |   |
    |    |   |   |   |   |   |   |
    |    |   |   |   |   |   |   |
    |    |   |   |   |   |   |   |
    |    |   |   |   |   |   |   |
    |    |   |   |   |   |   |  
  """
  When player X makes a move from 1,1 to 3,3
  Then the board should look like this
  """
    |   |   |    |   |   |   |   |
    |   |   |    |   |   |   |   |
    |   |   |    |   |   |   |   |
    |   |   | X$ |   |   |   |   |
    |   |   |    |   |   |   |   |
    |   |   |    |   |   |   |   |
    |   |   |    |   |   |   |   |
    |   |   |    |   |   |   |  
  """

Scenario: Player O captures a piece from player X using a king piece
  Given the following board with players O and X
  """
    |   |    |   |   |   |   |   |
    | X |    |   |   |   |   |   |
    |   | O$ |   |   |   |   |   |
    |   |    |   |   |   |   |   |
    |   |    |   |   |   |   |   |
    |   |    |   |   |   |   |   |
    |   |    |   |   |   |   |   |
    |   |    |   |   |   |   |  
  """
  When player O makes a move from 2,2 to 0,0
  Then the board should look like this
  """
  O$ |   |   |   |   |   |   |   |
     |   |   |   |   |   |   |   |
     |   |   |   |   |   |   |   |
     |   |   |   |   |   |   |   |
     |   |   |   |   |   |   |   |
     |   |   |   |   |   |   |   |
     |   |   |   |   |   |   |   |
     |   |   |   |   |   |   |  
  """