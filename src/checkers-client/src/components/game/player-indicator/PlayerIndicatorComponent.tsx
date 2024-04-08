import { FC } from "react";
import styles from "./PlayerIndicatorComponent.module.css";
import PieceComponent from "../piece/PieceComponent";
import SpinnerComponent from "../../effects/spinner/SpinnerComponent";
import GameInfo from "../../../models/game/gameInfo";
import Player from "../../../models/game/player";

type Props = {
  gameInfo: GameInfo;
  player: Player;
};

const PlayerIndicatorComponent: FC<Props> = (props): JSX.Element => {
  return (
    <div className={styles.container}>
      {props.gameInfo.winner && 
          <h2>{props.gameInfo.winner?.name} won</h2>}
      {!props.gameInfo.winner &&
        props.gameInfo.nextPlayerTurn.playerId === props.player?.playerId && 
          <h2>Your Turn</h2>}
      {!props.gameInfo.winner &&
        props.gameInfo.nextPlayerTurn.playerId !== props.player?.playerId && 
          <h2 className={styles["opponent-Text"]}>{props.gameInfo.nextPlayerTurn.name} is moving</h2>}
    </div>
  );
};

export default PlayerIndicatorComponent;
