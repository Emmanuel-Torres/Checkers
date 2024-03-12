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
    isReversed: boolean;
    isHighlighted: boolean;
}

const SquareComponent: FC<Props> = (props): JSX.Element => {
    const colorStyle = " " + styles[`bg-${props.color}`];
    const reversedStyle = props.isReversed ? " " + styles['square-reversed'] : "";
    const highlightedStyle = props.isHighlighted ? " " + styles['square-highlighted'] : "";
    const squareStyle = styles.square +  colorStyle + reversedStyle + highlightedStyle;

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