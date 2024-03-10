import { FC } from "react";
import JoinRoomComponent from "../components/room/join-room/JoinRoomComponent";
import CreateRoomComponent from "../components/room/create-room/CreateRoomComponent";

type Props = {
    onJoinRoom: (name: string, roomId: string) => void;
    onCreateRoom: (name: string) => void;
}

const JoinView: FC<Props> = (props): JSX.Element => {
    return (
        <>
            <JoinRoomComponent onJoinRoom={props.onJoinRoom}/>
            <CreateRoomComponent onCreateRoom={props.onCreateRoom}/>
        </>
    )
}

export default JoinView;