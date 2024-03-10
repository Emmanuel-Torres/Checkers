import { FC } from 'react';
import RoomInfo from "../models/room/roomInfo";

type Props = {
    roomInfo: RoomInfo;
}

const RoomView: FC<Props> = (props): JSX.Element => {
    return <>
        <h2>Room {props.roomInfo.roomId}</h2>
        <h2>Room Owner: {props.roomInfo.roomOwner.name}</h2>
    </>
}

export default RoomView;