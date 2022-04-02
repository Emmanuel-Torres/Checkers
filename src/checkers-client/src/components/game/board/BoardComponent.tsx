import { FC, useState } from "react";
import Square from "../../../game-models/square";
import BoardLocation from "../../../game-models/location";
import SquareComponent from "../square/SquareComponent";
import styles from "./BoardComponent.module.css"
import MoveRequest from "../../../game-models/moveRequest";

type Props = {
    board: Square[];
    validLocations: BoardLocation[];
    onGetValidMoves: (location: BoardLocation) => void;
    onMakeMove: (request: MoveRequest) => void;
}

const BoardComponent: FC<Props> = (props): JSX.Element => {
    const [source, setSource] = useState<Square>();

    // const validIndices = props.board.filter(s => props.validLocations)

    const squareSelected = (square: Square) => {
        if (!source ||
            !props.validLocations.find(l => square.location.row === l.row && square.location.column === l.column)) {
            setSource(square);
            props.onGetValidMoves(square.location);
            return;
        }

        props.onMakeMove(new MoveRequest(source.location, square.location));
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