import { FC } from "react";
import Square from "../../../models/game/square";
import styles from "./SquareComponent.module.css"
import PieceComponent from "../piece/PieceComponent";
import ValidMoveIndicatorComponent from "../valid-move-indicator/ValidMoveIndicatorComponent";
import Piece from "../../../models/game/piece";

type Props = {
    color: string;
    pieceColor?: string;
    pieceState?: string;
    // isValidMoveLocation: boolean;
    isReversed: boolean;
    // isSelected: boolean;
    // onSquareClicked: (square: Square) => void;
}

const SquareComponent: FC<Props> = (props): JSX.Element => {
    const squareStyle = styles.square + " " + styles[`bg-${props.color}`] + (props.isReversed ? " " + styles['square-reversed'] : "");

    return (
        <>
            <div className={squareStyle}>
                {/* {props.isValidMoveLocation &&
                    <ValidMoveIndicatorComponent />} */}
                {props.pieceColor && <PieceComponent color={props.pieceColor!} state={props.pieceState!}/>}
            </div>
        </>
    )
}

export default SquareComponent;