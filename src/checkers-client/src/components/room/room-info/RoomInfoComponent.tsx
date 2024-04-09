import { FC, useState } from 'react';
import RoomInfo from "../../../models/room/roomInfo";
import styles from "./RoomInfoComponent.module.css"
import crownIcon from "../../../assets/crown-icon.svg"
import GameInfo from '../../../models/game/gameInfo';

type Props = {
    roomInfo: RoomInfo;
    gameInfo?: GameInfo;
}

const RoomInfoComponent: FC<Props> = (props): JSX.Element => {
    const [tooltipText, setTooltipText] = useState<string>("Click to copy")
    const guestStyle = props.roomInfo.roomGuest === undefined ? "waiting" : "";

    const roomIdClicked = () => {
        navigator.clipboard.writeText(props.roomInfo.roomId);
        setTooltipText("Code copied");
        setTimeout(() => setTooltipText("Click to copy"), 5000);
    }

    return <div className={styles.container}>
        <div className={styles.owner}>
            <img className={styles.icon} src={crownIcon} alt='crown' />
            <h2>{props.roomInfo.roomOwner.name}</h2>
        </div>
        <h3 className={styles["room-id"]}>Room Id</h3>
        <h2 className={styles.room} onClick={roomIdClicked}>{props.roomInfo.roomId}<span className={styles["tooltip-text"]}>{tooltipText}</span></h2>
        {props.roomInfo.roomGuest && <h2 className={styles.guest}>{props.roomInfo.roomGuest?.name}</h2>}
        {!props.roomInfo.roomGuest && <div className={styles.placeholder} />}
    </div>
}

export default RoomInfoComponent;