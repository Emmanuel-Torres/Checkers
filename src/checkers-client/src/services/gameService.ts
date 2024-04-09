import Piece from "../models/game/piece";

const hasValidMoves = (
  row: number,
  column: number,
  board: Piece[][],
  isRoomOwner: boolean
): boolean => {
  const piece = board[row][column];
  if (!piece) {
    return false;
  }
  const forward = isRoomOwner ? 1 : -1;
  const backward = forward * -1;

  const result = hasAvailableNeighbor(row, column, board, forward);
  if (result || piece.state === "Regular") return result;

  return hasAvailableNeighbor(row, column, board, backward);
};

const hasAvailableNeighbor = (
  row: number,
  column: number,
  board: Piece[][],
  rowDirection: number
): boolean => {
  const piece = board[row][column];
  for (let i = 0; i < 2; i++) {
    const columnDirection = i === 0 ? -1 : 1;
    for (let j = 1; j <= 2; j++) {
      const next_r = row + rowDirection * j;
      const next_c = column + columnDirection * j;

      if (next_r < 0 || next_r > 7 || next_c < 0 || next_c > 7) continue;

      const tempPiece = board[next_r][next_c];

      if (!tempPiece) return true;

      if (tempPiece.ownerId === piece.ownerId) break;
    }
  }

  return false;
};

const gameService = {
  hasValidMoves
};

export default gameService;
