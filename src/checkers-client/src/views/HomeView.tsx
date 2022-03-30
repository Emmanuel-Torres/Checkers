import { FC } from 'react';
import { Link } from 'react-router-dom';
import Login from '../components/auth/Login';
import { useStoreSelector } from '../store';

const HomeView: FC = (): JSX.Element => {
    const user = useStoreSelector(store => store.auth.userProfile);


    return (
        <>
            <div className='container border border-dark border-5 rounded p-2 my-2 shadow text-center'>
                <h2>Welcome!</h2>
                <p>This is a checkers project that allows you to play checkers online with your friends!</p>
                <Link to="/game" className='btn btn-primary my-2 shadow'>Play Now!</Link>
                <br/>
                <Login />
                <p>{user?.email}</p>
            </div>
        </>
    )
};

export default HomeView;