import { FC, useState } from 'react';
import RoomInfo from "../../../models/room/roomInfo";
import styles from "./RoomInfoComponent.module.css"
import crownIcon from "../../../assets/crown-icon.svg"
import GameInfo from '../../../models/game/gameInfo';

type Props = {
    roomInfo: RoomInfo;
    gameInfo?: GameInfo;
    isRoomOwner: boolean;
    onStartGame: () => void;
}

const RoomInfoComponent: FC<Props> = (props): JSX.Element => {
    const [tooltipText, setTooltipText] = useState<string>("Click to copy");

    const roomIdClicked = () => {
        navigator.clipboard.writeText(props.roomInfo.roomId);
        setTooltipText("Code copied");
        setTimeout(() => setTooltipText("Click to copy"), 5000);
    }

    const roomExists = props.roomInfo;
    const guestExists = roomExists && props.roomInfo.roomGuest;
    const gameExists = props.gameInfo;
    const gameEnded = props.gameInfo && props.gameInfo.winner;

    return <>
        <div className={styles.container}>
            <div className={styles.owner}>
                <img className={styles.icon} src={crownIcon} alt='crown' />
                <h2>{props.roomInfo.roomOwner.name}</h2>
            </div>
            <h3 className={styles["room-id"]}>Room ID</h3>
            <h2 className={styles.room} onClick={roomIdClicked}>{props.roomInfo.roomId}<span className={styles["tooltip-text"]}>{tooltipText}</span></h2>
            {guestExists && <h2 className={styles.guest}>{props.roomInfo.roomGuest?.name}</h2>}
            {!guestExists && <div className={styles.placeholder} />}
        </div>
        {guestExists && props.isRoomOwner && !gameExists && <button className={styles.button} onClick={props.onStartGame}>Start Game</button>}
        {guestExists && props.isRoomOwner && gameEnded && <button className={styles.button} onClick={props.onStartGame}>Start New Game</button>}
        {!guestExists && <div className={styles.instructions}>
            <h3>You successfully created a room! To invite a player do the following:</h3>
            <ol>
                <li>Click or Tap the Room ID to copy it</li>
                <li>Send the Room ID to another player</li>
                <li>The other player must Join Room using the Room ID</li>
            </ol>
        </div>}
        {guestExists && !props.isRoomOwner && !gameExists && <h3 className={styles.waiting}>Waiting for room host to start the game.</h3>}
    </>
}

export default RoomInfoComponent;