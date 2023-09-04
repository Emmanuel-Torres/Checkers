import { FC, useState } from "react";
import Square from "../../../models/game/square";
import BoardLocation from "../../../models/game/location";
import SquareComponent from "../square/SquareComponent";
import styles from "./BoardComponent.module.css"
import MoveRequest from "../../../models/game/moveRequest";

type Props = {
    board: Square[];
    validLocations: BoardLocation[];
    isReversed: boolean;
    onGetValidMoves: (location: BoardLocation) => void;
    onMakeMove: (request: MoveRequest) => void;
}

const BoardComponent: FC<Props> = (props): JSX.Element => {
    const [source, setSource] = useState<Square>();

    const validIndices: number[] = [];

    for(let i = 0; i < props.validLocations.length; i++) {
        const location = props.validLocations[i];
        for(let j = 0; j < props.board.length; j++) {
            const square = props.board[j];
            if (square.location.row === location.row && square.location.column === location.column) {
                validIndices.push(j);
            }
        }
    }

    const squareSelected = (square: Square) => {
        if (square.color === 'White') {
            return;
        }

        if (!source ||
            !props.validLocations.find(l => square.location.row === l.row && square.location.column === l.column)) {
            setSource(square);
            props.onGetValidMoves(square.location);
            return;
        }

        props.onMakeMove(new MoveRequest(source.location, square.location));
        setSource(undefined);
    }

    const boardStyles = styles.board + (props.isReversed ? " " + styles['board-reversed'] : '');

    return (
        <div className={styles.container}>
            <div className={boardStyles}>
                {props.board.map((s, i) => {
                    const isSelected = s.location.column === source?.location.column && s.location.row === source?.location.row;
                    return <SquareComponent key={i} square={s} isSelected={isSelected} isReversed={props.isReversed} onSquareClicked={squareSelected} isValidMoveLocation={validIndices.includes(i)}/>
                })}
            </div>
        </div>
    )
}

export default BoardComponent;