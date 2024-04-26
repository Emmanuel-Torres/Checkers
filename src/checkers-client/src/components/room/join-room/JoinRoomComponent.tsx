import { ChangeEvent, FC, FormEvent, useState } from 'react';
import styles from './JoinRoomComponent.module.css'

type Props = {
    onJoinRoom: (roomId: string, name: string) => void;
}

const JoinRoomComponent: FC<Props> = (props): JSX.Element => {
    const [name, setName] = useState('');
    const [roomId, setRoomId] = useState('');
    const [isNameTouched, setIsNameTouched] = useState(false);
    const [isRoomIdTouched, setIsRoomIdTouched] = useState(false);

    const isNameValid = name.trim().length > 0;
    const isRoomIdValid = roomId.trim().length == 5;
    const isNameFieldInvalid = !isNameValid && isNameTouched;
    const isRoomIdFieldInvalid = !isRoomIdValid && isRoomIdTouched;

    const isFormValid = isNameValid && isRoomIdValid;

    const nameChangedHandler = (event: ChangeEvent<HTMLInputElement>) => {
        setName(event.target.value)
    }

    const roomIdChangedHandler = (event: ChangeEvent<HTMLInputElement>) => {
        setRoomId(event.target.value.toUpperCase());
    }

    const submitFormHandler = (event: FormEvent) => {
        event.preventDefault();

        setIsNameTouched(true);
        setIsRoomIdTouched(true);

        if (!isFormValid) {
            return;
        }

        console.log(roomId);

        props.onJoinRoom(roomId, name);
    }

    const roomIdStyles = styles['form-input'] + " " + styles['room-id'];

    return <>
        <div className={styles.container}>
            <h2 className={styles.header}>Join Room</h2>
            <form className={styles.form} onSubmit={submitFormHandler}>
                <label className={styles['form-label']}>Name</label>
                <input className={styles['form-input']} type='text' id='name' placeholder='e.g. Sally' value={name} onChange={nameChangedHandler} onBlur={() => setIsNameTouched(true)}/>
                {isNameFieldInvalid && <p className={styles['validation-error']}>*At least 1 character long</p>}
                <label className={styles['form-label']}>Room Id</label>
                <input className={roomIdStyles} type='text' id='room-code' placeholder='e.g. AB123' value={roomId} onChange={roomIdChangedHandler} onBlur={() => setIsRoomIdTouched(true)}/>
                {isRoomIdFieldInvalid && <p className={styles['validation-error']}>*Must be 5 characters long</p>}
                <button className={styles['form-button']} type='submit'>Join Room</button>
            </form>
        </div>
    </>
}

export default JoinRoomComponent;