import { FC } from "react";
import { Link } from "react-router-dom";

const Navbar: FC = (): JSX.Element => {
    return (
        <nav className="navbar navbar-expand navbar-dark bg-dark shadow">
            <div className="container-fluid">
                <Link className="navbar-brand" to="/">Checker's</Link>
                <div className="navbar-nav">
                    {/* <div className='nav-item'>
                        <Link className="nav-link" to="/">Home</Link>
                    </div>
                    <div className='nav-item'>
                        <Link className="nav-link" to="/game">Play</Link>
                    </div> */}
                </div>
            </div>
        </nav>
    )
}

export default Navbar;