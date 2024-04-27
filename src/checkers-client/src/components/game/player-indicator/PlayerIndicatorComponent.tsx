import { FC } from "react";
import styles from "./PlayerIndicatorComponent.module.css";
import GameInfo from "../../../models/game/gameInfo";
import Player from "../../../models/game/player";

type Props = {
  gameInfo: GameInfo;
  player: Player;
};

const PlayerIndicatorComponent: FC<Props> = (props): JSX.Element => {
  const gameEnded = props.gameInfo.winner;
  const yourTurn = !gameEnded && props.gameInfo.nextPlayerTurn.playerId === props.player?.playerId;

  return (
    <div className={styles.container}>
      {gameEnded && 
          <h2>{props.gameInfo.winner?.name} won</h2>}
      {yourTurn && 
          <h2>Your Turn</h2>}
      {!yourTurn && 
          <h2 className={styles["opponent-Text"]}>{props.gameInfo.nextPlayerTurn.name} is moving</h2>}
    </div>
  );
};

export default PlayerIndicatorComponent;
