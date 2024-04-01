import { FC, useState } from "react";
import SquareComponent from "../square/SquareComponent";
import styles from "./BoardComponent.module.css"
import Piece from "../../../models/game/piece";
import gameService from "../../../services/gameService";
import Location from "../../../models/game/location";

type Props = {
    board: Piece[][];
    isReversed: boolean; //true for room owner
    yourId: string;
    currentTurnId: string;
}

const BoardComponent: FC<Props> = (props): JSX.Element => {
    const [source, setSource] = useState<Location>();

    // const validIndices: number[] = [];

    // for(let i = 0; i < props.validLocations.length; i++) {
    //     const location = props.validLocations[i];
    //     for(let j = 0; j < props.board.length; j++) {
    //         const square = props.board[j];
    //         if (square.location.row === location.row && square.location.column === location.column) {
    //             validIndices.push(j);
    //         }
    //     }
    // }

    // const squareSelected = (square: Square) => {
    //     if (square.color === 'White') {
    //         return;
    //     }

    //     if (!source ||
    //         !props.validLocations.find(l => square.location.row === l.row && square.location.column === l.column)) {
    //         setSource(square);
    //         props.onGetValidMoves(square.location);
    //         return;
    //     }

    //     props.onMakeMove(new MoveRequest(source.location, square.location));
    //     setSource(undefined);
    // }

    const squareSelected = (row: number, column: number) => {
        const piece = props.board[row][column];

        if (piece?.ownerId !== props.yourId)
            return;

        if (!gameService.hasValidMoves(row, column, props.board, props.isReversed))
            return;

        setSource(new Location(row, column));
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

    const boardStyles = styles.board + (props.isReversed ? " " + styles['board-reversed'] : '');

    return (
        <div className={styles.container}>
            <div className={boardStyles}>
                {props.board.map((row, r) => {
                    return row.map((p, c) => {
                        const isHighlighted = getIsHighlighted(r, c);
                        return <SquareComponent onSquareClicked={() => squareSelected(r, c)}
                                                isHighlighted={getIsHighlighted(r,c)}
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