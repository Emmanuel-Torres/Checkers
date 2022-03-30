import { FC } from "react";
import Square from "../../../game-models/square";
import SquareComponent from "../square/SquareComponent";
import styles from "./BoardComponent.module.css"

type Props = {
    board: Square[]
}

const BoardComponent: FC<Props> = (props): JSX.Element => {
    console.log(props.board);

    return (
        <div className={styles.board}>
            {props.board.map((s, i) => {
                return <><SquareComponent square={s} /></>
            })}
        </div>
    )
}

export default BoardComponent;