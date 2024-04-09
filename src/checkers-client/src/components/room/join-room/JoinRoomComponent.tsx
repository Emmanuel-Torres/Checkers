import { ChangeEvent, FC, FormEvent, useState } from 'react';
import styles from './JoinRoomComponent.module.css'

type Props = {
    onJoinRoom: (roomId: string, name: string) => void;
}

const JoinRoomComponent: FC<Props> = (props): JSX.Element => {
    const [name, setName] = useState('');
    const [roomId, setRoomId] = useState('');

    const nameChangedHandler = (event: ChangeEvent<HTMLInputElement>) => {
        setName(event.target.value)
    }

    const roomIdChangedHandler = (event: ChangeEvent<HTMLInputElement>) => {
        setRoomId(event.target.value);
    }

    const submitFormHandler = (event: FormEvent) => {
        //TODO: validate inputs before form submission
        event.preventDefault();
        props.onJoinRoom(roomId, name);
    }
    return <>
        <div className={styles.container}>
            <h2 className={styles.header}>Join Room</h2>
            <form className={styles.form} onSubmit={submitFormHandler}>
                <label className={styles['form-label']}>Name</label>
                <input className={styles['form-input']} type='text' id='name' placeholder='e.g. Sally' value={name} onChange={nameChangedHandler} />
                <label className={styles['form-label']}>Room Id</label>
                <input className={styles['form-input']} type='text' id='room-code' placeholder='e.g. AB123' value={roomId} onChange={roomIdChangedHandler} />
                <button className={styles['form-button']} type='submit'>Join Room</button>
            </form>
        </div>
    </>
}

export default JoinRoomComponent;