import { FC } from 'react';
import { Link } from 'react-router-dom';
import styles from "./HomeView.module.css"

const HomeView: FC = (): JSX.Element => {
    return (
        <div className={styles.container}>
            <div className={styles.main}>
                <h1>Checkers Online</h1>
                <p>Ready to challenge your friends? This Checkers Online project lets you play checkers against your friends in real time!</p>
                <Link to="/game" className={styles["play-button"]}>Play Now!</Link>
            </div>
            <div className={styles.resources}>
                <h2>Resources</h2>
                <ul>
                    <li>
                        <a href='https://github.com/Emmanuel-Torres/Checkers'
                            target='_blank'
                            rel='noopener noreferrer'>
                            Github Repository
                        </a>
                    </li>
                    <li>
                        <a href='https://www.emmanueltorreshn.com'
                            target='_blank'
                            rel='noopener noreferrer'>
                            Personal Portfolio
                        </a>
                    </li>
                </ul>
            </div>
        </div>
    )
};

export default HomeView;