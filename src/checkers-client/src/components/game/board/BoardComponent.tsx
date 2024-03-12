import { FC, useState } from "react";
import Square from "../../../models/game/square";
import BoardLocation from "../../../models/game/location";
import SquareComponent from "../square/SquareComponent";
import styles from "./BoardComponent.module.css"
import MoveRequest from "../../../models/game/moveRequest";
import Piece from "../../../models/game/piece";
import gameService from "../../../services/gameService";

type Props = {
    board: Piece[];
    isReversed: boolean; //Is Room Owner
    yourId: string;
    currentTurnId: string;
}

const BoardComponent: FC<Props> = (props): JSX.Element => {
    // const [source, setSource] = useState<Square>();

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

    const getSquareColor = (index: number): string => {
        const row = Math.floor(index / 8);
        const colum = index % 8;

        if (row % 2 == colum % 2)
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

    const getIsHighlighted = (index: number): boolean => {
        return props.currentTurnId === props.yourId && props.board[index]?.ownerId === props.yourId && gameService.hasValidMoves(index, props.board, props.isReversed);
    }

    const boardStyles = styles.board + (props.isReversed ? " " + styles['board-reversed'] : '');

    return (
        <div className={styles.container}>
            <div className={boardStyles}>
                {props.board.map((p, i) => {
                    // const isSelected = p.location.column === source?.location.column && p.location.row === source?.location.row;
                    return <SquareComponent isHighlighted={getIsHighlighted(i)} pieceColor={getPieceColor(p?.ownerId)} pieceState={p?.state} color={getSquareColor(i)} key={i} isReversed={props.isReversed} />
                })}
            </div>
        </div>
    )
}

export default BoardComponent;