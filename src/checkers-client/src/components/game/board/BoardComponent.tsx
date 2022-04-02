import { FC, useEffect, useState } from "react";
import Square from "../../../game-models/square";
import BoardLocation from "../../../game-models/location";
import SquareComponent from "../square/SquareComponent";
import styles from "./BoardComponent.module.css"
import MoveRequest from "../../../game-models/moveRequest";

type Props = {
    board: Square[];
    validLocations: BoardLocation[];
    onGetValidMoves: (location: BoardLocation) => void;
    // onMakeMove: (request: MoveRequest) => void;
}

const BoardComponent: FC<Props> = (props): JSX.Element => {
    const [source, setSource] = useState<Square>();
    // const [validLocations, setValidLocation] = useState<BoardLocation[]>([]);

    const squareSelected = (square: Square) => {
        console.log("Here", source, props.validLocations);
        if (!source ||
            !props.validLocations.find(l => square.location.row === l.row && square.location.column === l.column)) {
            setSource(square);
            props.onGetValidMoves(square.location);
            return;
        }
    }

    return (
        <div className={styles.board}>
            {props.board.map((s, i) => {
                return <SquareComponent square={s} onSquareClicked={squareSelected} />
            })}
        </div>
    )
}

export default BoardComponent;