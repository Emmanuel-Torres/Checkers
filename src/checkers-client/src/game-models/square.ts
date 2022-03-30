import Piece from "./piece";

export default class Square {
  public readonly location: Location;
  public readonly color: string;
  public readonly isOccupied: boolean;
  public readonly piece?: Piece;

  constructor(location: Location, color: number, isOccupied: boolean, piece?: Piece) {
    this.location = location;
    this.color = color == 0 ? "white" : "black";
    this.isOccupied = isOccupied;
    this.piece = piece;
  }
}
