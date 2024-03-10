import { ChangeEvent, FC, FormEvent, useState } from 'react';
import styles from './JoinRoomComponent.module.css'

type Props = {
    onJoinRoom: (name: string, roomId: string) => void;
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
        props.onJoinRoom(name, roomId);
    }
    return <>
        <div className={styles.container}>
            <h2 className={styles.header}>Join Room</h2>
            <form className={styles.form} onSubmit={submitFormHandler}>
                <label className={styles['form-label']}>Name</label>
                <input className={styles['form-input']} type='text' id='name' value={name} onChange={nameChangedHandler} />
                <label className={styles['form-label']}>Room Id</label>
                <input className={styles['form-input']} type='text' id='room-code' value={roomId} onChange={roomIdChangedHandler} />
                <button className={styles['form-button']} type='submit'>Join Room</button>
            </form>
        </div>
    </>
}

export default JoinRoomComponent;