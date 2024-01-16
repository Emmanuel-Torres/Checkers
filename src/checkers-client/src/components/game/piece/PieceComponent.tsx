import { FC } from "react";
import Piece from "../../../models/game/piece";
import crown from "../../../assets/crown.svg";
import styles from "./PieceComponent.module.css";

type Props = {
  piece: Piece;
};

const PieceComponent: FC<Props> = (props): JSX.Element => {
  const pieceStyle = styles.piece + " " + styles[`piece-${props.piece.color}`];
  return (
    <>
      <div className={styles[`piece-Outline`]}>
        <div className={pieceStyle}>
          {props.piece.state === "King" && (
            <img
              className={styles.crown}
              src={crown}
              alt="crown"
              draggable="false"
            />
          )}
        </div>
      </div>
    </>
  );
};

export default PieceComponent;
