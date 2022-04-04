import { FC } from "react";
import Square from "../../../game-models/square";
import styles from "./SquareComponent.module.css"
import crown from "../../../assets/crown.svg"

type Props = {
    square: Square;
    isValidMoveLocation: boolean;
    onSquareClicked: (square: Square) => void;
}

const SquareComponent: FC<Props> = (props): JSX.Element => {
    const squareStyle = styles.square + " " + styles[`bg-${props.square.color}`];
    const pieceStyle = styles.piece + " " + styles[`piece-${props.square.piece?.color}`];
    return (
        <>
            <div className={squareStyle} onClick={() => props.onSquareClicked(props.square)}>
                {props.isValidMoveLocation &&
                    <div className={styles["valid-move"]} />}
                {props.square.isOccupied &&
                    <div className={pieceStyle}>
                        {props.square.piece?.state === "King" && <img className={styles.crown} src={crown} alt="crown" />}
                    </div>}
            </div>
        </>
    )
}

export default SquareComponent;