import { FC } from 'react';
import { Link } from 'react-router-dom';
import Login from '../components/auth/Login';
import { useStoreSelector } from '../store';

const HomeView: FC = (): JSX.Element => {
    const user = useStoreSelector(store => store.auth.userProfile);


    return (
        <>
            <h2>Welcome!</h2>
            <p>This is a checkers project that allows you to play checkers online with your friends!</p>
            <Link to="/game">Play Now!</Link>
            <Login />
            <p>{user?.email}</p>
        </>
    )
};

export default HomeView;