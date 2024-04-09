import Piece from "./piece";
import Player from "./player";

export default class GameInfo {
  public readonly roomId: string;
  public readonly isGameOver: boolean;
  public readonly nextPlayerTurn: Player;
  public readonly board: Piece[][];
  public readonly winner?: Player;

  constructor(
    roomId: string,
    isGameOver: boolean,
    nextPlayerTurn: Player,
    board: Piece[][],
    winner?: Player
  ) {
    this.roomId = roomId;
    this.isGameOver = isGameOver;
    this.nextPlayerTurn = nextPlayerTurn;
    this.board = board;
    this.winner = winner;
  }
}
