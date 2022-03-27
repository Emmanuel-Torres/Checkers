import { FC } from 'react';
import Login from '../components/auth/Login';
import Logout from '../components/auth/Logout';
import { useStoreSelector } from '../store';

const Home: FC = (): JSX.Element => {
    const token = useStoreSelector(store => store.auth.userToken);
    const profile = useStoreSelector(store => store.auth.userProfile);

    console.log(profile);

    return (
        <>
            <Login />
            <Logout />
            <h2>Welcome!</h2>
            <p>This is a checkers project that allows you to play checkers online with your friends!</p>
            <p>{token}</p>
            <p>{profile?.familyName}</p>
        </>
    )
};

export default Home;