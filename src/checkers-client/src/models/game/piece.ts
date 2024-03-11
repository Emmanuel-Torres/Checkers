export default class Piece {
  public readonly ownerId: string;
  public readonly state: string;

  constructor(ownerId: string, state: string) {
    this.ownerId = ownerId;
    this.state = state;
  }
}
