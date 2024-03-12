import Piece from "../models/game/piece";

const hasValidMoves = (index: number, board: Piece[], isRoomOwner: boolean): boolean => {
    const piece = board[index];
    if (!piece) {
        return false;
    }
    const forward = isRoomOwner ? 1 : -1;
    const backward = forward * -1;

    const result = hasAvailableNeighbor(index, board, forward);
    if (result || piece.state === "Regular")
        return result;

    return hasAvailableNeighbor(index, board, backward);
}

const hasAvailableNeighbor = (index: number, board: Piece[], rowDirection: number): boolean => {
    const piece = board[index];
    const row = Math.floor(index / 8);
    const colum = index % 8;
    for(let i = 0; i < 2; i++) {
        const columnDirection = i === 0 ? -1 : 1;
        for (let j = 1; j <= 2; j++) {
            const next_r = row + rowDirection * j;
            const next_c = colum + columnDirection * j;

            if (next_r < 0 || next_r > 7 || next_c < 0 || next_c > 7)
                continue;

            const nextIndex = next_r * 8 + next_c;
            const tempPiece = board[nextIndex];

            if (!tempPiece)
                return true;

            if (tempPiece.ownerId === piece.ownerId)
                break;
        }
    }

    return false;
}

const gameService = {
    hasValidMoves
}

export default gameService;