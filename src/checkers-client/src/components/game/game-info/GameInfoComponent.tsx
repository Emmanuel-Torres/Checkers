import { FC } from 'react'
import styles from './GameInfoComponent.module.css';

type Props = {
    yourColor: string;
    opponentColor: string;
}

const GameInfoComponent: FC<Props> = (props): JSX.Element => {
    const yourStyle = styles[`left-${props.yourColor}`];
    const theirStyle = styles[`right-${props.opponentColor}`];

    return (
    <div className={styles.container}>
        <h2 className={yourStyle}>You</h2>
        <h2 className={theirStyle}>Them</h2>
    </div>
    );
}

export default GameInfoComponent;