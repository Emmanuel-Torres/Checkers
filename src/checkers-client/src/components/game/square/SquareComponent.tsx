import { FC } from "react";
import Square from "../../../game-models/square";
import styles from "./SquareComponent.module.css"

type Props = {
    square: Square;
    onSquareClicked: (square: Square) => void;
}

const SquareComponent: FC<Props> = (props): JSX.Element => {
    const squareStyle = styles.square + " " + styles[`bg-${props.square.color}`]
    const pieceStyle = styles.piece + " " + styles[`piece-${props.square.piece?.color}`];
    return (
        <div className={squareStyle} onClick={() => props.onSquareClicked(props.square)}>
            {props.square.isOccupied && <div className={pieceStyle}></div>}
        </div>
    )
}

export default SquareComponent;