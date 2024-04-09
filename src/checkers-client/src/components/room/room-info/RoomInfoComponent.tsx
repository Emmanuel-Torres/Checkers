import { FC, useState } from 'react';
import RoomInfo from "../../../models/room/roomInfo";
import styles from "./RoomInfoComponent.module.css"
import crownIcon from "../../../assets/crown-icon.svg"
import SpinnerComponent from '../../effects/spinner/SpinnerComponent';

type Props = {
    roomInfo: RoomInfo;
}

const RoomView: FC<Props> = (props): JSX.Element => {
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
        <div className={styles.guest}>
            <h2 className={styles[guestStyle]}>{props.roomInfo.roomGuest?.name ?? "Waiting"}</h2>
            {!props.roomInfo.roomGuest && <SpinnerComponent />}
        </div>
    </div>
}

export default RoomView;