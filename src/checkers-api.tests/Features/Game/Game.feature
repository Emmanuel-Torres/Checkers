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
  Given the following board with players O and X and player X is moving
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
  Given the following board with players O and X and player X is moving
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
  When player X makes a move from '5,1 > 4,2'
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
  And player O should now be moving

Scenario: Player O makes a valid move
  Given the following board with players O and X and player O is moving
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
  When player O makes a move from '2,6 > 3,5'
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
  And player X should now be moving

Scenario Outline: Validating regular moves (excluding king moves and capturing)
  Given the following board with players O and X and player <turn> is moving
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
  When player <player> makes a move from '<request>'
  Then the move should fail with error '<error>'

  Examples:
  | turn | player | request    | error                                                    |
  | X    | X      | -1,8 > 5,0 | Source location (-1,8) is out of bounds                  |
  | X    | X      | 5,1 > 1,-1 | Destination location (1,-1) is out of bounds             |
  | X    | X      | 8,8 > 5,0  | Source location (8,8) is out of bounds                   |
  | X    | X      | 5,1 > 8,8  | Destination location (8,8) is out of bounds              |
  | X    | X      | 0,0 > 1,1  | Source location (0,0) does not contain a piece           |
  | X    | X      | 2,0 > 3,1  | Player X does not own the piece at source location (2,0) |
  | X    | X      | 6,0 > 5,1  | Destination location (5,1) is not empty                  |
  | X    | X      | 5,1 > 6,2  | Regular pieces cannot move backwards                     |
  | O    | O      | 2,0 > 1,1  | Regular pieces cannot move backwards                     | 
  | X    | X      | 5,1 > 5,2  | Pieces can only move diagonally                          |
  | X    | X      | 5,1 > 4,1  | Pieces can only move diagonally                          |
  | X    | X      | 5,1 > 4,4  | Pieces can only move diagonally                          |
  | X    | X      | 5,1 > 3,3  | Pieces can only move one square when not capturing       |
  | X    | X      | 6,4 > 3,7  | Pieces can only move one square when not capturing       |
  | O    | X      | 6,4 > 3,7  | Player X tried to move outside its turn                  |

Scenario: Crowning regular piece from player X
  Given the following board with players O and X and player X is moving
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
  When player X makes a move from '1,1 > 0,0'
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
  Given the following board with players O and X and player O is moving
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
  When player O makes a move from '6,6 > 7,7'
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
  Given the following board with players O and X and player O is moving
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
  Given the following board with players O and X and player X is moving
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
  When player X makes a move from '1,1 > 2,2'
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
  Given the following board with players O and X and player O is moving
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
  When player O makes a move from '6,6 > 5,5'
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
  Given the following board with players O and X and player X is moving
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
  When player X makes a move from '3,3 > 1,1'
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
  Given the following board with players O and X and player O is moving
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
  When player O makes a move from '2,2 > 4,4'
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
  Given the following board with players O and X and player X is moving
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
  When player X makes a move from '1,1 > 3,3'
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
  Given the following board with players O and X and player O is moving
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
  When player O makes a move from '2,2 > 0,0'
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

Scenario: Player X tries a straigt double jump when capturing
  Given the following board with players O and X and player X is moving
  """
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   | O |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   | O |   |   |   |
    |   |   |   |   | X |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |  
  """
  When player X makes a move from '5,5 > 3,3 > 1,1'
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

Scenario: Player O tries a straight triple jump when capturing
  Given the following board with players O and X and player O is moving
  """
  O |   |   |   |   |   |   |   |
    | X |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   | X |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   | X |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |  
  """
  When player O makes a move from '0,0 > 2,2 > 4,4 > 6,6'
  Then the board should look like this
    """
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   | O |   |
    |   |   |   |   |   |   |  
  """

Scenario: Player O tries a twisty triple jump when capturing
  Given the following board with players O and X and player O is moving
  """
  O |   |   |   |   |   |   |   |
    | X |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    | X |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    | X |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |  
  """
  When player O makes a move from '0,0 > 2,2 > 4,0 > 6,2'
  Then the board should look like this
    """
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   | O |   |   |   |   |   |
    |   |   |   |   |   |   |  
  """

Scenario: Player O tries an invalid double jump
  Given the following board with players O and X and player O is moving
  """
  O |   |   |   |   |   |   |   |
    | X |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   | X |   |   |   |   |
    |   |   |   | X |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |  
  """
  When player O makes a move from '0,0 > 2,2 > 5,5'
  Then the move should fail with error 'Pieces can only move one square when not capturing'

Scenario: Player O tries to double jump backwards with regular piece
  Given the following board with players O and X and player O is moving
  """
  O |   |   |   |   |   |   |   |
    | X |   | X |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |  
  """
  When player O makes a move from '0,0 > 2,2 > 0,4'
  Then the move should fail with error 'Regular pieces cannot move backwards'

Scenario: Player O tries to tripe jump backwards while crowning regular piece
  Given the following board with players O and X and player O is moving
  """
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   | X |   |
    | O |   |   |   |   |   |   |
    |   | X |   | X |   |   |   |
    |   |   |   |   |   |   |  
  """
  When player O makes a move from '5,1 > 7,3 > 5,5 > 3,7'
  Then the board should look like this
    """
    |   |   |   |   |   |   |    |
    |   |   |   |   |   |   |    |
    |   |   |   |   |   |   |    |
    |   |   |   |   |   |   | O$ |
    |   |   |   |   |   |   |    |
    |   |   |   |   |   |   |    |
    |   |   |   |   |   |   |    |
    |   |   |   |   |   |   |   
  """

Scenario: Player X captures all player O pieces
  Given the following board with players O and X and player X is moving
  """
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   | O |   |   |   |   |   |
    | X |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |  
  """
  When player X makes a move from '5,1 > 3,3'
  Then the board should look like this
  """
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   | X |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |   |
    |   |   |   |   |   |   |  
  """
  And player X won the game

# Scenario: Player X cannot make a valid move
#   Given the following board with players O and X and player O is moving
#   """
#     |   |   |   |   |   |   |   |
#     |   |   |   |   |   |   |   |
#     |   |   |   |   |   |   |   |
#     | O |   |   |   |   |   |   |
#     |   |   |   |   |   |   |   |
#     | O |   |   |   |   |   |   |
#   X |   |   |   |   |   |   |   |
#     |   |   |   |   |   |   |  
#   """
#   When player O makes a move from '3,1 > 4,2'
#   Then the board should look like this
#   """
#     |   |   |   |   |   |   |   |
#     |   |   |   |   |   |   |   |
#     |   |   |   |   |   |   |   |
#     |   |   |   |   |   |   |   |
#     |   | O |   |   |   |   |   |
#     | O |   |   |   |   |   |   |
#   X |   |   |   |   |   |   |   |
#     |   |   |   |   |   |   |  
#   """
#   And player O won the game
