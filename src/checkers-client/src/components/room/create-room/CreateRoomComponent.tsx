import { ChangeEvent, FC, FormEvent, useState } from 'react';
import styles from './CreateRoomComponent.module.css'

type Props = {
    onCreateRoom: (name: string) => void;
}

const CreateRoomComponent: FC<Props> = (props): JSX.Element => {
    const [name, setName] = useState('');
    const [isNameTouched, setIsNameTouched] = useState(false);

    const isNameValid = name.trim().length > 0;
    const isNameFieldInvalid = !isNameValid && isNameTouched;

    const nameChangedHandler = (event: ChangeEvent<HTMLInputElement>) => {
        setName(event.target.value)
    }

    const submitFormHandler = (event: FormEvent) => {
        event.preventDefault();

        setIsNameTouched(true);
        if (!isNameValid) {
            return;
        }

        props.onCreateRoom(name);
    }
    return <>
        <div className={styles.container}>
            <h2 className={styles.header}>Create Room</h2>
            <form className={styles.form} onSubmit={submitFormHandler}>
                <label className={styles['form-label']}>Name</label>
                <input className={styles['form-input']} type='text' id='name' placeholder='e.g. Sally' value={name} onChange={nameChangedHandler} onBlur={() => setIsNameTouched(true)}/>
                {isNameFieldInvalid && <p className={styles['validation-error']}>*At least 1 character long</p>}
                <button className={styles['form-button']} type='submit'>Create Room</button>
            </form>
        </div>
    </>
}

export default CreateRoomComponent;