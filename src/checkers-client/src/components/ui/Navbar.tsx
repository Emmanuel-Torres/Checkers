import { FC } from "react";
import { NavLink } from "react-router-dom";
// import Login from "../auth/Login";
// import Logout from "../auth/Logout";

const Navbar: FC = (): JSX.Element => {
    return (
        <nav>
            <NavLink to='/'>Checker's</NavLink>
            <div>
                <NavLink to='/'>Home</NavLink>
                <NavLink to='/secure'>Secure</NavLink>
            </div>
        </nav>
    )
}

export default Navbar;