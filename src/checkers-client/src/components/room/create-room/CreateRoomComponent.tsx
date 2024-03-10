import { ChangeEvent, FC, FormEvent, useState } from 'react';
import styles from './CreateRoomComponent.module.css'

type Props = {
    onCreateRoom: (name: string) => void;
}

const CreateRoomComponent: FC<Props> = (props): JSX.Element => {
    const [name, setName] = useState('');

    const nameChangedHandler = (event: ChangeEvent<HTMLInputElement>) => {
        setName(event.target.value)
    }

    const submitFormHandler = (event: FormEvent) => {
        //TODO: validate inputs before form submission
        event.preventDefault();
        props.onCreateRoom(name);
    }
    return <>
        <div className={styles.container}>
            <h2 className={styles.header}>Create Room</h2>
            <form className={styles.form} onSubmit={submitFormHandler}>
                <label className={styles['form-label']}>Name</label>
                <input className={styles['form-input']} type='text' id='name' value={name} onChange={nameChangedHandler} />
                <button className={styles['form-button']} type='submit'>Create Room</button>
            </form>
        </div>
    </>
}

export default CreateRoomComponent;