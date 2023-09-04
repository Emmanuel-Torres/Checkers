export default class BoardLocation {
  public readonly row: number;
  public readonly column: number;

  constructor(row: number, column: number) {
    this.row = row;
    this.column = column;
  }
}
