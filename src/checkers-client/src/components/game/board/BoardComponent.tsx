import { FC, useState } from "react";
import SquareComponent from "../square/SquareComponent";
import styles from "./BoardComponent.module.css"
import Piece from "../../../models/game/piece";
import gameService from "../../../services/gameService";
import Location from "../../../models/game/location";
import Move from "../../../models/game/move";

type Props = {
    board: Piece[][];
    isReversed: boolean; //true for room owner
    yourId: string;
    currentTurnId: string;
    getValidMoves: (source: Location) => { destination: Location, moveSeq: Move[] }[];
    makeMove: (moves: Move[]) => void;
}

const BoardComponent: FC<Props> = (props): JSX.Element => {
    const [source, setSource] = useState<Location>();
    const [validMoves, setValidMoves] = useState<{ destination: Location, moveSeq: Move[] }[]>([]);

    const selectSquare = (row: number, column: number) => {
        if (props.board[row][column] && props.board[row][column]?.ownerId !== props.yourId)
            return;

        const moves = validMoves.filter(m => m.destination.row === row && m.destination.column === column);
        console.log(moves);
        if (!source || moves.length === 0) {
            if (gameService.hasValidMoves(row, column, props.board, props.isReversed)) {
                setSource(new Location(row, column));
                const moves = props.getValidMoves(new Location(row, column));
                setValidMoves(moves);
            }
            return
        }

        if (moves.length === 1) {
            props.makeMove(moves[0].moveSeq);
            return;
        }

        console.log("move");
    }

    const getSquareColor = (row: number, column: number): string => {
        if (row % 2 == column % 2)
            return "Black"

        return "White";
    }

    const getPieceColor = (ownerId: string): string | undefined => {
        if (ownerId === undefined || ownerId === null) {
            return undefined;
        }

        if (ownerId === props.yourId)
            return "Black";

        return "White";
    }

    const getIsHighlighted = (row: number, column: number): boolean => {
        if (source === undefined) {
            const yourTurn = props.currentTurnId === props.yourId;
            const yourPiece = props.board[row][column]?.ownerId === props.yourId;
            const hasValidMoves = gameService.hasValidMoves(row, column, props.board, props.isReversed);
            return yourTurn && yourPiece && hasValidMoves;
        }

        return source.row === row && source.column === column;
    }

    const getHasMoveIndicator = (row: number, column: number): boolean => {
        const res = validMoves?.find(m => m.destination.row === row && m.destination.column === column);
        return res !== undefined;
    }

    const boardStyles = styles.board + (props.isReversed ? " " + styles['board-reversed'] : '');

    return (
        <div className={styles.container}>
            <div className={boardStyles}>
                {props.board.map((row, r) => {
                    return row.map((p, c) => {
                        return <SquareComponent onSquareClicked={() => selectSquare(r, c)}
                            isValidMoveLocation={getHasMoveIndicator(r, c)}
                            isHighlighted={getIsHighlighted(r, c)}
                            pieceColor={getPieceColor(p?.ownerId)}
                            pieceState={p?.state} color={getSquareColor(r, c)}
                            isReversed={props.isReversed}
                            key={r + c} />

                    })
                })}
            </div>
        </div>
    )
}

export default BoardComponent;