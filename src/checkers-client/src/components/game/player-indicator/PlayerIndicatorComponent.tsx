import { FC } from "react";
import styles from "./PlayerIndicatorComponent.module.css";
import PieceComponent from "../piece/PieceComponent";
import SpinnerComponent from "../../effects/spinner/SpinnerComponent";

type Props = {
  yourTurn: boolean;
};

const PlayerIndicatorComponent: FC<Props> = (props): JSX.Element => {
  return (
    <div className={styles.container}>
      {props.yourTurn 
        ? <h2>Your Turn</h2> 
        : <>
            <SpinnerComponent />
            <h2 className={styles["opponent-Text"]}>Waiting for opponent...</h2>
          </>}
    </div>
  );
};

export default PlayerIndicatorComponent;
