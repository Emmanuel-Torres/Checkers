import { FC } from 'react';
import { useStoreSelector } from '../store';

const Home: FC = (): JSX.Element => {
    const token = useStoreSelector(store => store.auth.userToken);
    
    return (
        <>
            <h2>Welcome!</h2>
            <p>This is a checkers project that allows you to play checkers online with your friends!</p>
            <p>{token}</p>
        </>
    )
};

export default Home;