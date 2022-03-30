export default class Piece {
  public readonly color: string;
  public readonly ownerId: string;
  public readonly state: string;

  constructor(color: number, ownerId: string, state: number) {
    this.color = color == 0 ? "white" : "black";
    this.ownerId = ownerId;
    this.state = state == 0 ? "regular" : "King";
  }
}
