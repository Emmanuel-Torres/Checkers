Feature: Game
    To ensure that my game logic is correct\

Scenario: Generating the board
    When I start a game with players O and X
    Then the following board should get created
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
