import { FC } from "react";
import JoinRoomComponent from "../../components/room/join-room/JoinRoomComponent";
import CreateRoomComponent from "../../components/room/create-room/CreateRoomComponent";
import styles from "./JoinView.module.css"

type Props = {
    onJoinRoom: (roomId: string, name: string) => void;
    onCreateRoom: (name: string) => void;
}

const JoinView: FC<Props> = (props): JSX.Element => {
    return (
        <div className={styles.container}>
            <CreateRoomComponent onCreateRoom={props.onCreateRoom}/>
            <JoinRoomComponent onJoinRoom={props.onJoinRoom}/>
        </div>
    )
}

export default JoinView;