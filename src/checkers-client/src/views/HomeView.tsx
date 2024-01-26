import { FC } from 'react';
import { Link } from 'react-router-dom';

const HomeView: FC = (): JSX.Element => {
    return (
        <>
            <div className='container border border-dark border-5 rounded p-2 my-2 shadow text-center bg-white'>
                <h2>Welcome!</h2>
                <p>This is a checkers project that allows you to play checkers online with your friends!</p>
                <Link to="/game" className='btn btn-primary my-2'>Play Now!</Link>
            </div>
        </>
    )
};

export default HomeView;