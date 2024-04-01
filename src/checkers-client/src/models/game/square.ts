import Piece from "./piece";
import Location from "./location";

export default class Square {
  public readonly location: Location;
  public readonly color: string;
  public readonly isOccupied: boolean;
  public readonly piece?: Piece;

  constructor(location: Location, color: string, isOccupied: boolean, piece?: Piece) {
    this.location = location;
    this.color = color;
    this.isOccupied = isOccupied;
    this.piece = piece;
  }
}
