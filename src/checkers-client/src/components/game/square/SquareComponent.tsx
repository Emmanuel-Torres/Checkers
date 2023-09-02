import { FC } from "react";
import Square from "../../../game-models/square";
import styles from "./SquareComponent.module.css"
import PieceComponent from "../piece/PieceComponent";
import ValidMoveIndicatorComponent from "../valid-move-indicator/ValidMoveIndicatorComponent";

type Props = {
    square: Square;
    isValidMoveLocation: boolean;
    isReversed: boolean;
    onSquareClicked: (square: Square) => void;
}

const SquareComponent: FC<Props> = (props): JSX.Element => {
    const squareStyle = styles.square + " " + styles[`bg-${props.square.color}`] + " " + (props.isReversed && styles['square-reversed']);

    return (
        <>
            <div className={squareStyle} onClick={() => props.onSquareClicked(props.square)}>
                {props.isValidMoveLocation &&
                    <ValidMoveIndicatorComponent />}
                {props.square.isOccupied && <PieceComponent piece={props.square.piece}/>}
            </div>
        </>
    )
}

export default SquareComponent;