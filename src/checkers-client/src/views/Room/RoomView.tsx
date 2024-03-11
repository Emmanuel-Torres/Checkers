import { FC } from 'react';
import RoomInfo from "../../models/room/roomInfo";
import styles from "./RoomView.module.css"
import crownIcon from "../../assets/crown-icon.svg"
import SpinnerComponent from '../../components/effects/spinner/SpinnerComponent';

type Props = {
    roomInfo: RoomInfo;
}

const RoomView: FC<Props> = (props): JSX.Element => {
    const guestStyle = props.roomInfo.roomGuest === undefined ? "waiting" : "";

    return <div className={styles.container}>
        <div className={styles.owner}>
            <img className={styles.icon} src={crownIcon} alt='crown' />
            <h2>{props.roomInfo.roomOwner.name}</h2>
        </div>
        <h3 className={styles["room-id"]}>Room Id</h3>
        <h2 className={styles.room}>{props.roomInfo.roomId}</h2>
        <div className={styles.guest}>
            <h2 className={styles[guestStyle]}>{props.roomInfo.roomGuest?.name ?? "Waiting"}</h2>
            {!props.roomInfo.roomGuest && <SpinnerComponent />}
        </div>
    </div>
}

export default RoomView;