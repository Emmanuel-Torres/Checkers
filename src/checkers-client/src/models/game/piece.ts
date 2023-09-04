export default class Piece {
  public readonly color: string;
  public readonly state: string;

  constructor(color: string, state: string) {
    this.color = color;
    this.state = state;
  }
}
