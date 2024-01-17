import { FC } from "react";
import crown from "../../../assets/crown.svg";
import styles from "./PieceComponent.module.css";

type Props = {
  color: string;
  state: string;
};

const PieceComponent: FC<Props> = (props): JSX.Element => {
  const pieceStyle = styles.piece + " " + styles[`piece-${props.color}`];
  return (
    <>
      <div className={styles[`piece-Outline`]}>
        <div className={pieceStyle}>
          {props.state === "King" && (
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
