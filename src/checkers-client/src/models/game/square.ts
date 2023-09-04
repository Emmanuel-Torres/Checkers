import Piece from "./piece";
import BoardLocation from "./location";

export default class Square {
  public readonly location: BoardLocation;
  public readonly color: string;
  public readonly isOccupied: boolean;
  public readonly piece?: Piece;

  constructor(location: BoardLocation, color: string, isOccupied: boolean, piece?: Piece) {
    this.location = location;
    this.color = color;
    this.isOccupied = isOccupied;
    this.piece = piece;
  }
}
