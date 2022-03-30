export default class Piece {
  public readonly color: string;
  public readonly ownerId: string;
  public readonly state: string;

  constructor(color: string, ownerId: string, state: string) {
    this.color = color;
    this.ownerId = ownerId;
    this.state = state;
  }
}
